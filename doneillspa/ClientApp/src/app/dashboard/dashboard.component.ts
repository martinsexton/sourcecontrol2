import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Timesheet } from '../timesheet';
import { TimesheetEntry } from '../timesheetentry';
import { Certificate } from '../certificate';
import { ApplicationUser } from '../applicationuser';
import { Project } from '../project';
import { MsUserService } from '../shared/services/msuser.service';

import {
  ProjectService
} from '../shared/services/project.service';

declare var $: any;

@Component({
  selector: 'dashboard',
  templateUrl: './dashboard.component.html'
})

export class DashboardComponent {
  public timesheets: Timesheet[];
  public selectedTimesheet: Timesheet;

  public selectedUser: ApplicationUser;
  public selectedUserCertifications: Certificate[];

  public selectedTimesheetUser: string;
  public users: ApplicationUser[];
  public newCertificate: Certificate = new Certificate("",0, new Date(), new Date(), "", null);

  displayAddCert = false;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService, private _msuserService : MsUserService) {
    //Retrieve Timesheets For display
    this._projectService.getTimesheets().subscribe(result => {
      this.timesheets = result;
    }, error => console.error(error));

    this._projectService.getTimesheets().subscribe(result => {
      this.timesheets = result;
    }, error => console.error(error));

    this._msuserService.getUsers().subscribe(result => {
      this.users = result;
    }, error => console.error(error))
  }

  calculateTotalDuration() {
    let totalDuration: number = 0;

    for (let tse of this.selectedTimesheet.timesheetEntries) {
      totalDuration = totalDuration + this.calculateDuration(tse);
    }
    return totalDuration;
  }

  calculateDuration(entry: TimesheetEntry): number {
    var start = new Date("2018-01-01 " + entry.startTime);
    var end = new Date("2018-01-01 " + entry.endTime);

    var elapsedTimeInSec = (end.getTime() - start.getTime()) / 1000;
    var elapsedTimeInMins = elapsedTimeInSec / 60;

    return elapsedTimeInMins;
  }

  toggleDisplayAddCertificate() {
    this.displayAddCert = !this.displayAddCert;
    if (this.displayAddCert) {
      $("#myNewCertificateModal").modal('show');
    } else {
      $("#myNewCertificateModal").modal('hide');
    }
  }

  displaySelectedTimesheetDetails(timesheet) {
    this.selectedTimesheet = timesheet;
  }

  displaySelectedUserDetails(user) {
    this.selectedUser = user;
    this._projectService.getCertifications(user.id).subscribe(result => {
      this.selectedUserCertifications = result;
    }, error => console.error(error));
  }

  addCertificateEntry() {
    this._msuserService.addCertificate(this.selectedUser.id, this.newCertificate).subscribe(result =>
    {
      $("#myNewCertificateModal").modal('hide');
      this.selectedUserCertifications.push(this.newCertificate);
    }, error => console.error(error));
  }
}
