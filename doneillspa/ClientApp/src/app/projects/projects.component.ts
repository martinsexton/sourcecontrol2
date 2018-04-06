import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Project } from '../project';

import {
  ProjectService
} from '../project.service';

@Component({
  selector: 'projects',
  templateUrl: './projects.component.html'
})

export class ProjectComponent {
  public projects: Project[];
  newProject: Project = new Project('', '', true);
  projectSaved: boolean = false;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService) {
    this._projectService.getProjects().subscribe(result => {
      this.projects = result;
    }, error => console.error(error));
  }

  saveProject() {
    this._projectService.saveProject(this.newProject).subscribe(
      res => {
        console.log(res);
        this.projectSaved = true;
        //Update the collection of projects with newly created one
        this.projects.push(new Project(this.newProject.name, this.newProject.details, this.newProject.isactive));
        //clear down the new project model
        this.newProject = new Project('', '', true);
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
