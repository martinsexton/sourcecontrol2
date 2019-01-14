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
  public barChartOptions: any = {
    scaleShowVerticalLines: false,
    responsive: true
  };
  public barChartLabels: string[] = [];
  public barChartType: string = 'bar';
  public barChartLegend: boolean = true;

  public barChartData: any[] = [
    { data: [], label: 'Week 1' }, { data: [], label: 'Week 2' }, { data: [], label: 'Week 3' }, { data: [], label: 'Week 4' }, { data: [], label: 'Week 5' }
  ];

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

      //Update Graph
      if (this.labourWeeks) {
        let data: number[] = [];
        let data2: number[] = [];
        let data3: number[] = [];
        let data4: number[] = [];
        let data5: number[] = [];

        let cloneLabels = JSON.parse(JSON.stringify(this.barChartLabels));
        let clone = JSON.parse(JSON.stringify(this.barChartData));

        let growingLabourCost: number[] = [this.labourWeeks.length];
        let totalamount: number = 0;
        for (var index = 0; index < this.labourWeeks.length; index++) {
          totalamount += this.labourWeeks[index].totalCost;

          growingLabourCost[index] = totalamount;
        }

        for (var index = growingLabourCost.length; index >= 0; index--) {
          if (index == 5) {
            data5.push(growingLabourCost[index - 1]);
            clone[index - 1].data = data5;
          }
          else if (index == 4) {
            data4.push(growingLabourCost[index - 1]);
            clone[index - 1].data = data4;
          }
          if (index == 3) {
            data3.push(growingLabourCost[index - 1]);
            clone[index - 1].data = data3;
          }
          else if (index == 2) {
            data2.push(growingLabourCost[index - 1]);
            clone[index - 1].data = data2;
          }
          else if (index == 1) {
            data.push(growingLabourCost[index - 1]);
            clone[index - 1].data = data;
          }
        }

        this.barChartLabels = cloneLabels;
        this.barChartData = clone;
      }

    }, error => this.errors = error)
  }
}
