import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Timesheet } from '../timesheet';
import { TimesheetEntry } from '../timesheetentry';
import { TimesheetCode } from '../timesheetcode';
import { Project } from '../project';
import { NonChargeableTime } from '../nonchargeabletime';
import * as moment from 'moment';

import {
  ProjectService
} from '../shared/services/project.service';

import {
  TimesheetService
} from '../shared/services/timesheet.service';
import { ApplicationUser } from '../applicationuser';
import { MsUserService } from '../shared/services/msuser.service';

declare var $: any;

@Component({
  selector: 'timesheets2',
  templateUrl: './timesheets2.component.html',
  styleUrls: ['./timesheets2.component.css']
})

export class Timesheet2Component {
  selectedMoment: moment.Moment = moment();
  public monday: DayOfWeek;
  public tues: DayOfWeek;
  public wed: DayOfWeek;
  public thurs: DayOfWeek;
  public fri: DayOfWeek;
  public sat: DayOfWeek;
  public sun: DayOfWeek;
  public loading = false;
  public monthToDisplay: string = "";
  activeTimeSheet: Timesheet;
  public newEntry: TimesheetEntry;
  public selectedUser: ApplicationUser = null;
  public timesheets: Timesheet[];
  public timesheetExists = false;
  public errors: string;
  public selectedDay: string = "Mon";

