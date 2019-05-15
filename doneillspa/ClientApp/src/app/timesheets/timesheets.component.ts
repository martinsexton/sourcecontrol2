import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Timesheet } from '../timesheet';
import { TimesheetEntry } from '../timesheetentry';
import { Project } from '../project';

import {
  ProjectService
} from '../shared/services/project.service';

import {
  TimesheetService
} from '../shared/services/timesheet.service';

declare var $: any;

@Component({
  selector: 'timesheets',
  templateUrl: './timesheets.component.html'
})

export class TimesheetComponent {
  public timesheets: Timesheet[];
  public projects: Project[];
  public timesheetExists = false;
  public loading = true;
  public selectedDate: string = null;
  public errors: string;

  //Default to Monday
  public selectedDay = "Mon";

  public monEntries: Array<TimesheetEntry> = new Array();
  public tueEntries: Array<TimesheetEntry> = new Array();
  public wedEntries: Array<TimesheetEntry> = new Array();
  public thursEntries: Array<TimesheetEntry> = new Array();
  public friEntries: Array<TimesheetEntry> = new Array();
  public satEntries: Array<TimesheetEntry> = new Array();

  activeTimeSheet: Timesheet;
 

  public newEntry: TimesheetEntry;
  public timesheetEntryToEdit: TimesheetEntry

  currentDate: Date;

  monday: DayOfWeek;
  tues: DayOfWeek;
  wed: DayOfWeek;
  thurs: DayOfWeek;
  fri: DayOfWeek;
  sat: DayOfWeek;

