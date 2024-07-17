import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Project } from '../project';

import {
  ProjectService
} from '../shared/services/project.service';
import { Router } from '@angular/router';
import { Client } from '../client';

declare var $: any;

@Component({
  selector: 'projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.css']
})

export class ProjectComponent {
  public projectsToDisplay: Project[];
  public clients: Client[];

  public selectedClient: Client;
  public activeTab: string = "Active";

  public userMessage: string;
  public selectedRole: string;
  public loading = true;
  public existingCodes: string[] = [];
  public projectsForCurrentPage: Project[];
  public clientsCurrentPage: number = 1;
  public projectsCurrentPage: number = 1;
  public pageLimit: number = 10;
  public projectPageLimit: number = 5;


  newProject: Project = new Project(0, '', '', '', '', true, new Date);
  newClient: Client = new Client(0, "", true);
  projectSaved: boolean = false;
  selectedProject: Project = new Project(0, '', '', '', '', true, new Date);

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService, private _router: Router) {
    this.retrieveClients();
  }

  displayProjectsForSelectedClient(client) {
    this.selectedClient = client;
    this.projectsCurrentPage = 1;
    if (this.selectedClient.projects) {
      this.setupProjectsForCurrentPage();
    }
  }

  disableClientNextButton() {
    return this.clients.length < this.pageLimit;
  }


  activeTabClicked() {
    this.activeTab = "Active";
    //Reset page count. 
    this.clientsCurrentPage = 1;

    this.projectsForCurrentPage = [];
    this.projectsToDisplay = [];
    this.retrieveClients();
  }

  inactiveTabClicked() {
    this.activeTab = "Inactive";
    this.projectsForCurrentPage = [];
    this.projectsToDisplay = [];
    //Reset page count. 
    this.clientsCurrentPage = 1;

    this.retrieveClients();
  }

  determineProjectsPageCount() {
    var pageCount = 1;

    if (this.selectedClient) {
      var numberOfProjects = this.selectedClient.projects.length;

      if (numberOfProjects > 0) {
        var totalPages_pre = Math.floor((numberOfProjects / this.projectPageLimit));
        pageCount = (numberOfProjects % this.projectPageLimit) == 0 ? totalPages_pre : totalPages_pre + 1
      }
      return pageCount;
    }
  }

  setupProjectsForCurrentPage() {
    var startingIndex = 0;
    var index = 0;

    this.projectsToDisplay = this.selectedClient.projects;

    //reset client current page array
    this.projectsForCurrentPage = [];

    if (this.projectsCurrentPage > 1) {
      startingIndex = (this.projectsCurrentPage - 1) * this.projectPageLimit;
    }

    for (let p of this.projectsToDisplay) {
      if (index >= startingIndex && index < (startingIndex + this.projectPageLimit)) {
        this.projectsForCurrentPage.push(p);
      }
      index = index + 1;
    }
  }

  previousPage() {
    this.clientsCurrentPage = this.clientsCurrentPage - 1;
    this.retrieveClients();
  }

  previousProjectPage() {
    this.projectsCurrentPage = this.projectsCurrentPage - 1;
    this.setupProjectsForCurrentPage();
  }

  nextPage() {
    this.clientsCurrentPage = this.clientsCurrentPage + 1;
    this.retrieveClients();
  }

  nextProjectPage() {
    this.projectsCurrentPage = this.projectsCurrentPage + 1;
    this.setupProjectsForCurrentPage();
  }

  isProjectActive(project: Project) {
    return project.isActive;
  }

  displaySelectedClient(c: Client) {
    this.selectedClient = c;
    $("#myEditClientModal").modal('show');
  }

  getClientStatus(c: Client) {
    if (c.isActive) {
      return "Active";
    }
    else {
      return "In Active"
    }
  }

  retrieveClients() {
    this._projectService.getClients(this.activeTab == "Active",this.clientsCurrentPage, this.pageLimit).subscribe(result => {
      this.loading = false;
      this.clients = result;
      if (this.clients) {

        //Will need to set the two below based on first client on current page.
        this.selectedClient = this.clients[0];
        this.displayProjectsForSelectedClient(this.clients[0]);

        for (let client of this.clients) {
          for (let p of client.projects) {
            this.existingCodes.push(p.code.toUpperCase());
          }
        }
      }
    }, error => {
      this.userMessage = "Failed to retrieve Client Details"
      $('.toast').toast('show');
      console.error(error);
    });
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
    roles.push("Loc1");
    roles.push("Loc2");
    roles.push("Loc3");

    return roles;
  }

  showUserMessage(msg :string) {
    this.userMessage = msg;
    $('.toast').toast('show');
  }

  updateClient() {
    this._projectService.updateClient(this.selectedClient).subscribe(
      res => {
        this.retrieveClients();
        //clear down the new project model
        this.newClient = new Client(0, '', true);
        $("#myEditClientModal").modal('hide');
      }, error => {
        $("#myEditClientModal").modal('hide');
        console.error(error);
      });
  }

  saveClient() {
    this._projectService.saveClient(this.newClient).subscribe(
      res => {
        this.newClient.id = res as number;
        console.log(res);
        //Update the collection of projects with newly created one
        var clientJustAdded = new Client(this.newClient.id, this.newClient.name, true);
        /*this.clientsForCurrentPage.push(clientJustAdded);*/
        this.clients.push(clientJustAdded);
        this.displayProjectsForSelectedClient(clientJustAdded);

        //clear down the new project model
        this.newClient = new Client(0, '', true);
        $("#myNewClientModal").modal('hide');

        this.showUserMessage("Client Saved Successfully!")
      }, error => {
        $("#myNewClientModal").modal('hide');
        this.userMessage = "Failed to save client"
        $('.toast').toast('show');
        console.error(error);
      });
  }
}
