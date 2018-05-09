import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Timesheet } from '../timesheet';
import { TimesheetEntry } from '../timesheetentry';
import { Project } from '../project';

import {
  ProjectService
} from '../project.service';

declare var $: any;

@Component({
  selector: 'dashboard',
  templateUrl: './dashboard.component.html'
})

export class DashboardComponent {
  public timesheets: Timesheet[];
  public selectedTimesheet: Timesheet;
  public selectedTimesheetUser: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService) {
    //Retrieve Timesheets For display
    this._projectService.getTimesheets().subscribe(result => {
      this.timesheets = result;
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

  displaySelectedTimesheetDetails(timesheet) {
    this.selectedTimesheet = timesheet;
  }
}
