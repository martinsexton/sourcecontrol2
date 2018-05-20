import { Component, OnInit, OnDestroy, Inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ProjectService } from '../shared/services/project.service';
import { TimesheetService } from '../shared/services/timesheet.service';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Project } from '../project';
import { Timesheet } from '../timesheet';
import { ProjectEffortDto } from '../projecteffortdto';

declare var $: any;

@Component({
  selector: 'projecthealth',
  templateUrl: './projecthealth.component.html'
})
export class ProjectHealthComponent {
  public barChartOptions: any = {
    scaleShowVerticalLines: false,
    responsive: true
  };
  public loading = true;
  public timesheets: Timesheet[];
  public barChartLabels: string[] = [];
  public barChartType: string = 'bar';
  public barChartLegend: boolean = true;

  public projects: Project[];

  public efforts: ProjectEffortDto[];

  public barChartData: any[] = [
    { data: [], label: 'Hours Worked' }
  ];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService, private _timesheetService: TimesheetService) {
    this._projectService.getProjectEffort().subscribe(result => {
      this.loading = false;
      this.efforts = result;

        if (this.efforts) {
          let data: number[] = [];
          let cloneLabels = JSON.parse(JSON.stringify(this.barChartLabels));
          let index: number = 0;

          for (let effort of this.efforts) {
            cloneLabels.push(effort.projectName);
            data.push(effort.totalEffort);
            index = index + 1;
          }
          this.barChartLabels = cloneLabels;

          let clone = JSON.parse(JSON.stringify(this.barChartData));
          clone[0].data = data;
          this.barChartData = clone;
        }
      }, error => console.error(error));
  }

  // events
  public chartClicked(e: any): void {
    console.log(e);
  }

  public chartHovered(e: any): void {
    console.log(e);
  }

  public displayEffortGraph() {
    if (!this.efforts) {
      this._projectService.getProjectEffort().subscribe(result => {
        this.efforts = result;

        if (this.efforts) {
          let data: number[] = [];
          let cloneLabels = JSON.parse(JSON.stringify(this.barChartLabels));
          let index: number = 0;

          for (let effort of this.efforts) {
            cloneLabels.push(effort.projectName);
            data.push(effort.totalEffort);
            index = index + 1;
          }
          this.barChartLabels = cloneLabels;

          let clone = JSON.parse(JSON.stringify(this.barChartData));
          clone[0].data = data;
          clone[0].label = "Project Effort";
          this.barChartData = clone;
        }
      }, error => console.error(error));
    }
    else {
      let data: number[] = [];
      let cloneLabels = JSON.parse(JSON.stringify(this.barChartLabels));
      let index: number = 0;

      for (let effort of this.efforts) {
        cloneLabels.push(effort.projectName);
        data.push(effort.totalEffort);
        index = index + 1;
      }
      this.barChartLabels = cloneLabels;

      let clone = JSON.parse(JSON.stringify(this.barChartData));
      clone[0].data = data;
      clone[0].label = "Project Effort";
      this.barChartData = clone;
    }
  }

  public displayCostsGraph() {
    let data: number[] = [];
    let cloneLabels = JSON.parse(JSON.stringify(this.barChartLabels));
    let index: number = 0;

    for (let effort of this.efforts) {
      cloneLabels.push(effort.projectName);
      data.push(effort.totalCost);
      index = index + 1;
    }
    this.barChartLabels = cloneLabels;

    let clone = JSON.parse(JSON.stringify(this.barChartData));
    clone[0].data = data;
    clone[0].label = "Project Cost";
    this.barChartData = clone;
  }
}
