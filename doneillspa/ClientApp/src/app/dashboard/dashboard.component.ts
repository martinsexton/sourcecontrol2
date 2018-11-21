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

  retrieveTimeSheetsForCustomer(name : string) {
    this.filteredTimesheets = [];
    for (let item of this.timesheets) {
      if (item.username.toUpperCase() == name.toUpperCase()) {
        this.filteredTimesheets.push(item);
      }
    }
    if (this.filteredTimesheets) {
      this.selectedTimesheet = this.filteredTimesheets[0];
      this.selectedTsRow = 0;
    }
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

  calculateTotalDuration() {
    let totalDuration: number = 0;

    for (let tse of this.selectedTimesheet.timesheetEntries) {
      totalDuration = totalDuration + this.calculateDuration(tse);
    }
    return totalDuration;
  }

  calculateDuration(entry: TimesheetEntry): number {
    var start = new Date("2018-01-01 " + entry.startTime);
    var end = new Date("2018-01-01 " + entry.endTime);

    var elapsedTimeInSec = (end.getTime() - start.getTime()) / 1000;
    var elapsedTimeInMins = elapsedTimeInSec / 60;

    return elapsedTimeInMins;
  }

  displaySelectedTimesheetDetails(timesheet, index) {
    this.selectedTimesheet = timesheet;
    this.selectedTsRow = index
  }
}
