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
    return this.isUserLoggedIn && this.userService.isAdministrator();
  }

  isWorker() {
    return this.isUserLoggedIn && !this.userService.isAdministrator();
  }

  isSupervisor() {
    return this.isUserLoggedIn && this.userService.isSupervisor();
  }

  isUserLoggedIn() {
    return this.userService.isLoggedIn();
  }

  retrieveTenant() : string {
    return localStorage.getItem('tenantname');
  }

  logOut() {
    this.userService.logout();
  }
}
