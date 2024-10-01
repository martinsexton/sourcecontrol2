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
  public projectsToDisplay: Project[] = [];
  public clients: Client[];

  public selectedClient: Client;
  public activeTab: string = "Active";
  public showInActiveProjects: boolean = false;

  public userMessage: string;
  public selectedRole: string;
  public loading = true;
  public existingCodes: string[] = [];
  public clientsCurrentPage: number = 1;
  public projectsCurrentPage: number = 1;
  public pageLimit: number = 10;
  public projectPageLimit: number = 5;
  public searchFilter: string = "";


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
    if (this.selectedClient) {
      this.retrieveProjectsForClient(this.selectedClient, this.projectsCurrentPage);
    }
  }

  disableClientNextButton() {
    return this.clients.length < this.pageLimit;
  }

  onKey(event: any) { // without type info
    this.clientsCurrentPage = 1;

    if (event.target.value != '') {
      this.retrieveClientsForFilter();
    }
    else {
      this.retrieveClients();
    }
  }


  activeTabClicked() {
    this.showInActiveProjects = false;
    this.activeTab = "Active";
    //Reset page count. 
    this.clientsCurrentPage = 1;
    this.projectsToDisplay = [];
    this.retrieveClients();
  }

  inactiveTabClicked() {
    this.showInActiveProjects = true;
    this.activeTab = "Inactive";
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

  previousPage() {
    this.clientsCurrentPage = this.clientsCurrentPage - 1;
    this.retrieveClients();
  }

  previousProjectPage() {
    this.projectsCurrentPage = this.projectsCurrentPage - 1;
    this.retrieveProjectsForClient(this.selectedClient, this.projectsCurrentPage);
  }

  nextPage() {
    this.clientsCurrentPage = this.clientsCurrentPage + 1;
    this.retrieveClients();
  }

  nextProjectPage() {
    this.projectsCurrentPage = this.projectsCurrentPage + 1;
    this.retrieveProjectsForClient(this.selectedClient, this.projectsCurrentPage);
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

  retrieveProjectsForClient(c : Client, currentPage: number) {
    this._projectService.getProjectsForClient(c.id, this.showInActiveProjects, currentPage, this.projectPageLimit).subscribe(result => {
      this.loading = false;
      this.projectsToDisplay = result;
    }, error => {
      this.userMessage = "Failed to retrieve projects"
      $('.toast').toast('show');
      console.error(error);
    });
  }

  triggerProjectRetrieve() {
    this.retrieveProjectsForClient(this.selectedClient, this.projectsCurrentPage);
  }

  retrieveClientsForFilter() {
    this._projectService.getClientsForFilter(this.searchFilter, this.activeTab == "Active", this.clientsCurrentPage, this.pageLimit).subscribe(result => {
      this.loading = false;
      this.clients = result;
      if (this.clients.length > 0) {

        //Will need to set the two below based on first client on current page.
        this.selectedClient = this.clients[0];
        this.projectsCurrentPage = 1;

        this.retrieveProjectsForClient(this.selectedClient, this.projectsCurrentPage)

        for (let client of this.clients) {
          for (let p of client.projects) {
            this.existingCodes.push(p.code.toUpperCase());
          }
        }
      }
      else {
        //clear the projects if no clients found
        this.projectsToDisplay = [];
      }
    }, error => {
      this.userMessage = "Failed to retrieve Client Details"
      $('.toast').toast('show');
      console.error(error);
    });
  }

  retrieveClientsWithoutFilter() {
    this._projectService.getClients(this.activeTab == "Active", this.clientsCurrentPage, this.pageLimit).subscribe(result => {
      this.loading = false;
      this.clients = result;
      if (this.clients) {

        //Will need to set the two below based on first client on current page.
        this.selectedClient = this.clients[0];
        this.projectsCurrentPage = 1;
        this.retrieveProjectsForClient(this.selectedClient, this.projectsCurrentPage)

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

  retrieveClients() {
    if (this.searchFilter !== "") {
      this.retrieveClientsForFilter();
    }
    else {
      this.retrieveClientsWithoutFilter();
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
