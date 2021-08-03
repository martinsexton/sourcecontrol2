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

  constructor(private _msuserService: MsUserService) { }

  isUserEnabled() {
    return this.user.isEnabled;
  }

  displayUserDetailsForEdit() {
    $("#myUserModal").modal('show');
  }

  updateUser() {
    this._msuserService.updateUser(this.user).subscribe(
      result => {
        $("#myUserModal").modal('hide');
      });
  }
}
