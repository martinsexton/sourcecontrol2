import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Project } from '../project';

import {
  ProjectService
} from '../shared/services/project.service';

declare var $: any;

@Component({
  selector: 'projects',
  templateUrl: './projects.component.html'
})

export class ProjectComponent {
  public projects: Project[];
  newProject: Project = new Project('', '', true, new Date);
  projectSaved: boolean = false;
  displayAddProject = false;
  selectedProject: Project = new Project('', '', true, new Date);

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService) {
    this._projectService.getProjects().subscribe(result => {
      this.projects = result;
    }, error => console.error(error));
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

  saveProject() {
    this._projectService.saveProject(this.newProject).subscribe(
      res => {
        console.log(res);
        this.projectSaved = true;
        //Update the collection of projects with newly created one
        this.projects.push(new Project(this.newProject.name, this.newProject.details, this.newProject.isactive, this.newProject.startDate));
        //clear down the new project model
        this.newProject = new Project('', '', true, new Date);
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
