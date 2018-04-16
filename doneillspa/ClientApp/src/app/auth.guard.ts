import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { MsUserService } from './shared/services/msuser.service';


@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private user: MsUserService, private router: Router) { }


  canActivate() {


    if (!this.user.isLoggedIn()) {
      this.router.navigate(['/account/login']);
      return false;

    }


    return true;

  }
} 
