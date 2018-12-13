import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Certificate } from '../certificate';
import { ApplicationUser } from '../applicationuser';
import { Project } from '../project';
import { MsUserService } from '../shared/services/msuser.service';
import { Timesheet } from '../timesheet';
import { TimesheetEntry } from '../timesheetentry';

import {
  ProjectService
} from '../shared/services/project.service';

import {
  TimesheetService
} from '../shared/services/timesheet.service';

import { CertificateService } from '../shared/services/certificate.service';
import { EmailNotification } from '../emailnotification';
import { NotificationService } from '../shared/services/notification.service';

declare var $: any;

@Component({
  selector: 'userdashboard',
  templateUrl: './userdashboard.component.html',
})

export class UserDashboardComponent {
  public selectedUser: ApplicationUser;
  public selectedUserRow: number;
  public selectedUserCertifications: Certificate[] = [];
  public selectedUserNotifications: EmailNotification[] = [];
  public errors: string;

  public filterName: string;

  public timesheets: Timesheet[];

  public users: ApplicationUser[];
  public filteredUsers: ApplicationUser[];
  public newCertificate: Certificate = new Certificate(0, new Date(), new Date(), "");
  public newEmailNotification: EmailNotification = new EmailNotification(0, '', '', '');

  displayAddCert = false;
  displayAddNotification = false;
  public loading = true;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService,
    private _timesheetService: TimesheetService, private _msuserService: MsUserService, private _certificationService: CertificateService,
    private _notificationService: NotificationService) {
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
        if (this.selectedUser.emailNotifications) {
          this.selectedUserNotifications = this.selectedUser.emailNotifications;
        }
        else {
          this.selectedUserNotifications = new Array<EmailNotification>();
        }
      }
    }, error => this.errors = error)
  }

  retrieveUser() {
    this.filteredUsers = [];
    for (let item of this.users) {
      if ((item.firstName + item.surname) == this.filterName) {
        this.filteredUsers.push(item);
      }
    }
    if (this.filteredUsers) {
      this.selectedUser = this.filteredUsers[0];

      if (this.selectedUser.certifications) {
        this.selectedUserCertifications = this.selectedUser.certifications;
      }
      else {
        this.selectedUserCertifications = new Array<Certificate>();
      }
    }
  }

  retrieveUsersToDisplay() {
    if (this.filteredUsers) {
      return this.filteredUsers;
    }
    else {
      return this.users;
    }
  }

  deleteCertification(crt) {
    this._certificationService.deleteCertification(crt).subscribe(
      res => {
        console.log(res);
        this.removeFromArrayList(this.selectedUserCertifications, crt);
        //Need to reflect the change in the user also.
        for (let u of this.users) {
          if ((u.firstName + u.surname) == (this.selectedUser.firstName + this.selectedUser.surname)) {
            if (u.certifications) {
              this.removeFromArrayList(u.certifications, crt);
            }
          }
        }
      }, error => this.errors = error)
  }

  deleteNotification(not) {
    this._notificationService.deleteNotification(not).subscribe(
      res => {
        console.log(res);
        this.removeFromNotificationArrayList(this.selectedUserNotifications, not);
        //Need to reflect the change in the user also.
        for (let u of this.users) {
          if ((u.firstName + u.surname) == (this.selectedUser.firstName + this.selectedUser.surname)) {
            if (u.certifications) {
              this.removeFromNotificationArrayList(u.emailNotifications, not);
            }
          }
        }
      }, error => this.errors = error)
  }

  removeFromArrayList(list :Certificate[], crt: Certificate) {
    for (let item of list) {
      if (crt.id == item.id) {
        list.splice(list.indexOf(item), 1);
        break;
      }
    }
  }

  removeFromNotificationArrayList(list: EmailNotification[], not: EmailNotification) {
    for (let item of list) {
      if (not.id == item.id) {
        list.splice(list.indexOf(item), 1);
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

  toggleDisplayAddEmailNotification() {
    this.displayAddNotification = !this.displayAddNotification;
    if (this.displayAddNotification) {
      $("#myNewEmailNotificationModal").modal('show');
    } else {
      $("#myNewEmailNotificationModal").modal('hide');
    }
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
    if (this.selectedUser.emailNotifications) {
      this.selectedUserNotifications = this.selectedUser.emailNotifications;
    }
    else {
      this.selectedUserNotifications = new Array<EmailNotification>();
    }
  }

  addCertificateEntry() {
    this._msuserService.addCertificate(this.selectedUser.id, this.newCertificate).subscribe(result => {
      $("#myNewCertificateModal").modal('hide');
      //Update the identifier of the newly created cert so if we delete it, it will be deleted on database
      this.newCertificate.id = result as number;
      this.selectedUserCertifications.push(this.newCertificate);
      this.newCertificate = new Certificate(0, new Date(), new Date(), "")
    }, error => console.error(error));
  }

  addNotification() {
    this._msuserService.addEmailNotification(this.selectedUser.id, this.newEmailNotification).subscribe(result => {
      $("#myNewEmailNotificationModal").modal('hide');
      //Update the identifier of the newly created cert so if we delete it, it will be deleted on database
      this.newEmailNotification.id = result as number;
      this.selectedUserNotifications.push(this.newEmailNotification);
      this.newEmailNotification = new EmailNotification(0, '', '', '');
    }, error => console.error(error));
  }

  retrieveTimesheetsForDisplay() {
    return this.timesheets;
  }

  getTimesheetEntriesForTimesheet(index) {
    let ts = this.timesheets[index];
    return ts.timesheetEntries;
  }

  calculateDuration(entry: TimesheetEntry): number {
    var start = new Date("2018-01-01 " + entry.startTime);
    var end = new Date("2018-01-01 " + entry.endTime);

    var elapsedTimeInSec = (end.getTime() - start.getTime()) / 1000;
    var elapsedTimeInMins = elapsedTimeInSec / 60;

    return elapsedTimeInMins;
  }

  retrieveTimesheetsForUser() {
    this._msuserService.retrieveTimesheets(this.selectedUser.id).subscribe(result => {
      this.loading = false;
      this.timesheets = result;
    }, error => {
      this.errors = error
      this.loading = false;
    });
  }
}
