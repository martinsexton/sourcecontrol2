import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Timesheet } from '../timesheet';

import {
  ProjectService
} from '../project.service';

@Component({
  selector: 'timesheets',
  templateUrl: './timesheets.component.html'
})

export class TimesheetComponent {
  public timesheets: Timesheet[];
  newTimesheet: Timesheet = new Timesheet('', new Date());

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService) {
    this._projectService.getTimesheets().subscribe(result => {
      this.timesheets = result;
    }, error => console.error(error));
  }

  saveTimesheet() {
    this._projectService.saveTimesheet(this.newTimesheet).subscribe(
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
}
