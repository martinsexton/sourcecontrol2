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

  //Default to Monday
  public selectedDay = "Mon";

  public monEntries: Array<TimesheetEntry> = new Array();
  public tueEntries: Array<TimesheetEntry> = new Array();
  public wedEntries: Array<TimesheetEntry> = new Array();
  public thursEntries: Array<TimesheetEntry> = new Array();
  public friEntries: Array<TimesheetEntry> = new Array();
  public satEntries: Array<TimesheetEntry> = new Array();

  newTimesheet: Timesheet;
  public newEntry: TimesheetEntry;

  currentDate: Date;

  monday: DayOfWeek;
  tues: DayOfWeek;
  wed: DayOfWeek;
  thurs: DayOfWeek;
  fri: DayOfWeek;
  sat: DayOfWeek;

  displayAddTimesheet = false;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService, private _timesheetService : TimesheetService) {
    this.currentDate = new Date();

    var startOfWeek = new Date();
    startOfWeek.setDate(this.currentDate.getDate() - (this.currentDate.getDay() - 1))


    //Setting up default timesheet and timesheet entries
    this.newTimesheet = new Timesheet(0, localStorage.getItem('username'),localStorage.getItem('client_id'), startOfWeek);
    this.newEntry = new TimesheetEntry("", "", "", "", "");

    this.refreshCalendarTabs();

    this._projectService.getProjects().subscribe(result => {
      this.projects = result;
    }, error => console.error(error));

    this._timesheetService.getTimesheet(startOfWeek.getFullYear(), (startOfWeek.getMonth()+1), startOfWeek.getDate()).subscribe(result => {
      this.loading = false;
      this.timesheets = result;
      //Populate calendar if we find timesheets for this week
      this.populateWeeklyCalendar(this.timesheets);
    }, error => console.error(error));
  }

  populateWeeklyCalendar(array: Timesheet[]) {
    for (let ts of array) {
      if (ts.owner == localStorage.getItem("client_id")) {
        this.newTimesheet.id = ts.id;
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


  previousWeek() {
    //move back one week
    this.currentDate.setDate(this.currentDate.getDate() - 7);
    this.refreshCalendarTabs();
  }

  nextWeek() {
    //move forward one week
    this.currentDate.setDate(this.currentDate.getDate() + 7);
    this.refreshCalendarTabs();
  }

  refreshCalendarTabs() {
    let day = this.currentDate.getDay();

    var monday = new Date();
    monday.setDate(this.currentDate.getDate() - (day - 1))
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

  removeTimesheetEntry(ts) {
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
      },
      (err: HttpErrorResponse) => {
        console.log(err.error);
        console.log(err.name);
        console.log(err.message);
        console.log(err.status);
      }
    );
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
    let entry: TimesheetEntry = new TimesheetEntry(this.newEntry.project, this.selectedDay, this.newEntry.startTime, this.newEntry.endTime, this.newEntry.equipment);
    if (this.timesheetExists) {
      this.newTimesheet.timesheetEntries.push(entry);

      this._timesheetService.updateTimesheet(this.newTimesheet).subscribe(
        res => {
          console.log(res);
        },
        (err: HttpErrorResponse) => {
          console.log(err.error);
          console.log(err.name);
          console.log(err.message);
          console.log(err.status);
        }
      );
    }
    if (this.selectedDay == "Mon") {
      this.monEntries.push(new TimesheetEntry(this.newEntry.project, "Mon", this.newEntry.startTime, this.newEntry.endTime, this.newEntry.equipment));
    }
    else if (this.selectedDay == "Tue") {
      this.tueEntries.push(new TimesheetEntry(this.newEntry.project, "Tue", this.newEntry.startTime, this.newEntry.endTime, this.newEntry.equipment));
    }
    else if (this.selectedDay == "Wed") {
      this.wedEntries.push(new TimesheetEntry(this.newEntry.project, "Wed", this.newEntry.startTime, this.newEntry.endTime, this.newEntry.equipment));
    }
    else if (this.selectedDay == "Thurs") {
      this.thursEntries.push(new TimesheetEntry(this.newEntry.project, "Thurs", this.newEntry.startTime, this.newEntry.endTime, this.newEntry.equipment));
    }
    else if (this.selectedDay == "Fri") {
      this.friEntries.push(new TimesheetEntry(this.newEntry.project, "Fri", this.newEntry.startTime, this.newEntry.endTime, this.newEntry.equipment));
    }
    else {
      this.satEntries.push(new TimesheetEntry(this.newEntry.project, "Sat", this.newEntry.startTime, this.newEntry.endTime, this.newEntry.equipment));
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
      this.newTimesheet.timesheetEntries.push(item);
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
    
    this._timesheetService.saveTimesheet(this.newTimesheet).subscribe(
      res => {
        console.log(res);
        this.timesheetExists = true;
        this.newTimesheet.id = res as number;
        this.timesheets.push(this.newTimesheet);
        $("#myNewTimesheetModal").modal('hide');
      },
      (err: HttpErrorResponse) => {
        console.log(err.error);
        console.log(err.name);
        console.log(err.message);
        console.log(err.status);
      }
    );
  }
}

export class DayOfWeek {
  constructor(
    public nameOfDay: string,
    public date: Date
  ) { }
} 
