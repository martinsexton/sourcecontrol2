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


  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService) {
    //Retrieve Timesheets For display
    this._projectService.getTimesheets().subscribe(result => {
      this.timesheets = result;
    }, error => console.error(error));
  }
}
