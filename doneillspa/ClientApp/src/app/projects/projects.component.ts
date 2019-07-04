import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Project } from '../project';

import {
  ProjectService
} from '../shared/services/project.service';
import { Router } from '@angular/router';
import { LabourRate } from '../labourrate';

declare var $: any;

@Component({
  selector: 'projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.css']
})

export class ProjectComponent {
  public projects: Project[];
  public labourRates: LabourRate[];
  public filteredRates: LabourRate[] = [];
  public errors: string;
  public selectedRole: string;

  newProject: Project = new Project(0, '', '', '', true, new Date);
  projectSaved: boolean = false;
  selectedProject: Project = new Project(0, '', '', '', true, new Date);
  selectedRate: LabourRate = new LabourRate(0, null, null, '', 0, 0);

  newRate: LabourRate = new LabourRate(0, null, null, '', 0, 0);

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService, private _router : Router) {
    this.retrieveProjects()
    this.retrieveRates();
  }

  retrieveRates() {
    //Retrieve Default list of labour rates
    this._projectService.getLabourRates().subscribe(result => {
      this.labourRates = result;
      //Default to supervisor
      this.displayRatesForSelectedRole("Supervisor");
    }, error => {
      this.errors = "Failed to retrieve project rates"
      $('.toast').toast('show');
    })
  }

  retrieveProjects() {
    this._projectService.getProjects().subscribe(result => {
      this.projects = result;
    }, error => {
      this.errors = "Failed to retrieve available Projects"
      $('.toast').toast('show');
      console.error(error);
    });
  }

  displayRatesForSelectedRole(role) {
    this.selectedRole = role;
    this.filteredRates = [];
    for (let r of this.labourRates) {
      if (r.role == this.selectedRole) {
        this.filteredRates.push(r);
      }
    }
  }

  retrieveRolesToDisplay() {
    let roles: string[] = [];
    roles.push("Supervisor");
    roles.push("ChargeHand");
    roles.push("ElectR1");
    roles.push("ElectR2");
    roles.push("ElectR3");
    roles.push("Temp");
    roles.push("First Year Apprentice");
    roles.push("Second Year Apprentice");
    roles.push("Third Year Apprentice");
    roles.push("Fourth Year Apprentice");

    return roles;
  }

  displaySelectedProject(project) {
    this.selectedProject = project;
    $("#myModal").modal('show');
  }

  displaySelectedRate(rate) {
    this.selectedRate = rate;
    $("#myDisplayRateModal").modal('show');
  }

  updateRate() {
    this._projectService.updateRate(this.selectedRate).subscribe(
      res => {
        console.log(res);
        $("#myDisplayRateModal").modal('hide');
      }, error => {
        $("#myDisplayRateModal").modal('hide');
        this.errors = "Failed to update Rate"
        $('.toast').toast('show');
        console.error(error);
      });
  }

  updateProject(){
    this._projectService.updateProject(this.selectedProject).subscribe(
      res => {
        console.log(res);
        $("#myModal").modal('hide');
      }, error => {
        $("#myModal").modal('hide');
        this.errors = "Failed to update project"
        $('.toast').toast('show');
        console.error(error);
      });
  }

  deleteRate(r) {
    this._projectService.deleteRate(r).subscribe(
      res => {
        console.log(res);
        this.removeFromRatesArrays(r);
        
      }, error => {
        this.errors = "Failed to delete Rate"
        $('.toast').toast('show');
        console.error(error);
      });
  }

  deleteProject(p) {
    this._projectService.deleteProject(p).subscribe(
      res => {
        console.log(res);
        this.removeFromArrayList(p);
      }, error => {
        this.errors = "Failed to delete Project"
        $('.toast').toast('show');
        console.error(error);
      });
  }

  removeFromArrayList(p: Project) {
    for (let item of this.projects) {
      if (item.name == p.name && item.client == p.client && item.details == p.details) {
        this.projects.splice(this.projects.indexOf(item), 1);
        break;
      }
    }
  }

  removeFromRatesArrays(r: LabourRate) {
    for (let item of this.filteredRates) {
      if (item.id == r.id) {
        this.filteredRates.splice(this.filteredRates.indexOf(item), 1);
        break;
      }
    }
    for (let item of this.labourRates) {
      if (item.id == r.id) {
        this.labourRates.splice(this.filteredRates.indexOf(item), 1);
        break;
      }
    }
  }

  saveRate() {
    this._projectService.saveRate(this.newRate).subscribe(
      res => {
        console.log(res);
        this.newRate.id = res as number;
        //Update the collection of projects with newly created one
        this.labourRates.push(new LabourRate(this.newRate.id,this.newRate.effectiveFrom, this.newRate.effectiveTo, this.newRate.role, this.newRate.ratePerHour, this.newRate.overTimeRatePerHour));

        this.displayRatesForSelectedRole(this.newRate.role);

        //clear down the new project model
        this.newRate = new LabourRate(0, null, null, '', 0, 0);
        $("#myNewRateModal").modal('hide');
      }, error => {
        $("#myNewRateModal").modal('hide');
        this.errors = "Failed to save Rate"
        $('.toast').toast('show');
        console.error(error);
      });
  }

  saveProject() {
    this._projectService.saveProject(this.newProject).subscribe(
      res => {
        this.newProject.id = res as number;
        console.log(res);
        this.projectSaved = true;
        //Update the collection of projects with newly created one
        this.projects.push(new Project(this.newProject.id, this.newProject.client, this.newProject.name, this.newProject.details, this.newProject.isactive, this.newProject.startDate));
        //clear down the new project model
        this.newProject = new Project(0, '', '', '', true, new Date);
        $("#myNewProjectModal").modal('hide');
      }, error => {
        $("#myNewProjectModal").modal('hide');
        this.errors = "Failed to save project"
        $('.toast').toast('show');
        console.error(error);
      });
  }
}
