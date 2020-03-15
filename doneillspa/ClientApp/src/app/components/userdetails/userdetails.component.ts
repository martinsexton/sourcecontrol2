import { Component, Input } from '@angular/core';
import { ApplicationUser } from '../../applicationuser';
import { MsUserService } from '../../shared/services/msuser.service';

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

  enableUser() {
    this.user.isEnabled = true;
    this._msuserService.updateUser(this.user).subscribe(
      result => {
        console.info(result);
      });
  }

  disableUser() {
    this.user.isEnabled = false;
    this._msuserService.updateUser(this.user).subscribe(
      result => {
        console.info(result);
      });
  }
}
