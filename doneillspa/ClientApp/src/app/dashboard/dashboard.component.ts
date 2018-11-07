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
  public selectedTimesheet: Timesheet;
  public selectedTsRow: number;
  public selectedUserRow: number;
  public selectedDate: string = null;
  public selectedTimesheetUser: string;

  public loading = true;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService, private _timesheetService: TimesheetService, private _certificationService: CertificateService) {
    this.loading = false;
    //Retrieve Default list of tui Timesheets For display
    //this._timesheetService.getTimesheets().subscribe(result => {
    //  this.timesheets = result;
    //  if (this.timesheets.length > 0) {
    //    this.selectedTimesheet = this.timesheets[0];
    //    this.selectedTsRow = 0;
    //  }
    //}, error => console.error(error));
  }

  setSelectedDate() {
    let newDate = new Date(this.selectedDate);

    var startOfWeek = new Date();
    startOfWeek.setDate(newDate.getDate() - (newDate.getDay() - 1));
    startOfWeek.setMonth(newDate.getMonth());

    //Retrieve Timesheets For display
    this._timesheetService.getTimesheet(startOfWeek.getFullYear(), (startOfWeek.getMonth() + 1), startOfWeek.getDate()).subscribe(result => {
      this.timesheets = result;
      if (this.timesheets.length > 0) {
        this.selectedTimesheet = this.timesheets[0];
        this.selectedTsRow = 0;
      }
    }, error => console.error(error));
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
