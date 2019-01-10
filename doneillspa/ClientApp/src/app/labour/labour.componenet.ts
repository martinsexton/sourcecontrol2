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
import { LabourRate } from '../labourrate';

@Component({
  selector: 'labour',
  templateUrl: './labour.component.html',
})

export class LabourComponent {
  public labourWeeks: LabourWeek[];
  public errors: string;
  public projects: Project[];
  public selectedProject: Project;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService, private _timesheetService: TimesheetService, private _certificationService: CertificateService) {
    this._projectService.getProjects().subscribe(result => {
      this.projects = result;
      this.selectedProject = this.projects[0];

      //Display labour costs for first project
      this.filterLabourCostForProject(this.selectedProject);
    });
  }

  filterLabourCostForProject(project) {
    this.selectedProject = project;
    this._timesheetService.getLabourWeekDetailsForProject(project.name).subscribe(result => {
      this.labourWeeks = result;
    }, error => this.errors = error)
  }
}
