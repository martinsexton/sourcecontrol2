import { Component, Input } from '@angular/core';
import { Client } from '../client';
import { Project } from '../project';
import {
  ProjectService
} from '../shared/services/project.service';

declare var $: any;

@Component({
  selector: 'project-list',
  templateUrl: './projectlist.component.html',
  styleUrls: ['./projectlist.component.css']
})

export class ProjectsListComponent {
  @Input() selectedClient: Client;
  @Input() projects: Project[];
  @Input() projectCodes: string[];
  selectedProject: Project;
  newProject: Project = new Project(0, '', '', '', '', true, new Date);

  constructor(private _projectService: ProjectService) {}

  deleteProject(p) {
    this._projectService.deleteProject(p).subscribe(
      res => {
        console.log(res);
        this.removeFromArrayList(p);
      });
  }

  isProjectActive(project: Project) {
    return project.isActive;
  }

  saveProject() {
    this._projectService.addProject(this.selectedClient.id, this.newProject).subscribe(
      res => {
        this.newProject.id = res as number;
        this.newProject.client = this.selectedClient.name;

        let projectToPush = new Project(this.newProject.id, this.newProject.client, this.newProject.name, this.newProject.code, this.newProject.details, this.newProject.isActive, this.newProject.startDate);

        //Update the collection of projects with newly created one
        this.projects.push(projectToPush);
        this.selectedClient.projects.push(projectToPush);

        //clear down the new project model
        this.newProject = new Project(0, '', '', '', '', true, new Date);
        $("#myNewProjectModal").modal('hide');

      });
  }

  removeFromArrayList(p: Project) {
    for (let item of this.selectedClient.projects) {
      if (item.client == p.client && item.client == p.client && item.details == p.details) {
        this.selectedClient.projects.splice(this.selectedClient.projects.indexOf(item), 1);
        break;
      }
    }
    for (let item of this.projects) {
      if (item.client == p.client && item.client == p.client && item.details == p.details) {
        this.projects.splice(this.projects.indexOf(item), 1);
        break;
      }
    }
  }

  updateProject() {
    this._projectService.updateProject(this.selectedProject).subscribe(
      res => {
        $("#myModal").modal('hide');
      });
  }

  displaySelectedProject(project) {
    this.selectedProject = project;
    $("#myModal").modal('show');
  }
}

