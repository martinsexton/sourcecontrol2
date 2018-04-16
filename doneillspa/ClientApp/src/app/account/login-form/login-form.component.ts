import { Subscription } from 'rxjs';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { Credentials } from '../../shared/models/credentials.interface';
import { MsUserService } from '../../shared/services/msuser.service';
import { LoginResponse } from '../../shared/models/loginresponse.interface';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html'
})
export class LoginFormComponent implements OnInit, OnDestroy {


  private subscription: Subscription;


  brandNew: boolean;
  errors: string;
  isRequesting: boolean;
  submitted: boolean = false;
  credentials: Credentials = { username: '', password: '' };


  constructor(private userService: MsUserService, private router: Router, private activatedRoute: ActivatedRoute) { }


  ngOnInit() {


    // subscribe to router event 
    this.subscription = this.activatedRoute.queryParams.subscribe(
      (param: any) => {
        this.brandNew = param['brandNew'];
        this.credentials.username = param['username'];
      });
  }


  ngOnDestroy() {
    // prevent memory leak by unsubscribing 
    this.subscription.unsubscribe();

  }


  login({ value, valid }: { value: Credentials, valid: boolean }) {
    this.submitted = true;
    this.isRequesting = true;
    this.errors = '';
    if (valid) {
      this.userService.login(value.username, value.password)
        .subscribe(
        result => {
          if (result) {
            if (result.error) {
              this.errors = result.error;
            }
            else {
              localStorage.setItem('auth_token', result.auth_token);
              this.router.navigate(['/projects']);
            }
          }
        });
        //error => this.errors = error);

    }
  }
}
