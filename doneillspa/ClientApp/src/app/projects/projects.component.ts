import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Project } from '../project';

import {
  ProjectService
} from '../shared/services/project.service';
import { Router } from '@angular/router';
import { LabourRate } from '../labourrate';
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
  public clientsFilteredByState: Client[];

  public selectedClient: Client;
  public activeTab: string = "Active";

  public labourRates: LabourRate[];
  public filteredRates: LabourRate[] = [];
  public userMessage: string;
  public selectedRole: string;
  public loading = true;
  public existingCodes: string[] = [];
  public clientsForCurrentPage: Client[];
  public projectsForCurrentPage: Project[];
  public clientsCurrentPage: number = 1;
  public projectsCurrentPage: number = 1;
  public pageLimit: number = 10;
  public projectPageLimit: number = 5;

  newProject: Project = new Project(0, '', '', '', '', true, new Date);
  newClient: Client = new Client(0, "", true);
  projectSaved: boolean = false;
  selectedProject: Project = new Project(0, '', '', '', '', true, new Date);
  selectedRate: LabourRate = new LabourRate(0, null, null, '', 0, 0);

  newRate: LabourRate = new LabourRate(0, null, null, '', 0, 0);

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService, private _router: Router) {
    this.retrieveClients();
    this.retrieveRates();
  }

  displayProjectsForSelectedClient(client) {
    this.selectedClient = client;
    this.projectsCurrentPage = 1;
    this.projectsToDisplay = this.selectedClient.projects;
    this.setupProjectsForCurrentPage();
  }

  determinePageCount() {
    var pageCount = 1;
    if (this.clientsFilteredByState) {
      var numberOfClients = this.clientsFilteredByState.length;

      if (numberOfClients > 0) {
        var totalPages_pre = Math.floor((numberOfClients / this.pageLimit));
        pageCount = (numberOfClients % this.pageLimit) == 0 ? totalPages_pre : totalPages_pre + 1
      }
      return pageCount;
    }
  }

  activeTabClicked() {
    this.activeTab = "Active";
    this.clientsFilteredByState = [];
    this.projectsForCurrentPage = [];
    this.projectsToDisplay = [];
    
    //Setup active clients first
    for (let client of this.clients) {
      if (client.isActive) {
        this.clientsFilteredByState.push(client);
      }
    }

    //Reset page count. 
    this.clientsCurrentPage = 1;
    this.setupClientsForCurrentPage();

    if (this.clientsFilteredByState.length > 0) {
      this.displayProjectsForSelectedClient(this.clientsFilteredByState[0]);
    }
  }

  inactiveTabClicked() {
    this.activeTab = "Inactive";
    this.clientsFilteredByState = [];
    this.projectsForCurrentPage = [];
    this.projectsToDisplay = [];

    //Setup active clients first
    for (let client of this.clients) {
      if (!client.isActive) {
        this.clientsFilteredByState.push(client);
      }
    }
    //Reset page count. 
    this.clientsCurrentPage = 1;
    this.setupClientsForCurrentPage();

    if (this.clientsFilteredByState.length > 0) {
      this.displayProjectsForSelectedClient(this.clientsFilteredByState[0]);
    }
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

  setupClientsForCurrentPage() {
    var startingIndex = 0;
    var index = 0;

    //Need to reset selectedClient
    this.selectedClient = null;

    //reset client current page array
    this.clientsForCurrentPage = [];

    if (this.clientsCurrentPage > 1) {
      startingIndex = (this.clientsCurrentPage - 1) * this.pageLimit;
    }

    for (let client of this.clientsFilteredByState) {
      if (index >= startingIndex && index < (startingIndex + this.pageLimit)) {
        this.clientsForCurrentPage.push(client);

        //Setup selected client as the firt client on current page.
        if (this.selectedClient == null) {
          this.displayProjectsForSelectedClient(client);
        }
      }
      index = index + 1;
    }
  }

  setupProjectsForCurrentPage() {
    var startingIndex = 0;
    var index = 0;

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
    this.setupClientsForCurrentPage();
  }

  previousProjectPage() {
    this.projectsCurrentPage = this.projectsCurrentPage - 1;
    this.setupProjectsForCurrentPage();
  }

  nextPage() {
    this.clientsCurrentPage = this.clientsCurrentPage + 1;
    this.setupClientsForCurrentPage();
  }

  nextProjectPage() {
    this.projectsCurrentPage = this.projectsCurrentPage + 1;
    this.setupProjectsForCurrentPage();
  }

  retrieveRates() {
    //Retrieve Default list of labour rates
    this._projectService.getLabourRates().subscribe(result => {
      this.labourRates = result;
      //Default to supervisor
      this.displayRatesForSelectedRole("Supervisor");
    }, error => {
      this.userMessage = "Failed to retrieve project rates"
      $('.toast').toast('show');
    })
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
    this._projectService.getClients().subscribe(result => {
      this.loading = false;
      this.clients = result;
      if (this.clients) {
        this.clientsFilteredByState = [];
        //Setup active clients first
        for (let client of this.clients) {
          if (client.isActive) {
            this.clientsFilteredByState.push(client);
          }
        }

        //By default select first client
        this.setupClientsForCurrentPage();

        if (this.clientsFilteredByState) {
          //Will need to set the two below based on first client on current page.
          this.selectedClient = this.clientsFilteredByState[0];
          this.projectsToDisplay = this.selectedClient.projects;

          for (let client of this.clients) {
            for (let p of client.projects) {
              this.existingCodes.push(p.code.toUpperCase());
            }
          }
        }

      }
    }, error => {
      this.userMessage = "Failed to retrieve Client Details"
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
    roles.push("Loc1");
    roles.push("Loc2");
    roles.push("Loc3");

    return roles;
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
        this.showUserMessage("Rate Updated Successfully!")
      }, error => {
        $("#myDisplayRateModal").modal('hide');
        this.userMessage = "Failed to update Rate"
        $('.toast').toast('show');
        console.error(error);
      });
  }

  deleteRate(r) {
    this._projectService.deleteRate(r).subscribe(
      res => {
        console.log(res);
        this.removeFromRatesArrays(r);
        this.showUserMessage("Rate Deleted!")
        
      }, error => {
        this.userMessage = "Failed to delete Rate"
        $('.toast').toast('show');
        console.error(error);
      });
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

        this.showUserMessage("Rate Saved Successfully!")
      }, error => {
        $("#myNewRateModal").modal('hide');
        this.userMessage = "Failed to save Rate"
        $('.toast').toast('show');
        console.error(error);
      });
  }

  showUserMessage(msg :string) {
    this.userMessage = msg;
    $('.toast').toast('show');
  }

  updateClient() {
    this._projectService.updateClient(this.selectedClient).subscribe(
      res => {

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
        this.clientsForCurrentPage.push(clientJustAdded);
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
