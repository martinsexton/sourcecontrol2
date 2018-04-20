import { Component } from '@angular/core';
import { MsUserService } from '../shared/services/msuser.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  constructor(private userService: MsUserService) { }

  isUserLoggedIn() {
    return this.userService.isLoggedIn();
  }
}
