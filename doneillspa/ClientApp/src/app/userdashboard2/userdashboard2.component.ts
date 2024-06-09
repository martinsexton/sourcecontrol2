import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { MsUserService } from '../shared/services/msuser.service';
import { ApplicationUser } from '../applicationuser';
import { UserRegistration } from '../shared/models/user.registration.interface';
import { PasswordReset } from '../passwordreset';

declare var $: any;

@Component({
  selector: 'userdashboard2',
  templateUrl: './userdashboard2.component.html',
  styleUrls: ['./userdashboard2.component.css']
})

export class UserDashboard2Component {
  public usersCurrentPage: number = 1;
  public usersPageLimit: number = 8;
  public users: ApplicationUser[];
  public selectedUser: ApplicationUser;
  public userMessage: string;
  newUser: ApplicationUser = new ApplicationUser('', '', '', '', '', '', false);
  public roles: string[] = ["Administrator", "Supervisor", "ChargeHand", "ElectR1", "ElectR2",
    "ElectR3", "Temp", "First Year Apprentice", "Second Year Apprentice",
    "Third Year Apprentice", "Fourth Year Apprentice", "Electrical Engineer", "Fire Engineer", "General Operative"];
  public resetPasswordDetails: PasswordReset = new PasswordReset('', '');
  public resettingPassword: boolean = false;
  public fulltimeStaffRole: string = "ChargeHand";

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _msuserService: MsUserService) {
    $('[data-toggle="tooltip"]').tooltip();
    this._msuserService.getUsers(this.usersCurrentPage, this.usersPageLimit).subscribe(result => {
      this.users = result;
      if (this.users.length > 0) {
        this.selectedUser = this.users[0];
        this.resetPasswordDetails.userid = this.selectedUser.id;
      }
    }, error => this.userMessage = error)
  }

  previousPage() {
    if (!this.disablePreviousButton()) {
      this.usersCurrentPage = this.usersCurrentPage - 1;
      this._msuserService.getUsers(this.usersCurrentPage, this.usersPageLimit).subscribe(result => {
        this.users = result;
        if (this.users.length > 0) {
          this.selectedUser = this.users[0];
          this.resetPasswordDetails.userid = this.selectedUser.id;
        }
      }, error => this.userMessage = error)
    }
  }

  nextPage() {
    if (!this.disableNextButton()) {
      this.usersCurrentPage = this.usersCurrentPage + 1;
      this._msuserService.getUsers(this.usersCurrentPage, this.usersPageLimit).subscribe(result => {
        this.users = result;
        if (this.users.length > 0) {
          this.selectedUser = this.users[0];
          this.resetPasswordDetails.userid = this.selectedUser.id;
        }
      }, error => this.userMessage = error)
    }
  }

  disableNextButton() {
    if (this.users) {
      return this.users.length < this.usersPageLimit;
    }
    else {
      return true;
    }
  }

  disablePreviousButton() {
    return this.usersCurrentPage == 1;
  }

  displayUserDetailsForEdit(user: ApplicationUser) {
    this.selectedUser = user;
    $("#myUserModal").modal('show');
  }

  displayResetPasswordDialog(user: ApplicationUser) {
    this.selectedUser = user;
    this.resetPasswordDetails.userid = this.selectedUser.id;
    $("#resetPwdModal").modal('show');
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

  showAddUser() {
    $("#myRegistrationModal").modal('show');
  }

  updateUser() {
    this._msuserService.updateUser(this.selectedUser).subscribe(
      result => {
        $("#myUserModal").modal('hide');
      });
  }

  retrieveListOfRoles() {
    return this.roles;
  }

  showUserMessage(msg: string) {
    this.userMessage = msg;
    $('.toast').toast('show');
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

  setUser(user: ApplicationUser) {
    this.selectedUser = user;
  }

  onKey(event: any) { // without type info
    this.usersCurrentPage = 1;
    if (event.target.value != '') {
      this._msuserService.getUsersBasedOnFilter(event.target.value, this.usersCurrentPage, this.usersPageLimit).subscribe(result => {
        this.users = result;
        if (this.users.length > 0) {
          this.selectedUser = this.users[0];
        }
      }, error => this.userMessage = error)
    }
    else {
      this._msuserService.getUsers(this.usersCurrentPage, this.usersPageLimit).subscribe(result => {
        this.users = result;
        if (this.users.length > 0) {
          this.selectedUser = this.users[0];
        }
      }, error => this.userMessage = error)
    }
  }
}
