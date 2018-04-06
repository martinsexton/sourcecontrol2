import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
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
  newProject: Project = new Project(1, '', '', true);

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService) {
    this._projectService.getProjects().subscribe(result => {
      this.projects = result;
    }, error => console.error(error));
  }
}
