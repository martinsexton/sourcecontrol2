import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Timesheet } from '../timesheet';
import { TimesheetEntry } from '../timesheetentry';
import { Project } from '../project';
import * as moment from 'moment';

import {
  ProjectService
} from '../shared/services/project.service';

import {
  TimesheetService
} from '../shared/services/timesheet.service';

declare var $: any;

@Component({
  selector: 'timesheets',
  templateUrl: './timesheets.component.html',
  styleUrls: ['./timesheets.component.css']
})

export class TimesheetComponent {
  public timesheets: Timesheet[];
  public projects: Project[];
  public timesheetExists = false;
  public loading = true;
  public selectedDate: string = null;
  public errors: string;

  //Default to Monday
  public selectedDay: string = "Mon";

  public monEntries: Array<TimesheetEntry> = new Array();
  public tueEntries: Array<TimesheetEntry> = new Array();
  public wedEntries: Array<TimesheetEntry> = new Array();
  public thursEntries: Array<TimesheetEntry> = new Array();
  public friEntries: Array<TimesheetEntry> = new Array();
  public satEntries: Array<TimesheetEntry> = new Array();

  public daysOfWeek: Array<string> = new Array();

  activeTimeSheet: Timesheet;

  selectedMoment: moment.Moment = moment();

  public newEntry: TimesheetEntry;
  public timesheetEntryToEdit: TimesheetEntry

  public monday: DayOfWeek;
  public tues: DayOfWeek;
  public wed: DayOfWeek;
  public thurs: DayOfWeek;
  public fri: DayOfWeek;
  public sat: DayOfWeek;

  displayAddTimesheet = false;
  displayEditTimesheet = false;

  //declare var $: any;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService, private _timesheetService : TimesheetService) {
    //Populate available days of the week for entering timesheets
    this.populateDaysOfWeek();

    //Setup list of available projects
    this.setupListOfProjects();

    //setup dates for each day of the week
    this.refreshCalendarDates();

    //Get the start of this working week
    var startOfWeek = this.getStartOfWeek();

    //Setting up default timesheet and timesheet entries
    this.activeTimeSheet = new Timesheet(0, localStorage.getItem('username'), localStorage.getItem('client_id'), localStorage.getItem('role'), startOfWeek,'New');
    this.newEntry = new TimesheetEntry("", "", "", "", "");

