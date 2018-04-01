import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'projects',
  templateUrl: './projects.component.html'
})

export class ProjectComponent {
  public projects: Project[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Project[]>(baseUrl + 'api/project').subscribe(result => {
      this.projects = result;
    }, error => console.error(error));
  }
}

interface Project {
  name: string;
  isActive: boolean;
  details: string;
}
