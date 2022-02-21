import { Component, Input } from '@angular/core';
import { ApplicationUser } from '../../applicationuser';
import { MsUserService } from '../../shared/services/msuser.service';

declare var $: any;

@Component({
  selector: 'user-details',
  templateUrl: './userdetails.component.html',
  styleUrls: ['./userdetails.component.css']
})

export class UserDetailsComponent {
  @Input() user: ApplicationUser;

  public roles: string[] = ["Administrator", "Supervisor", "ChargeHand", "ElectR1", "ElectR2",
    "ElectR3", "Temp", "First Year Apprentice", "Second Year Apprentice",
    "Third Year Apprentice", "Fourth Year Apprentice"];

  public fulltimeStaffRole: string = "";

  constructor(private _msuserService: MsUserService) { }

  isUserEnabled() {
    return this.user.isEnabled;
  }

  displayUserDetailsForEdit() {
    $("#myUserModal").modal('show');
  }

  updateUser() {
    if (this.fulltimeStaffRole != "" || this.fulltimeStaffRole != this.user.role) {
      this.user.role = this.fulltimeStaffRole;
    }
    this._msuserService.updateUser(this.user).subscribe(
      result => {
        $("#myUserModal").modal('hide');
      });
  }

  retrieveListOfRoles() {
      return this.roles
  }
}