    //Retrieve timesheets for given date
    this.retrieveTimeSheetsForDate(startOfWeek);
  }

  ngOnInit() {
    $('[data-toggle="tooltip"]').tooltip(); 
  }

  retrieveTimeSheetsForDate(startOfWeek:Date) {
    this._timesheetService.getTimesheet(startOfWeek.getFullYear(), (startOfWeek.getMonth() + 1), startOfWeek.getDate()).subscribe(result => {
      this.loading = false;
      this.timesheets = result;
      this.populateEntriesForDaysOfWeek(this.timesheets, startOfWeek);
    }, error => {
      this.errors = error
      this.loading = false;
    });
  }

  getStartOfWeek(): Date {
    var baseDate = this.selectedMoment.toDate();
    let day = baseDate.getDay();
    var startOfWeek = new Date(this.selectedMoment.toDate());
    startOfWeek.setDate(baseDate.getDate() - (day - 1))
    return startOfWeek;
  }

  setupListOfProjects() {
    this._projectService.getProjects().subscribe(result => {
      this.projects = result;
      if (this.projects) {
        this.newEntry.project = this.projects[0].name;
      }
    }, error => console.error(error));  
  }

  populateDaysOfWeek() {
    this.daysOfWeek.push("Mon");
    this.daysOfWeek.push("Tue");
    this.daysOfWeek.push("Wed");
    this.daysOfWeek.push("Thurs");
    this.daysOfWeek.push("Fri");
    this.daysOfWeek.push("Sat");
  }

  nextWeek() {
    this.selectedMoment.add(7, 'days');
    this.LoadWeek();
  }

  previousWeek() {
    this.selectedMoment.add(-7, 'days');
    this.LoadWeek();
  }

  LoadWeek() {
    this.refreshCalendarDates();

    var startOfWeek = this.getStartOfWeek();

    this._timesheetService.getTimesheet(startOfWeek.getFullYear(), (startOfWeek.getMonth() + 1), startOfWeek.getDate()).subscribe(result => {
      this.loading = false;
      this.timesheets = result;
      this.populateEntriesForDaysOfWeek(this.timesheets, startOfWeek);
    }, error => this.errors = error);
  }

  setActiveDay(index) {
    this.selectedDay = this.daysOfWeek[index];;
  }

  retrieveTimesheetsForIndex(index) {
    let day = this.daysOfWeek[index];
    return this.retrieveTimesheetsForDay(day);
  }
  retrieveTimesheetsForDay(day:string) {
    if (day == "Mon") {
      return this.monEntries;
    }
    else if (day == "Tue") {
      return this.tueEntries;
    }
    else if (day == "Wed") {
      return this.wedEntries;
    }
    else if (day == "Thurs") {
      return this.thursEntries;
    }
    else if (day == "Fri") {
      return this.friEntries;
    }
    else {
      return this.satEntries;
    }
  }

  populateEntriesForDaysOfWeek(array: Timesheet[], ws : Date) {
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
    this.activeTimeSheet.status = "New";
    this.activeTimeSheet.weekStarting = ws;
    //Clear the timesheet entries in the existing in memory timesheet
    this.activeTimeSheet.timesheetEntries.length = 0;

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

  retrieveDateForDay(day: string) : Date{
    if (day == "Mon") {
      return this.monday.date;
    }
    else if (day == "Tue") {
      return this.tues.date;
    }
    else if (day == "Wed") {
      return this.wed.date;
    }
    else if (day == "Thurs") {
      return this.thurs.date;
    }
    else if (day == "Fri") {
      return this.fri.date;
    }
    else {
      return this.sat.date;
    }
  }

  refreshCalendarDates() {
    var baseDate = this.selectedMoment.toDate();
    let day = baseDate.getDay();

    var monday = new Date(this.selectedMoment.toDate());
    monday.setDate(baseDate.getDate() - (day - 1))
    this.monday = new DayOfWeek("Mon", monday);

    var tuesday = new Date(this.selectedMoment.toDate());
    tuesday.setDate(monday.getDate() + 1);
    this.tues = new DayOfWeek("Tue", tuesday);

    var wednesday = new Date(this.selectedMoment.toDate());
    wednesday.setDate(tuesday.getDate() + 1);
    this.wed = new DayOfWeek("Wed", wednesday);

    var thursday = new Date(this.selectedMoment.toDate());
    thursday.setDate(wednesday.getDate() + 1);
    this.thurs = new DayOfWeek("Thurs", thursday);

    var friday = new Date(this.selectedMoment.toDate());
    friday.setDate(thursday.getDate() + 1);
    this.fri = new DayOfWeek("Fri", friday);

    var saturday = new Date(this.selectedMoment.toDate());
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
    if (this.activeTimeSheet.status == 'New' || this.activeTimeSheet.status == 'Rejected') {
      this.timesheetEntryToEdit = entry;
      this.displayEditTimesheet = true;
      if (this.displayEditTimesheet) {
        $("#myEditTimesheetModal").modal('show');
      } else {
        $("#myEditTimesheetModal").modal('hide');
      }
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
          $("#myEditTimesheetModal").modal('hide');

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

          this.pushTimesheetToCalendarDays(entry);

        }, error => this.errors = error);
    }
    else {
      this.pushTimesheetToCalendarDays(entry);
      //Automatically save timesheet if it has not been saved already
      this.saveTimesheet();
    }

    $("#myNewTimesheetModal").modal('hide');
  }

  pushTimesheetToCalendarDays(entry:TimesheetEntry) {
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
  }

  calculateTotalDuration() {
    let totalDuration: number = 0;

    for (let tse of this.retrieveTimesheetsForDay(this.selectedDay)) {
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
