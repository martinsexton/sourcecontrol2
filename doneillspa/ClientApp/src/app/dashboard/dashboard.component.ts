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
import { CertificateService } from '../shared/services/certificate.service';

declare var $: any;

@Component({
  selector: 'dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})

export class DashboardComponent {
  public timesheets: Timesheet[];
  public filteredTimesheets: Timesheet[];
  public users: string[];
  public selectedTimesheet: Timesheet;
  public selectedTsRow: number;
  public selectedUserRow: number;
  public selectedDate: string = null;
  public selectedTimesheetUser: string;
  public filterusername: string;
  public errors: string;

  public loading = true;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService, private _timesheetService: TimesheetService, private _certificationService: CertificateService) {
    this.loading = false;
    //Retrieve Default list of tui Timesheets For display 
    this._timesheetService.getTimesheets().subscribe(result => {
      this.timesheets = result;
      if (this.timesheets.length > 0) {
        this.refreshListOfUsers();
        this.selectedTimesheet = this.timesheets[0];
        this.selectedTsRow = 0;
      }
    }, error => this.errors = error);
  }

  refreshListOfUsers() {

    //Clear array before we start
    if (this.users) {
      this.users = null;
    }

    for (let ts of this.timesheets) {
      if (!this.users) {
        this.users = [];
        this.users.push(ts.username);
      }
      else {
        for (let n of this.users) {
          if (n.toUpperCase() != ts.username.toUpperCase()) {
            this.users.push(ts.username);
          }
        }
      }
    }
  }

  retrieveSubmittedTimesheets() {
    this.filteredTimesheets = [];
    for (let item of this.timesheets) {
      if (item.status.toUpperCase() == 'SUBMITTED') {
        this.filteredTimesheets.push(item);
      }
    }
    if (this.filteredTimesheets) {
      return this.filteredTimesheets;
    }
    else {
      return this.timesheets;
    }
  }

  rejectTimesheet(ts: Timesheet) {
    ts.status = 'Rejected';
    this._timesheetService.updateTimesheet(ts).subscribe(
      res => {
        console.log(res);
      },
      error => this.errors = error);
  }

  approveTimesheet(ts: Timesheet) {
    ts.status = 'Approved';
    this._timesheetService.updateTimesheet(ts).subscribe(
      res => {
        console.log(res);
      },
      error => this.errors = error);
  }

  retrieveTimesheetsForDisplay() {
    if (this.filteredTimesheets) {
      return this.filteredTimesheets;
    }
    else {
      return this.timesheets;
    }
  }

  setSelectedDate() {
    let newDate = new Date(this.selectedDate);

    var startOfWeek = new Date();
    startOfWeek.setDate(newDate.getDate() - (newDate.getDay() - 1));
    startOfWeek.setMonth(newDate.getMonth());
    startOfWeek.setFullYear(newDate.getFullYear());

    //Retrieve Timesheets For display
    this._timesheetService.getTimesheet(startOfWeek.getFullYear(), (startOfWeek.getMonth() + 1), startOfWeek.getDate()).subscribe(result => {
      this.timesheets = result;
      //Clear filters
      this.filteredTimesheets = null;
      if (this.timesheets.length > 0) {
        this.refreshListOfUsers();
        this.selectedTimesheet = this.timesheets[0];
        this.selectedTsRow = 0;
      } else {
        //Clear the selected timesheet if no result found.
        this.selectedTimesheet = null;
        this.selectedTsRow = 0;
      }
    }, error => this.errors = error);
  }

  calculateTotalDuration() : string{
    let totalDuration: number = 0;

    for (let tse of this.selectedTimesheet.timesheetEntries) {
      var start = new Date("2018-01-01 " + tse.startTime);
      var end = new Date("2018-01-01 " + tse.endTime);

      var elapsedTimeInSec = (end.getTime() - start.getTime()) / 1000;
      var elapsedTimeInMins = elapsedTimeInSec / 60;
      totalDuration += elapsedTimeInMins;
    }

    var hours = Math.floor(totalDuration / 60);
    var minutes = totalDuration % 60;

    return hours + ':' + minutes;
  }

  calculateDuration(entry: TimesheetEntry): string {
    var start = new Date("2018-01-01 " + entry.startTime);
    var end = new Date("2018-01-01 " + entry.endTime);

    var elapsedTimeInSec = (end.getTime() - start.getTime()) / 1000;
    var elapsedTimeInMins = elapsedTimeInSec / 60;

    var hours = Math.floor(elapsedTimeInMins / 60);
    var minutes = elapsedTimeInMins % 60;

    return hours+ ':' + minutes;
  }

  displaySelectedTimesheetDetails(timesheet, index) {
    this.selectedTimesheet = timesheet;
    this.selectedTsRow = index
  }

  getTimesheetEntriesForSubmittedTimesheets(index) {
    let ts = this.retrieveSubmittedTimesheets()[index];
    return ts.timesheetEntries;
  }
}
