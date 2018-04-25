import { Component } from '@angular/core';
import { MsUserService } from '../shared/services/msuser.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  constructor(private userService: MsUserService) { }

  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  isAdministrator() {
    return this.userService.isAdministrator();
  }

  isWorker() {
    return this.isUserLoggedIn && !this.userService.isAdministrator();
  }

  isUserLoggedIn() {
    return this.userService.isLoggedIn();
  }

  logOut() {
    this.userService.logout();
  }
}
