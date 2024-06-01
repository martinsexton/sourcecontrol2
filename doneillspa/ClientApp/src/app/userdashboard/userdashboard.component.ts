import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { ApplicationUser } from '../applicationuser';
import { Project } from '../project';
import { MsUserService } from '../shared/services/msuser.service';
import { Timesheet } from '../timesheet';
import { TimesheetEntry } from '../timesheetentry';
import { FormBuilder, FormControl } from '@angular/forms';
import {
  ProjectService
} from '../shared/services/project.service';

import {
  TimesheetService
} from '../shared/services/timesheet.service';

import { UserRegistration } from '../shared/models/user.registration.interface';
import { PasswordReset } from '../passwordreset';
import * as moment from 'moment';
import { formatDate } from '@angular/common';

declare var $: any;

@Component({
  selector: 'userdashboard',
  templateUrl: './userdashboard.component.html',
  styleUrls: ['./userdashboard.component.css']
})

export class UserDashboardComponent implements OnInit {
  public selectedUser: ApplicationUser;
  public selectedUserRow: number;
  public userMessage: string;
  public resetPasswordDetails: PasswordReset = new PasswordReset('', '');
  public loadingTimesheets: boolean = false;
  public resettingPassword: boolean = false;
  public userImage: string = "user.png";
  public contractorRole: string = "Loc1";
  public fulltimeStaffRole: string = "ChargeHand";
  newUser: ApplicationUser = new ApplicationUser('', '', '', '', '', '', false);
  public usersCurrentPage: number = 1;
  public usersPageLimit: number = 15;
  public activeTab: string = "Submitted";
  public errors: string;
  public filteredTimesheets: Timesheet[];
  public selectedTimesheet: Timesheet;
  public timesheetsCurrentPage: number = 1;
  public filterName: string;
  public timesheetsForCurrentPage: Timesheet[];
  public timesheets: Timesheet[];
  public pageLimit: number = 10;
  searchFromDate = new FormControl('');
  searchToDate = new FormControl('');
  selectedMoment: moment.Moment = moment();

  public roles: string[] = ["Administrator", "Supervisor", "ChargeHand", "ElectR1", "ElectR2",
    "ElectR3", "Temp", "First Year Apprentice", "Second Year Apprentice",
    "Third Year Apprentice", "Fourth Year Apprentice", "Electrical Engineer", "Fire Engineer", "General Operative"];

  public contractorRoles: string[] = ["Loc1", "Loc2",
    "Loc3"];

