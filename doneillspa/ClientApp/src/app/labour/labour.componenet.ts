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
  public timesheets: Timesheet[];
  public labourWeeks: LabourWeek[];
  public errors: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService, private _timesheetService: TimesheetService, private _certificationService: CertificateService) {
    //Retrieve Default list of tui Timesheets For display
    this._timesheetService.getTimesheets().subscribe(result => {
      this.timesheets = result;
      if (this.timesheets) {
        this.labourWeeks = [];
        for (let ts of this.timesheets) {
          let lw: LabourWeek = new LabourWeek(ts.weekStarting, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
          for (let tse of ts.timesheetEntries) {
            if (ts.role == "Administrator") {
              lw.administratorHours = lw.administratorHours + 1;
            }
            else if (ts.role == "Supervisor") {
              lw.supervisorHours = lw.supervisorHours + 1;
            }
            else if (ts.role == "ChargeHand") {
              lw.chargehandHours = lw.chargehandHours + 1;
            }
            else if (ts.role == "ElectR1") {
              lw.elecR1Hours = lw.elecR1Hours + 1;
            }
            else if (ts.role == "ElectR2") {
              lw.elecR2Hours = lw.elecR2Hours + 1;
            }
            else if (ts.role == "ElectR3") {
              lw.elecR3Hours = lw.elecR3Hours + 1;
            }
            else if (ts.role == "Temp") {
              lw.tempHours = lw.tempHours + 1;
            }
            else if (ts.role == "First Year Apprentice") {
              lw.firstYearApprenticeHours = lw.firstYearApprenticeHours + 1;
            }
            else if (ts.role == "Second Year Apprentice") {
              lw.secondYearApprenticeHours = lw.secondYearApprenticeHours + 1;
            }
            else if (ts.role == "Third Year Apprentice") {
              lw.thirdYearApprenticeHours = lw.thirdYearApprenticeHours + 1;
            }
            else if (ts.role == "Fourth Year Apprentice") {
              lw.fourthYearApprenticeHours = lw.fourthYearApprenticeHours + 1;
            }
          }
          this.labourWeeks.push(lw);
        }
      }
    }, error => this.errors = error);
  }
}
