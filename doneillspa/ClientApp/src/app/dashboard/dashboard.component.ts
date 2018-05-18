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

import {
  TimesheetService
} from '../shared/services/timesheet.service';
import { CertificateService } from '../shared/services/certificate.service';

declare var $: any;

@Component({
  selector: 'dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})

export class DashboardComponent {
  public timesheets: Timesheet[];
  public selectedTimesheet: Timesheet;
  public selectedTsRow: number;
  public selectedUserRow: number;

  public selectedUser: ApplicationUser;
  public selectedUserCertifications: Certificate[];

  public selectedTimesheetUser: string;
  public users: ApplicationUser[];
  public newCertificate: Certificate = new Certificate(0, new Date(), new Date(), "");

  displayAddCert = false;
  public loading = true;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService, private _msuserService: MsUserService, private _timesheetService: TimesheetService, private _certificationService : CertificateService) {
    //Retrieve Timesheets For display
    this._timesheetService.getTimesheets().subscribe(result => {
      this.timesheets = result;
      if (this.timesheets.length > 0) {
        this.selectedTimesheet = this.timesheets[0];
        this.selectedTsRow = 0;
      }
    }, error => console.error(error));

    this._msuserService.getUsers().subscribe(result => {
      this.users = result;
      this.loading = false;
      if (this.users.length > 0) {
        this.selectedUser = this.users[0];
        this.selectedUserRow = 0;
        if (this.selectedUser.certifications) {
          this.selectedUserCertifications = this.selectedUser.certifications;
        }
        else {
          this.selectedUserCertifications = new Array<Certificate>();
        }
      }
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

  deleteCertification(crt) {
    this._certificationService.deleteCertification(crt).subscribe(
      res => {
        console.log(res);
        this.removeFromArrayList(crt);
      },
      (err: HttpErrorResponse) => {
        console.log(err.error);
        console.log(err.name);
        console.log(err.message);
        console.log(err.status);
      }
    );
  }

  removeFromArrayList(crt: Certificate) {
    for (let item of this.selectedUserCertifications) {
      if (crt.id == item.id) {
        this.selectedUserCertifications.splice(this.selectedUserCertifications.indexOf(item), 1);
        break;
      }
    }
  }

  hasCertExpired(cert: Certificate): boolean {
    var todaysDate: Date = new Date();
    var certExpiryDate: Date = new Date(cert.expiry);

    var td = new Date();
    td.setMonth(todaysDate.getMonth());
    td.setDate(todaysDate.getDate());
    td.setFullYear(todaysDate.getFullYear());

    var cd = new Date();
    cd.setMonth(certExpiryDate.getMonth());
    cd.setDate(certExpiryDate.getDate());
    cd.setFullYear(certExpiryDate.getFullYear());
     
    return (cd < td);
  }

  toggleDisplayAddCertificate() {
    this.displayAddCert = !this.displayAddCert;
    if (this.displayAddCert) {
      $("#myNewCertificateModal").modal('show');
    } else {
      $("#myNewCertificateModal").modal('hide');
    }
  }

  displaySelectedTimesheetDetails(timesheet, index) {
    this.selectedTimesheet = timesheet;
    this.selectedTsRow = index
  }

  displaySelectedUserDetails(user, index) {
    this.selectedUser = user;
    this.selectedUserRow = index;
    if (this.selectedUser.certifications) {
      this.selectedUserCertifications = this.selectedUser.certifications;
    }
    else {
      this.selectedUserCertifications = new Array<Certificate>();
    }
    
  }

  addCertificateEntry() {
    this._msuserService.addCertificate(this.selectedUser.id, this.newCertificate).subscribe(result =>
    {
      $("#myNewCertificateModal").modal('hide');
      this.selectedUserCertifications.push(this.newCertificate);
      this.newCertificate = new Certificate(0, new Date(), new Date(), "")
    }, error => console.error(error));
  }
}
