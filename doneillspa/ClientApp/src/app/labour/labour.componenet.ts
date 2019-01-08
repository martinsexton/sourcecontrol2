import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Timesheet } from '../timesheet';
import { TimesheetEntry } from '../timesheetentry';
import { Project } from '../project';
import { LabourWeek } from '../labourweek';

import {
  ProjectService
} from '../shared/services/project.service';

import {
  TimesheetService
} from '../shared/services/timesheet.service';
import { CertificateService } from '../shared/services/certificate.service';

@Component({
  selector: 'labour',
  templateUrl: './labour.component.html',
})

export class LabourComponent {
  public labourWeeks: LabourWeek[];
  public errors: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService, private _timesheetService: TimesheetService, private _certificationService: CertificateService) {
    //Retrieve Default list of tui Timesheets For display
    this._timesheetService.getLabourWeekDetails().subscribe(result => {
      this.labourWeeks = result;
    }, error => this.errors = error)
  }
}
