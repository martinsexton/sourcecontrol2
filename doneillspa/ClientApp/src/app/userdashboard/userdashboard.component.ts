import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
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

import { HolidayRequest } from '../holidayrequest';
import { UserRegistration } from '../shared/models/user.registration.interface';
import { PasswordReset } from '../passwordreset';

declare var $: any;

@Component({
  selector: 'userdashboard',
  templateUrl: './userdashboard.component.html',
  styleUrls: ['./userdashboard.component.css']
})

export class UserDashboardComponent {
  public selectedUser: ApplicationUser;
  public selectedUserRow: number;
  public selectedUsersHolidayRequests: HolidayRequest[] = [];
  public userMessage: string;
  public resetPasswordDetails: PasswordReset = new PasswordReset('', '');
  public loadingTimesheets: boolean = false;
  public resettingPassword: boolean = false;
  public userImage: string = "user.png";
  public contractorRole: string = "Loc1";
  public fulltimeStaffRole: string = "ChargeHand";
  newUser: ApplicationUser = new ApplicationUser('', '', '', '', '', '', false);
  public usersCurrentPage: number = 1;
  public usersForCurrentPage: ApplicationUser[];
  public usersPageLimit: number = 15;
 

  public filterName: string;

  public timesheets: Timesheet[];

  public roles: string[] = ["Administrator", "Supervisor", "ChargeHand", "ElectR1", "ElectR2",
    "ElectR3", "Temp", "First Year Apprentice", "Second Year Apprentice",
    "Third Year Apprentice", "Fourth Year Apprentice"];

  public contractorRoles: string[] = ["Loc1", "Loc2",
    "Loc3"];

  public users: ApplicationUser[];
  public loading = true;
  public addingContractor = false;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService,
    private _timesheetService: TimesheetService, private _msuserService: MsUserService) {
    $('[data-toggle="tooltip"]').tooltip();
    this._msuserService.getUsers().subscribe(result => {
      this.users = result;
      this.loading = false;
      if (this.users.length > 0) {
        this.selectedUser = this.users[0];
        this.setupUsersForCurrentPage();
        this.resetPasswordDetails.userid = this.selectedUser.id;
        this.selectedUserRow = 0;
        if (this.selectedUser.holidayRequests) {
          this.selectedUsersHolidayRequests = this.selectedUser.holidayRequests;
        }
        else {
          this.selectedUsersHolidayRequests = new Array<HolidayRequest>();
        }
      }
    }, error => this.userMessage = error)
  }

  showAddUser() {
    this.addingContractor = false;
    $("#myRegistrationModal").modal('show');
  }

  isContractor() {
    if (!this.selectedUser) {
      return false;
    }
    else {
      return (this.selectedUser.role == "Loc1" || this.selectedUser.role == "Loc2" || this.selectedUser.role == "Loc3");
    }
  }

  previousPage() {
    this.usersCurrentPage = this.usersCurrentPage - 1;
    this.setupUsersForCurrentPage();
  }

  nextPage() {
    this.usersCurrentPage = this.usersCurrentPage + 1;
    this.setupUsersForCurrentPage();
  }

  determineUsersPageCount() {
    var pageCount = 1;

    var numberOfUsers = this.users.length;

    if (numberOfUsers > 0) {
      var totalPages_pre = Math.floor((numberOfUsers / this.usersPageLimit));
      pageCount = (numberOfUsers % this.usersPageLimit) == 0 ? totalPages_pre : totalPages_pre + 1
    }
    return pageCount;
  }

  setupUsersForCurrentPage() {
    var startingIndex = 0;
    var index = 0;

    //reset client current page array
    this.usersForCurrentPage = [];

    if (this.usersCurrentPage > 1) {
      startingIndex = (this.usersCurrentPage - 1) * this.usersPageLimit;
    }

    for (let p of this.users) {
      if (index >= startingIndex && index < (startingIndex + this.usersPageLimit)) {
        this.usersForCurrentPage.push(p);
      }
      index = index + 1;
    }
  }

  showAddContractUser() {
    this.addingContractor = true;
    $("#myContractorRegistrationModal").modal('show');
  }

  retrieveListOfRoles() {
    if (this.addingContractor == false) {
      return this.roles
    }
    else {
      return this.contractorRoles;
    }
  }

  GetImageName() {
    if (this.selectedUser) {
      if (this.selectedUser.role == "Administrator") {
        return "admin.png";
      }
      else {
        return "user.png";
      }
    } else {
      return "";
    }
  }

  registerUser({ value, valid }: { value: UserRegistration, valid: boolean }) {
    this.userMessage = '';
    if (valid) {
      this._msuserService.register(value.email, value.password, value.firstname, value.surname, value.role, value.phone)
        .subscribe(
        result => {
            if (result) {
              this.newUser.id = result as string;
              this.newUser.firstName = value.firstname;
              this.newUser.surname = value.surname;
              this.newUser.isEnabled = true;
              this.newUser.phoneNumber = value.phone;
              this.newUser.role = value.role;

              this.users.push(this.newUser);

            $("#myRegistrationModal").modal('hide');
            $("#myContractorRegistrationModal").modal('hide');
          }
        },
        errors => this.userMessage = errors);
    }
  }

  showUserMessage(msg: string) {
    this.userMessage = msg;
    $('.toast').toast('show');
  }

  resetPassword() {
      this.resettingPassword = true;
      this._msuserService.resetPassword(this.resetPasswordDetails).subscribe(
        result => {
        this.resettingPassword = false;
        this.resetPasswordDetails.password = '';
        //this.passwordReset = true;
        }, error => {
          this.resettingPassword = false;
          this.userMessage = error
          this.showUserMessage('Failed to reset password. Please ensure password is at least 6 characters long including upper and lower case letters.');
        });
  }

  retrieveUsersToDisplay() {
    return this.usersForCurrentPage;
  }

  displaySelectedUserDetails(user, index) {
    this.selectedUser = user;
    this.selectedUserRow = index;
    this.resetPasswordDetails.userid = this.selectedUser.id;

    //Clear Timesheets
    this.timesheets = new Array<Timesheet>();

    if (this.selectedUser.holidayRequests) {
      this.selectedUsersHolidayRequests = this.selectedUser.holidayRequests;
    }
    else {
      this.selectedUsersHolidayRequests = new Array<HolidayRequest>();
    }
  }

  retrieveTimesheetsForDisplay() {
    return this.timesheets;
  }

  getActiveTimesheet(index) {
    return this.timesheets[index];;
  }

  retrieveTimesheetsForUser() {
    this.loadingTimesheets = true;
    this._msuserService.retrieveTimesheets(this.selectedUser.id).subscribe(result => {
      this.loadingTimesheets = false;
      this.timesheets = result;
    }, error => {
      this.userMessage = error
      this.loadingTimesheets = false;
    });
  }
}