  public selectedDayEntries: Array<TimesheetEntry> = new Array();
  public monEntries: Array<TimesheetEntry> = new Array();
  public tueEntries: Array<TimesheetEntry> = new Array();
  public wedEntries: Array<TimesheetEntry> = new Array();
  public thursEntries: Array<TimesheetEntry> = new Array();
  public friEntries: Array<TimesheetEntry> = new Array();
  public satEntries: Array<TimesheetEntry> = new Array();
  public sunEntries: Array<TimesheetEntry> = new Array();
  public daysOfWeek: Array<string> = new Array();

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService, private _timesheetService: TimesheetService, private _msuserService: MsUserService) {
    //Setup the user to user in retrieving and saving timesheets
    this.setDefaultUser();

    //Populate available days of the week for entering timesheets
    this.populateDaysOfWeek();

    //setup dates for each day of the week
    this.refreshCalendarDates();

    //Get the start of this working week
    var startOfWeek = this.getStartOfWeek();

    //Setting up default timesheet and timesheet entries
    this.activeTimeSheet = new Timesheet(0, this.selectedUser.firstName + this.selectedUser.surname, this.selectedUser.id, this.selectedUser.role, startOfWeek, null, 'New');
    this.newEntry = new TimesheetEntry("", "", "", "", "", "", true);

    //Retrieve timesheets for given date
    this.retrieveTimeSheetsForDate(startOfWeek);

    this.loading = false;
  }


  calculateTotalDurationForEntry(item: TimesheetEntry): string {
    let totalDuration: number = this.deriveElapsedTimeInMins(item.startTime, item.endTime);

    var hours = Math.floor(totalDuration / 60);
    var minutes = totalDuration % 60;

    return hours + ' hrs ' + minutes + ' mins';
  }

  deriveElapsedTimeInMins(startTime: string, endTime: string): number {
    var start = new Date("2018-01-01 " + startTime);
    var end = new Date("2018-01-01 " + endTime);

    var elapsedTimeInSec = (end.getTime() - start.getTime()) / 1000;
    var elapsedTimeInMins = elapsedTimeInSec / 60;

    return elapsedTimeInMins;
  }

  populateDaysOfWeek() {
    this.daysOfWeek.push("Mon");
    this.daysOfWeek.push("Tue");
    this.daysOfWeek.push("Wed");
    this.daysOfWeek.push("Thurs");
    this.daysOfWeek.push("Fri");
    this.daysOfWeek.push("Sat");
    this.daysOfWeek.push("Sun");
  }

  mondaySelected() {
    this.setActiveDay(0);
  }

  tuesdaySelected() {
    this.setActiveDay(1);
  }

  wednesdaySelected() {
    this.setActiveDay(2);
  }

  thursdaySelected() {
    this.setActiveDay(3);
  }

  fridaySelected() {
    this.setActiveDay(4);
  }

  saturdaySelected() {
    this.setActiveDay(5);
  }

  setActiveDay(index) {
    this.selectedDay = this.daysOfWeek[index];
    this.newEntry.day = this.selectedDay;
  }

  setDefaultUser() {
    this.selectedUser = new ApplicationUser(localStorage.getItem('client_id'), localStorage.getItem('firstname'), localStorage.getItem('surname'), "", "", localStorage.getItem('role'), true);
  }

  retrieveTimesheetsForDay() {
    if (this.selectedDay == "Mon") {
      this.monEntries.sort(function sortByStartTime(a, b) {
        let day1 = parseInt(a.startTime.split(':')[0]);
        let day2 = parseInt(b.startTime.split(':')[0]);
        return day1 - day2;
      });
      return this.monEntries;
    }
    else if (this.selectedDay == "Tue") {
      this.tueEntries.sort(function sortByStartTime(a, b) {
        let day1 = parseInt(a.startTime.split(':')[0]);
        let day2 = parseInt(b.startTime.split(':')[0]);
        return day1 - day2;
      });
      return this.tueEntries;
    }
    else if (this.selectedDay == "Wed") {
      this.wedEntries.sort(function sortByStartTime(a, b) {
        let day1 = parseInt(a.startTime.split(':')[0]);
        let day2 = parseInt(b.startTime.split(':')[0]);
        return day1 - day2;
      });
      return this.wedEntries;
    }
    else if (this.selectedDay == "Thurs") {
      this.thursEntries.sort(function sortByStartTime(a, b) {
        let day1 = parseInt(a.startTime.split(':')[0]);
        let day2 = parseInt(b.startTime.split(':')[0]);
        return day1 - day2;
      });
      return this.thursEntries;
    }
    else if (this.selectedDay == "Fri") {
      this.friEntries.sort(function sortByStartTime(a, b) {
        let day1 = parseInt(a.startTime.split(':')[0]);
        let day2 = parseInt(b.startTime.split(':')[0]);
        return day1 - day2;
      });
      return this.friEntries;
    }
    else if (this.selectedDay == "Sat") {
      this.satEntries.sort(function sortByStartTime(a, b) {
        let day1 = parseInt(a.startTime.split(':')[0]);
        let day2 = parseInt(b.startTime.split(':')[0]);
        return day1 - day2;
      });
      return this.satEntries;
    }
    else {
      this.sunEntries.sort(function sortByStartTime(a, b) {
        let day1 = parseInt(a.startTime.split(':')[0]);
        let day2 = parseInt(b.startTime.split(':')[0]);
        return day1 - day2;
      });
      return this.sunEntries;
    }
  }

  populateEntriesForDaysOfWeek(array: Timesheet[], ws: Date) {
    //Clear Arrays before loading
    this.monEntries.length = 0;
    this.tueEntries.length = 0;
    this.wedEntries.length = 0;
    this.thursEntries.length = 0;
    this.friEntries.length = 0;
    this.satEntries.length = 0;
    this.sunEntries.length = 0;

    //reset flag that determines if timesheet exists or not for the given date
    this.timesheetExists = false;
    this.activeTimeSheet.id = 0;
    this.activeTimeSheet.status = "New";
    this.activeTimeSheet.weekStarting = ws;
    //Clear the timesheet entries in the existing in memory timesheet
    this.activeTimeSheet.timesheetEntries.length = 0;
    this.activeTimeSheet.timesheetNotes.length = 0;

    for (let ts of array) {
      this.activeTimeSheet.weekStarting = ts.weekStarting;
      this.activeTimeSheet.id = ts.id;
      this.activeTimeSheet.status = ts.status;
      this.activeTimeSheet.timesheetNotes = ts.timesheetNotes;
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
        else if (item.day == "Sat") {
          this.satEntries.push(item);
        }
        else {
          this.sunEntries.push(item);
        }
      }
      break;
    }
  }

  retrieveTimeSheetsForDate(startOfWeek: Date) {
    this._timesheetService.getTimesheetForUser(startOfWeek.getFullYear(), (startOfWeek.getMonth() + 1), startOfWeek.getDate(), this.selectedUser.firstName + this.selectedUser.surname).subscribe(result => {
      this.timesheets = result;
      this.populateEntriesForDaysOfWeek(this.timesheets, startOfWeek);
    }, error => {
      this.errors = error
    });
  }

  getStartOfWeek(): Date {
    var baseDate = this.selectedMoment.toDate();
    let day = baseDate.getDay();
    var startOfWeek = new Date(this.selectedMoment.toDate());
    startOfWeek.setDate(baseDate.getDate() - (day - 1))
    return startOfWeek;
  }

  nextWeek() {
    this.selectedMoment.add(7, 'days');
    this.refreshCalendarDates();
  }

  previousWeek() {
    this.selectedMoment.add(-7, 'days');
    this.refreshCalendarDates();
  }

  refreshCalendarDates() {
    var baseDate = this.selectedMoment.toDate();
    let day = baseDate.getDay();

    var monday = new Date(this.selectedMoment.toDate());
    monday.setDate(baseDate.getDate() - (day - 1))
    this.monday = new DayOfWeek("Mon", monday);

    var tuesday = new Date(this.selectedMoment.toDate());
    tuesday.setDate(baseDate.getDate() - (day - 2));
    this.tues = new DayOfWeek("Tue", tuesday);

    var wednesday = new Date(this.selectedMoment.toDate());
    wednesday.setDate(baseDate.getDate() - (day - 3));
    this.wed = new DayOfWeek("Wed", wednesday);

    var thursday = new Date(this.selectedMoment.toDate());
    thursday.setDate(baseDate.getDate() - (day - 4));
    this.thurs = new DayOfWeek("Thurs", thursday);

    var friday = new Date(this.selectedMoment.toDate());
    friday.setDate(baseDate.getDate() - (day - 5));
    this.fri = new DayOfWeek("Fri", friday);

    var saturday = new Date(this.selectedMoment.toDate());
    saturday.setDate(baseDate.getDate() - (day - 6));
    this.sat = new DayOfWeek("Sat", saturday);

    var sunday = new Date(this.selectedMoment.toDate());
    sunday.setDate(baseDate.getDate() - (day - 7));
    this.sun = new DayOfWeek("Sun", sunday);
  }

  getMondayDate() : string {
    if (this.monday) {
      return this.monday.date.getDate().toString();
    }
    else {
      return "";
    }
  }

  getTuesdayDate(): string {
    if (this.tues) {
      return this.tues.date.getDate().toString();
    }
    else {
      return "";
    }
  }

  getWednesdayDate(): string {
    if (this.wed) {
      return this.wed.date.getDate().toString();
    }
    else {
      return "";
    }
  }

  getThursdayDate(): string {
    if (this.thurs) {
      return this.thurs.date.getDate().toString();
    }
    else {
      return "";
    }
  }

  getFridayDate(): string {
    if (this.fri) {
      return this.fri.date.getDate().toString();
    }
    else {
      return "";
    }
  }

  getSaturdayDate(): string {
    if (this.sat) {
      return this.sat.date.getDate().toString();
    }
    else {
      return "";
    }
  }

  getMonthToDisplay(): string {
    let month: string = "";

    if (!this.loading) {
      switch (this.selectedMoment.toDate().getMonth()) {
        case 0:
          month = "January";
          break;
        case 1:
          month = "February";
          break;
        case 2:
          month = "March";
          break;
        case 3:
          month = "April";
          break;
        case 4:
          month = "May";
          break;
        case 5:
          month = "June";
          break;
        case 6:
          month = "July";
          break;
        case 7:
          month = "August";
          break;
        case 8:
          month = "September";
          break;
        case 9:
          month = "October";
          break;
        case 10:
          month = "November";
          break;
        case 11:
          month = "December";
          break;
        default:
          break;
      }
    }
    
    return month;
  }
}

export class DayOfWeek {
  constructor(
    public nameOfDay: string,
    public date: Date
  ) { }
} 