  public users: ApplicationUser[];
  public loading = true;
  public addingContractor = false;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService,
    private _timesheetService: TimesheetService, private _msuserService: MsUserService) {
    $('[data-toggle="tooltip"]').tooltip();
    this._msuserService.getUsers(this.usersCurrentPage, this.usersPageLimit ).subscribe(result => {
      this.users = result;
      this.loading = false;
      if (this.users.length > 0) {
        this.selectedUser = this.users[0];
        this.resetPasswordDetails.userid = this.selectedUser.id;
        this.selectedUserRow = 0;
      }
    }, error => this.userMessage = error)
  }

  ngOnInit() {
    this.initForm();
  }

  initForm() {
    var toDate = this.selectedMoment.toDate();
    var fromDate = this.selectedMoment.subtract(3, 'months').toDate();

    this.searchFromDate.setValue(formatDate(fromDate, "yyyy-MM-dd", "en"));
    this.searchToDate.setValue(formatDate(toDate, "yyyy-MM-dd", "en"));
  }

  submittedTabClicked() {
    this._timesheetService.getUserSubmittedTimesheets(this.selectedUser.id).subscribe(result => {
      this.timesheets = result;
      this.activeTab = "Submitted";
      this.setTimesheetsByState();
    }, error => this.errors = error);
  }


  approvedTabClicked() {
    this._timesheetService.getUserApprovedTimesheets(this.selectedUser.id).subscribe(result => {
      this.timesheets = result;
      this.activeTab = "Approved";
      this.setTimesheetsByState();
    }, error => this.errors = error);
  }

  rejectedTabClicked() {
    this._timesheetService.getUserRejectedTimesheets(this.selectedUser.id).subscribe(result => {
      this.timesheets = result;
      this.activeTab = "Rejected";
      this.setTimesheetsByState();
    }, error => this.errors = error);
  }

  archievedTabClicked() {
    this._timesheetService.getUserArchievedTimesheetsForRange(this.selectedUser.id, this.searchFromDate.value, this.searchToDate.value).subscribe(result => {
      this.timesheets = result;
      this.activeTab = "Archieved";
      this.setTimesheetsByState();
    }, error => this.errors = error);
  }

  onTimesheetSelected(ts: Timesheet) {
    this.selectedTimesheet = ts;
  }

  filterArchievedTab() {
    this._timesheetService.getUserArchievedTimesheetsForRange(this.selectedUser.id, this.searchFromDate.value, this.searchToDate.value).subscribe(result => {
      this.timesheets = result;
      this.activeTab = "Archieved";
      this.setTimesheetsByState();
    }, error => this.errors = error);
  }

  setTimesheetsByState() {
    this.filteredTimesheets = [];
    for (let item of this.timesheets) {
      if (item.status.toUpperCase() == this.activeTab.toUpperCase()) {
        this.filteredTimesheets.push(item);
      }
    }

    this.selectedTimesheet = null;
    if (this.filteredTimesheets) {
      this.selectedTimesheet = this.filteredTimesheets[0];
    }
    //Reset page count and refresh.
    this.timesheetsCurrentPage = 1;
    this.setupTimehseetsForCurrentPage();
  }

  setupTimehseetsForCurrentPage() {
    this.loading = true;

    var startingIndex = 0;
    var index = 0;

    this.selectedTimesheet = null;

    //reset client current page array
    this.timesheetsForCurrentPage = [];

    if (this.timesheetsCurrentPage > 1) {
      startingIndex = (this.timesheetsCurrentPage - 1) * this.pageLimit;
    }

    for (let ts of this.filteredTimesheets) {
      if (index >= startingIndex && index < (startingIndex + this.pageLimit)) {
        this.timesheetsForCurrentPage.push(ts);

        //Setup selected client as the firt client on current page.
        if (this.selectedTimesheet == null) {
          this.selectedTimesheet = ts;
        }
      }
      index = index + 1;
    }
    this.loading = false;
  }

  determinePageCount() {
    var pageCount = 1;
    if (this.filteredTimesheets) {
      var numberOfTimesheets = this.filteredTimesheets.length;

      if (numberOfTimesheets > 0) {
        var totalPages_pre = Math.floor((numberOfTimesheets / this.pageLimit));
        pageCount = (numberOfTimesheets % this.pageLimit) == 0 ? totalPages_pre : totalPages_pre + 1
      }
      return pageCount;
    }
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

  previousTimesheetsPage() {
    this.timesheetsCurrentPage = this.timesheetsCurrentPage - 1;
    this.setupTimehseetsForCurrentPage();
  }

  nextTimesheetsPage() {
    this.timesheetsCurrentPage = this.timesheetsCurrentPage + 1;
    this.setupTimehseetsForCurrentPage();
  }

  previousPage() {
    this.usersCurrentPage = this.usersCurrentPage - 1;
    this._msuserService.getUsers(this.usersCurrentPage, this.usersPageLimit).subscribe(result => {
      this.users = result;
      this.loading = false;
      if (this.users.length > 0) {
        this.selectedUser = this.users[0];
        this.resetPasswordDetails.userid = this.selectedUser.id;
        this.selectedUserRow = 0;
      }
    }, error => this.userMessage = error)
  }

  nextPage() {
    this.usersCurrentPage = this.usersCurrentPage + 1;
    this._msuserService.getUsers(this.usersCurrentPage, this.usersPageLimit).subscribe(result => {
      this.users = result;
      this.loading = false;
      if (this.users.length > 0) {
        this.selectedUser = this.users[0];
        this.resetPasswordDetails.userid = this.selectedUser.id;
        this.selectedUserRow = 0;
      }
    }, error => this.userMessage = error)
  }

  disableNextButton() {
    return this.users.length < this.usersPageLimit;
  }

  disablePreviousButton() {
    return this.usersCurrentPage == 1;
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
    return this.users;
  }

  displaySelectedUserDetails(user, index) {
    this.selectedUser = user;
    this.selectedUserRow = index;
    this.resetPasswordDetails.userid = this.selectedUser.id;

    //Clear Timesheets
    this.selectedTimesheet = null;
    this.timesheets = [];
    this.filteredTimesheets = [];
    this.timesheetsForCurrentPage = [];

    this.retrieveTimesheetsForUser();
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
      this.setTimesheetsByState();
    }, error => {
      this.userMessage = error
      this.loadingTimesheets = false;
    });
  }
}
