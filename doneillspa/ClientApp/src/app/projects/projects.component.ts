import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Project } from '../project';

import {
  ProjectService
} from '../shared/services/project.service';
import { Router } from '@angular/router';

declare var $: any;

@Component({
  selector: 'projects',
  templateUrl: './projects.component.html'
})

export class ProjectComponent {
  public projects: Project[];
  newProject: Project = new Project(0, '', '', '', true, new Date);
  projectSaved: boolean = false;
  displayAddProject = false;
  selectedProject: Project = new Project(0, '', '', '', true, new Date);

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService, private _router : Router) {
    this._projectService.getProjects().subscribe(result => {
      this.projects = result;
    }, error => {
      console.error(error);
      this._router.navigate(['/login'], { queryParams: { brandNew: false, firstname: '', surname: '' } });
    });
  }

  displaySelectedProject(project) {
    console.log("testMethod Clicked: " + project.name);
    this.selectedProject = project;
    $("#myModal").modal('show');
  }

  toggleDisplayAddProjectForm() {
    this.displayAddProject = !this.displayAddProject;
    if (this.displayAddProject) {
      $("#myNewProjectModal").modal('show');
    } else {
      $("#myNewProjectModal").modal('hide');
    }
  }

  getProjectButtonStyle() {
    if (this.displayAddProject) {
      return "glyphicon glyphicon-minus";
    }
    else {
      return "glyphicon glyphicon-plus";
    }
  }

  updateProject(){
    this._projectService.updateProject(this.selectedProject).subscribe(
      res => {
        console.log(res);
        $("#myModal").modal('hide');
      },
      (err: HttpErrorResponse) => {
        console.log(err.error);
        console.log(err.name);
        console.log(err.message);
        console.log(err.status);
      }
    );
  } 

  deleteProject(p) {
    this._projectService.deleteProject(p).subscribe(
      res => {
        console.log(res);
        this.removeFromArrayList(p);
      },
      (err: HttpErrorResponse) => {
        console.log(err.error);
        console.log(err.name);
        console.log(err.message);
        console.log(err.status);
      }
    );
  }

  removeFromArrayList(p: Project) {
    for (let item of this.projects) {
      if (item.name == p.name && item.client == p.client && item.details == p.details) {
        this.projects.splice(this.projects.indexOf(item), 1);
        break;
      }
    }
  }

  saveProject() {
    this._projectService.saveProject(this.newProject).subscribe(
      res => {
        console.log(res);
        this.projectSaved = true;
        //Update the collection of projects with newly created one
        this.projects.push(new Project(0, this.newProject.client, this.newProject.name, this.newProject.details, this.newProject.isactive, this.newProject.startDate));
        //clear down the new project model
        this.newProject = new Project(0, '', '', '', true, new Date);
        this.displayAddProject = false;
        $("#myNewProjectModal").modal('hide');
      },
      (err: HttpErrorResponse) => {
        console.log(err.error);
        console.log(err.name);
        console.log(err.message);
        console.log(err.status);
      }
    );
  }
}
