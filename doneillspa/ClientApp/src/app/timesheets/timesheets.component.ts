import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Timesheet } from '../timesheet';

import {
  ProjectService
} from '../project.service';

declare var $: any;

@Component({
  selector: 'timesheets',
  templateUrl: './timesheets.component.html'
})

export class TimesheetComponent {
  public timesheets: Timesheet[];
  displayAddTimesheet = false;
  newTimesheet: Timesheet = new Timesheet(localStorage.getItem('client_id'), new Date());

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService) {
    this._projectService.getTimesheets().subscribe(result => {
      this.timesheets = result;
    }, error => console.error(error));
  }

  toggleDisplayAddTimesheet() {
    this.displayAddTimesheet = !this.displayAddTimesheet;
    if (this.displayAddTimesheet) {
      $("#myNewTimesheetModal").modal('show');
    } else {
      $("#myNewTimesheetModal").modal('hide');
    }
  }

  saveTimesheet() {
    this._projectService.saveTimesheet(this.newTimesheet).subscribe(
      res => {
        console.log(res);
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