  displayAddTimesheet = false;
  displayEditTimesheet = false;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService, private _timesheetService : TimesheetService) {
    this.currentDate = new Date();

    var startOfWeek = new Date();
    startOfWeek.setDate(this.currentDate.getDate() - (this.currentDate.getDay() - 1))

    //Setting up default timesheet and timesheet entries
    this.activeTimeSheet = new Timesheet(0, localStorage.getItem('username'), localStorage.getItem('client_id'), localStorage.getItem('role'), startOfWeek,'New');
    this.newEntry = new TimesheetEntry("", "", "", "", "");

    this._projectService.getProjects().subscribe(result => {
      this.projects = result;
      if (this.projects) {
        this.newEntry.project = this.projects[0].name;
      }
    }, error => console.error(error));    

    this.refreshCalendarTabs(this.currentDate);

    this._timesheetService.getTimesheet(startOfWeek.getFullYear(), (startOfWeek.getMonth()+1), startOfWeek.getDate()).subscribe(result => {
      this.loading = false;
      this.timesheets = result;
      //Populate calendar if we find timesheets for this week
      this.populateWeeklyCalendar(this.timesheets, startOfWeek);
    }, error => {
      this.errors = error
      this.loading = false;
    });
  }

  setSelectedDate() {
    let newDate = new Date(this.selectedDate);

    var startOfWeek = new Date();
    startOfWeek.setDate(newDate.getDate() - (newDate.getDay() - 1));
    startOfWeek.setMonth(newDate.getMonth());

    this.refreshCalendarTabs(newDate);

    this._timesheetService.getTimesheet(startOfWeek.getFullYear(), (startOfWeek.getMonth() + 1), startOfWeek.getDate()).subscribe(result => {
      this.loading = false;
      this.timesheets = result;
      //Populate calendar if we find timesheets for this week
      this.populateWeeklyCalendar(this.timesheets, startOfWeek);
    }, error => this.errors = error);
  }
  populateWeeklyCalendar(array: Timesheet[], ws : Date) {
    //Clear Arrays before loading
    this.monEntries.length = 0;
    this.tueEntries.length = 0;
    this.wedEntries.length = 0;
    this.thursEntries.length = 0;
    this.friEntries.length = 0;
    this.satEntries.length = 0;

    //reset flag that determines if timesheet exists or not for the given date
    this.timesheetExists = false;
    this.activeTimeSheet.id = 0;
    this.activeTimeSheet.weekStarting = ws;

    for (let ts of array) {
      if (ts.owner == localStorage.getItem("client_id")) {
        this.activeTimeSheet.weekStarting = ts.weekStarting;
        this.activeTimeSheet.id = ts.id;
        this.activeTimeSheet.status = ts.status;
        this.timesheetExists = true;

        for (let item of ts.timesheetEntries) {
          if (item.day == "Mon") {
            this.monEntries.push(item);
          }
          else if (item.day == "Tue") {
            this.tueEntries.push(item);
          }
          else if (item.day == "Wed") {
            this.wedEntries.push(item);
          }
          else if (item.day == "Thurs") {
            this.thursEntries.push(item);
          }
          else if (item.day == "Fri") {
            this.friEntries.push(item);
          }
          else {
            this.satEntries.push(item);
          }
        }
        break;
      }
    }
  }


  refreshCalendarTabs(baseDate : Date) {
    let day = baseDate.getDay();

    var monday = new Date();
    monday.setDate(baseDate.getDate() - (day - 1))
    this.monday = new DayOfWeek("Mon", monday);

    var tuesday = new Date();
    tuesday.setDate(monday.getDate() + 1);
    this.tues = new DayOfWeek("Tue", tuesday);

    var wednesday = new Date();
    wednesday.setDate(tuesday.getDate() + 1);
    this.wed = new DayOfWeek("Wed", wednesday);

    var thursday = new Date();
    thursday.setDate(wednesday.getDate() + 1);
    this.thurs = new DayOfWeek("Thurs", thursday);

    var friday = new Date();
    friday.setDate(thursday.getDate() + 1);
    this.fri = new DayOfWeek("Fri", friday);

    var saturday = new Date();
    saturday.setDate(friday.getDate() + 1);
    this.sat = new DayOfWeek("Sat", saturday);
  }

  toggleDisplayAddTimesheet() {
    this.displayAddTimesheet = !this.displayAddTimesheet;
    if (this.displayAddTimesheet) {
      $("#myNewTimesheetModal").modal('show');
    } else {
      $("#myNewTimesheetModal").modal('hide');
    }
  }

  showEditTimesheet(entry: TimesheetEntry) {
    this.timesheetEntryToEdit = entry;
    this.displayEditTimesheet = true;
    if (this.displayEditTimesheet) {
      $("#myEditTimesheetModal").modal('show');
    } else {
      $("#myEditTimesheetModal").modal('hide');
    }
  }

  updateTimesheetEntry() {
    console.log('updating timesheetEntryToEdit')
    $("#myEditTimesheetModal").modal('hide');
    this._timesheetService.updateTimesheetEntry(this.timesheetEntryToEdit).subscribe(
      res => {
      }, error => this.errors = error);
  }

  removeTimesheetEntry(ts) {
    //We can only delete timesheet entries while the timesheet is in a status of new.
    if (this.activeTimeSheet.status == 'New' || this.activeTimeSheet.status == 'Rejected') {
      this._timesheetService.deleteTimesheetEntry(ts).subscribe(
        res => {
          console.log(res);
          if (this.selectedDay == "Mon") {
            this.removeFromArrayList(this.monEntries, ts);
          }
          else if (this.selectedDay == "Tue") {
            this.removeFromArrayList(this.tueEntries, ts);
          }
          else if (this.selectedDay == "Wed") {
            this.removeFromArrayList(this.wedEntries, ts);
          }
          else if (this.selectedDay == "Thurs") {
            this.removeFromArrayList(this.thursEntries, ts);
          }
          else if (this.selectedDay == "Fri") {
            this.removeFromArrayList(this.friEntries, ts);
          }
          else {
            this.removeFromArrayList(this.satEntries, ts);
          }
        }, error => this.errors = error);
    }
  }

  removeFromArrayList(array: TimesheetEntry[], ts: TimesheetEntry) {
    for (let item of array) {
      if (item.project == ts.project && item.startTime == ts.startTime && item.endTime == ts.endTime) {
        array.splice(array.indexOf(item), 1);
        break;
      }
    }
  }

  addTimesheetEntry() {
    let entry: TimesheetEntry = new TimesheetEntry(this.newEntry.project, this.selectedDay, this.newEntry.startTime, this.newEntry.endTime, this.newEntry.details);
    if (this.timesheetExists) {
      this._timesheetService.addTimesheetEntry(this.activeTimeSheet.id, entry).subscribe(
        res => {
          //Update the entry with the primary key that has come back from server
          entry.id = res as number;
          this.activeTimeSheet.timesheetEntries.push(entry);

          if (this.selectedDay == "Mon") {
            this.monEntries.push(entry);
          }
          else if (this.selectedDay == "Tue") {
            this.tueEntries.push(entry);
          }
          else if (this.selectedDay == "Wed") {
            this.wedEntries.push(entry);
          }
          else if (this.selectedDay == "Thurs") {
            this.thursEntries.push(entry);
          }
          else if (this.selectedDay == "Fri") {
            this.friEntries.push(entry);
          }
          else {
            this.satEntries.push(entry);
          }
        }, error => this.errors = error);
    }
    else {
      if (this.selectedDay == "Mon") {
        this.monEntries.push(entry);
      }
      else if (this.selectedDay == "Tue") {
        this.tueEntries.push(entry);
      }
      else if (this.selectedDay == "Wed") {
        this.wedEntries.push(entry);
      }
      else if (this.selectedDay == "Thurs") {
        this.thursEntries.push(entry);
      }
      else if (this.selectedDay == "Fri") {
        this.friEntries.push(entry);
      }
      else {
        this.satEntries.push(entry);
      }
      //Automatically save timesheet if it has not been saved already
      this.saveTimesheet();
    }

    $("#myNewTimesheetModal").modal('hide');
  }

  paintMonTab() {
    this.selectedDay = "Mon";
  }

  paintTueTab() {
    this.selectedDay = "Tue";
  }

  paintWedTab() {
    this.selectedDay = "Wed";
  }

  paintThursTab() {
    this.selectedDay = "Thurs";
  }

  paintFriTab() {
    this.selectedDay = "Fri";
  }

  paintSatTab() {
    this.selectedDay = "Sat";
  }

  calculateTotalDuration() {
    let totalDuration: number = 0;

    for (let tse of this.retrieveTimesheetsForTab()) {
      totalDuration = totalDuration + this.calculateDuration(tse);
    }
    return totalDuration;
  }

  calculateDuration(entry : TimesheetEntry) : number {
    var start = new Date("2018-01-01 " + entry.startTime);
    var end = new Date("2018-01-01 " + entry.endTime);

    var elapsedTimeInSec = (end.getTime() - start.getTime()) / 1000;
    var elapsedTimeInMins = elapsedTimeInSec / 60;

    return elapsedTimeInMins;
  }

  retrieveTimesheetsForTab(): Array<TimesheetEntry> {
    if (this.selectedDay == "Mon") {
      return this.monEntries;
    }
    else if (this.selectedDay == "Tue") {
      return this.tueEntries;
    }
    else if (this.selectedDay == "Wed") {
      return this.wedEntries;
    }
    else if (this.selectedDay == "Thurs") {
      return this.thursEntries;
    }
    else if (this.selectedDay == "Fri") {
      return this.friEntries;
    }
    else {
      return this.satEntries;
    }
  }

  displayListOfEntries(): boolean {
    return this.retrieveTimesheetsForTab().length > 0;
  }

  populateTimesheet(entries: TimesheetEntry[]) {
    for (let item of entries) {
      this.activeTimeSheet.timesheetEntries.push(item);
    }
  }
  saveTimesheet() {
    //Populate timesheet object with entries
    this.populateTimesheet(this.monEntries);
    this.populateTimesheet(this.tueEntries);
    this.populateTimesheet(this.wedEntries);
    this.populateTimesheet(this.thursEntries);
    this.populateTimesheet(this.friEntries);
    this.populateTimesheet(this.satEntries);
    
    this._timesheetService.saveTimesheet(this.activeTimeSheet).subscribe(
      res => {
        console.log(res);
        this.timesheetExists = true;
        this.activeTimeSheet.id = res as number;
        this.timesheets.push(this.activeTimeSheet);
        $("#myNewTimesheetModal").modal('hide');
      },
      error => this.errors = error);
  }

  submitTimesheet() {
    this.activeTimeSheet.status = "Submitted";
    this._timesheetService.updateTimesheet(this.activeTimeSheet).subscribe(
      res => {
        console.log(res);
      },
      error => this.errors = error);
  }
}

export class DayOfWeek {
  constructor(
    public nameOfDay: string,
    public date: Date
  ) { }
} 
