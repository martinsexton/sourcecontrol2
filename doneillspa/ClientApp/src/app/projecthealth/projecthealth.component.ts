import { Component, OnInit, OnDestroy, Inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ProjectService } from '../shared/services/project.service';
import { TimesheetService } from '../shared/services/timesheet.service';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Project } from '../project';
import { Timesheet } from '../timesheet';
import { ProjectCostDto } from '../projectcostdto';
import { TimesheetEntry } from '../timesheetentry';

declare var $: any;

@Component({
  selector: 'projecthealth',
  templateUrl: './projecthealth.component.html',
  styleUrls: ['./projecthealth.component.css']
})
export class ProjectHealthComponent {
  public timesheets: Timesheet[];

  public projects: Project[];
  public selectedProject: Project;

  public projectsCurrentPage: number = 1;
  public projectsForCurrentPage: Project[];
  public pageLimit: number = 10;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService, private _timesheetService: TimesheetService) {
    this._projectService.getActiveProjects().subscribe(result => {
      this.projects = result;
      //By default select first client
      this.setupProjectsForCurrentPage();
    }, error => {
      });
  }

  setupProjectsForCurrentPage() {
    var startingIndex = 0;
    var index = 0;

    //Need to reset selectedClient
    this.selectedProject = null;

    //reset client current page array
    this.projectsForCurrentPage = [];

    if (this.projectsCurrentPage > 1) {
      startingIndex = (this.projectsCurrentPage - 1) * this.pageLimit;
    }

    for (let proj of this.projects) {
      if (index >= startingIndex && index < (startingIndex + this.pageLimit)) {
        this.projectsForCurrentPage.push(proj);

        if (this.selectedProject == null) {
          this.selectedProject = proj;
        }
      }
      index = index + 1;
    }
  }

  previousPage() {
    this.projectsCurrentPage = this.projectsCurrentPage - 1;
    this.setupProjectsForCurrentPage();
  }

  nextPage() {
    this.projectsCurrentPage = this.projectsCurrentPage + 1;
    this.setupProjectsForCurrentPage();
  }

  determinePageCount() {
    var pageCount = 1;
    if (this.projects) {
      var numberOfProjects = this.projects.length;

      if (numberOfProjects > 0) {
        var totalPages_pre = Math.floor((numberOfProjects / this.pageLimit));
        pageCount = (numberOfProjects % this.pageLimit) == 0 ? totalPages_pre : totalPages_pre + 1
      }
      return pageCount;
    }
  }

  setSelectedProject(project: Project) {
    this.selectedProject = project;
  }
}
