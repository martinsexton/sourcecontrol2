import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Timesheet } from '../timesheet';
import { TimesheetEntry } from '../timesheetentry';
import { TimesheetCode } from '../timesheetcode';
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
import { SignalRService } from '../shared/services/signalrservice';

declare var $: any;

@Component({
  selector: 'labour',
  templateUrl: './labour.component.html',
  styleUrls: ['./labour.component.css']
})

export class LabourComponent {
  public barChartOptions: any = {
    scaleShowVerticalLines: false,
    responsive: true
  };
  public barChartLabels: string[] = [];
  public barChartType: string = 'bar';
  public barChartLegend: boolean = true;
  public sendingReport: boolean = false;
  public sendingFullReport: boolean = false;

  public barChartData: any[] = [
    { data: [], label: 'Week 1' }, { data: [], label: 'Week 2' }, { data: [], label: 'Week 3' }, { data: [], label: 'Week 4' }, { data: [], label: 'Week 5' }
  ];

  public labourWeeks: LabourWeek[] = [];
  public errors: string;
  public projects: Project[];
  public selectedProject: Project;
  public loadingLabourDetails: Boolean = false;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService,
    private _timesheetService: TimesheetService, private _certificationService: CertificateService, public signalRService: SignalRService) {
    this.loadingLabourDetails = true;
    this._projectService.getActiveProjects().subscribe(result => {
      this.loadingLabourDetails = false;
      this.projects = result;
      this.selectedProject = this.projects[0];

      //Display labour costs for first project 
      this.filterLabourCostForProject(this.selectedProject);
    }, error => {
      this.loadingLabourDetails = false;
      this.errors = "Failed to retrieve labour details"
      $('.toast').toast('show');
    });
  }

  ngOnInit(): void {
    this.signalRService.startConnection();
    this.signalRService.addReportEmailsListener();
  }

  calculateTotalCostToDate() {
    let totalCostToDate: number = 0;

    for(let lw of this.labourWeeks){
      totalCostToDate = totalCostToDate + lw.totalCost;
    }
    return totalCostToDate;
  }

  public downloadFullReport() {
    this.sendingFullReport = true;
    this._timesheetService.downloadFullReport().subscribe(result => {
      this.sendingFullReport = false;
    }, error => {
      this.loadingLabourDetails = false;
      this.sendingFullReport = false;
      this.errors = "Failed to download report"
      $('.toast').toast('show');
    });
  }

  public downloadReport() {
    this.sendingReport = true;
    this._timesheetService.downloadReport(this.selectedProject.code).subscribe(result => {
      this.sendingReport = false;
    }, error => {
      this.loadingLabourDetails = false;
      this.sendingReport = false;
      this.errors = "Failed to download report"
      $('.toast').toast('show');
    });
  }

  retrieveTimeCodes() {
    let codes: Array<TimesheetCode> = new Array();


    if (this.projects) {
      for (let p of this.projects) {
        let code = new TimesheetCode(p.code, p.name, true);
        codes.push(code);
      }
    }
    return codes;
  }

  filterLabourCostForProject(project) {
    this._timesheetService.getLabourWeekDetailsForProject(project.code).subscribe(result => {
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
        this.selectedProject = project;
        this.barChartLabels = cloneLabels;
        this.barChartData = clone;
      }

    }, error => {
      this.loadingLabourDetails = false;
      this.errors = "Failed to retrieve labour details"
      $('.toast').toast('show');
    });
  }
}
