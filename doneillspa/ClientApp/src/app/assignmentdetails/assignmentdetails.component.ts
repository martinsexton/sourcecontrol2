import { Component, OnInit, OnDestroy, Inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import * as moment from 'moment';
import { TimesheetService } from '../shared/services/timesheet.service';
import { Timesheet } from '../timesheet';
import { forEach } from '@angular/router/src/utils/collection';
import { TimesheetNote } from '../timesheetnote';
import { ProjectAssignment } from '../projectassignment';

declare var $: any;

@Component({
  selector: 'assignment',
  templateUrl: './assignmentdetails.component.html',
  styleUrls: ['./assignmentdetails.component.css']
})
export class AssignmentDetailsComponent {
  public selectedDate: Date;
  public assignments: ProjectAssignment[];
  public assignedProjects: ProjectAssignment[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _timesheetService: TimesheetService) {}

  displayAssignmentsForSelectedWeek() {
    let sd = new Date(this.selectedDate);

    this._timesheetService.getTimesheet(sd.getFullYear(), (sd.getMonth() + 1), sd.getDate()).subscribe(result => {
      this.assignments = result;
      this.assignedProjects = [];

      for (let assignmentDetails of this.assignments) {
        this.assignedProjects.push(assignmentDetails);
      }
    }, error => {});
  }
}
