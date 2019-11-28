import { Component, OnInit, OnDestroy, Inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ProjectService } from '../shared/services/project.service';
import { TimesheetService } from '../shared/services/timesheet.service';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Project } from '../project';
import { Timesheet } from '../timesheet';
import { ProjectCostDto } from '../projectcostdto';

declare var $: any;

@Component({
  selector: 'projecthealth',
  templateUrl: './projecthealth.component.html'
})
export class ProjectHealthComponent {
  public barChartOptions: any = {
    scaleShowVerticalLines: false,
    responsive: true,
    scales: {
      xAxes: [{
        scaleLabel: {
          display: true,
          labelString: 'Weeks'
        }
      }],
      yAxes: [{
        scaleLabel: {
          display: true,
          labelString: 'Euro'
        }
      }]
    }//END scales
  };
  public loading = true;
  public dataToPresent = false;
  public timesheets: Timesheet[];
  //public barChartLabels: string[] = ['January', 'February', 'Mars', 'April'];
  public barChartLabels: string[] = [];

  public barChartType: string = 'bar';
  public barChartLegend: boolean = true;

  public projects: Project[];
  public selectedProject: Project;

  public costsGraphData: ProjectCostDto;

  public barChartData: any[] = [];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService, private _timesheetService: TimesheetService) {
    this._projectService.getProjects().subscribe(result => {
      this.projects = result;
      this.selectedProject = this.projects[0];

      //Retrive Graph Details for selected project
      this._projectService.getProjectEffort(this.selectedProject.code).subscribe(result => {
        this.setupGraph(result);

      }, error => console.error(error));
    }, error => {
      });
  }

  refreshGraphForProject(project: Project) {
    this.loading = true;
    this.dataToPresent = false;

    this.selectedProject = project;

    //Retrive Graph Details for selected project
    this._projectService.getProjectEffort(this.selectedProject.code).subscribe(result => {
      this.setupGraph(result);
      this.loading = false;
    }, error => console.error(error));
  }

  setupGraph(result: ProjectCostDto) {
    this.costsGraphData = result;
    this.barChartLabels = [];
    this.barChartData = [];

    if (result == null || result.weeks == null || result.weeks.length == 0) {
      return;
    }
    this.dataToPresent = true;

    for (let wk of this.costsGraphData.weeks) {
      this.barChartLabels.push(wk);
    }
    let d: number[] = [];
    for (let cost of this.costsGraphData.costs) {
      d.push(cost);
    }
    console.log('populating data');
    this.barChartData.push({ data: d, label: this.costsGraphData.projectName });
    this.loading = false;
  }

  // events
  public chartClicked(e: any): void {
    console.log(e);
  }

  public chartHovered(e: any): void {
    console.log(e);
  }

  //public displayEffortGraph() {
  //  if (!this.efforts) {
  //    this._projectService.getProjectEffort().subscribe(result => {
  //      this.efforts = result;

  //      if (this.efforts) {
  //        let data: number[] = [];
  //        let cloneLabels = JSON.parse(JSON.stringify(this.barChartLabels));
  //        let index: number = 0;

  //        for (let effort of this.efforts) {
  //          cloneLabels.push(effort.projectName);
  //          data.push(effort.totalEffort);
  //          index = index + 1;
  //        }
  //        this.barChartLabels = cloneLabels;

  //        let clone = JSON.parse(JSON.stringify(this.barChartData));
  //        clone[0].data = data;
  //        clone[0].label = "Project Effort";
  //        this.barChartData = clone;
  //      }
  //    }, error => console.error(error));
  //  }
  //  else {
  //    let data: number[] = [];
  //    let cloneLabels = JSON.parse(JSON.stringify(this.barChartLabels));
  //    let index: number = 0;

  //    for (let effort of this.efforts) {
  //      cloneLabels.push(effort.projectName);
  //      data.push(effort.totalEffort);
  //      index = index + 1;
  //    }
  //    this.barChartLabels = cloneLabels;

  //    let clone = JSON.parse(JSON.stringify(this.barChartData));
  //    clone[0].data = data;
  //    clone[0].label = "Project Effort";
  //    this.barChartData = clone;
  //  }
  //}

  //public displayCostsGraph() {
  //  let data: number[] = [];
  //  let cloneLabels = JSON.parse(JSON.stringify(this.barChartLabels));
  //  let index: number = 0;

  //  for (let effort of this.efforts) {
  //    cloneLabels.push(effort.projectName);
  //    data.push(effort.totalCost);
  //    index = index + 1;
  //  }
  //  this.barChartLabels = cloneLabels;

  //  let clone = JSON.parse(JSON.stringify(this.barChartData));
  //  clone[0].data = data;
  //  clone[0].label = "Project Cost";
  //  this.barChartData = clone;
  //}
}
