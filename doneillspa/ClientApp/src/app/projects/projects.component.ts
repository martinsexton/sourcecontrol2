import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import {
  ProjectService
} from '../project.service';

@Component({
  selector: 'projects',
  templateUrl: './projects.component.html'
})

export class ProjectComponent {
  public projects: Project[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService) {
    this._projectService.getProjects().subscribe(result => {
      this.projects = result;
    }, error => console.error(error));
  }
}

interface Project {
  name: string;
  id: number;
  isActive: boolean;
  details: string;
}
