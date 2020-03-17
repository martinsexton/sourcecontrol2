import { Component, OnDestroy, OnInit, Output, EventEmitter } from '@angular/core';
import { Project } from '../../project';
import { ProjectService } from '../../shared/services/project.service';
import { ProjectCostDto } from '../../projectcostdto';
import { TimesheetEntry } from '../../timesheetentry';

declare var $: any;

@Component({
  selector: 'project-graph',
  templateUrl: './projectgraph.component.html',
  styleUrls: ['./projectgraph.component.css']
})
export class ProjectGraphComponent  {
  @Output() eventPublisher = new EventEmitter<Project>();
  public timesheetsEntries: TimesheetEntry[];

  constructor(private _projectService: ProjectService) {}

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

  public loading = false;
  public dataToPresent = false;
  public costsGraphData: ProjectCostDto;
  public selectedProject: Project;

  public barChartLabels: string[] = [];
  public barChartType: string = 'bar';
  public barChartLegend: boolean = true;
  public barChartData: any[] = [];

  refreshGraphForProject(project: Project) {
    this.loading = true;
    this.dataToPresent = false;
    this.selectedProject = project;
    this.eventPublisher.emit(project);

    //Retrive Graph Details for selected project
    this._projectService.getProjectEffort(project.code).subscribe(result => {
      this.setupGraph(result);
      this.loading = false;
    }, error => console.error(error));
  }

  setupGraph(result: ProjectCostDto) {
    this.costsGraphData = result;
    this.barChartLabels = [];
    this.barChartData = [];

    if (result == null || result.weeks == null || result.weeks.length == 0) {
      console.log('No data found');
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
    this.barChartData.push({ data: d, label: this.costsGraphData.projectName });
    this.loading = false;
  }

  // events
  public chartClicked(e: any): void {
    if (e.active.length > 0) {
      let week = e.active[0]._model.label;
      //Retrive Graph Details for selected project
      this._projectService.getTimesheetEntriesForProjectAndWeek(this.selectedProject.code, week).subscribe(result => {
        this.timesheetsEntries = result;
        const sorter = {
          "mon": 1,
          "tue": 2,
          "wed": 3,
          "thurs": 4,
          "fri": 5,
          "sat": 6,
          "sun": 7
        }

        //Order timesheet entries
        this.timesheetsEntries.sort(function sortByDay(a, b) {
          let day1 = a.day.toLowerCase();
          let day2 = b.day.toLowerCase();
          return sorter[day1] - sorter[day2];
        });

        $("#myModal").modal('show');
      }, error => console.error(error));
    }
  }
}
