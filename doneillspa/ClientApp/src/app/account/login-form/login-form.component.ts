import { Subscription } from 'rxjs';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { Credentials } from '../../shared/models/credentials.interface';
import { MsUserService } from '../../shared/services/msuser.service';
import { LoginResponse } from '../../shared/models/loginresponse.interface';

declare var $: any;

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html'
})
export class LoginFormComponent implements OnInit, OnDestroy {


  private subscription: Subscription;

  brandNew: boolean;
  errors: string;
  submitted: boolean = false;
  credentials: Credentials = { firstname: '', surname: '', password: '' };
  public loginProgress = false;


  constructor(private userService: MsUserService, private router: Router, private activatedRoute: ActivatedRoute) {}


  ngOnInit() {
    $("#myLoginModal").modal('show');

    // subscribe to router event 
    this.subscription = this.activatedRoute.queryParams.subscribe(
      (param: any) => {
        this.brandNew = param['brandNew'];
        this.credentials.firstname = param['firstname'];
        this.credentials.surname = param['surname'];
      });
  }


  ngOnDestroy() {
    // prevent memory leak by unsubscribing 
    this.subscription.unsubscribe();
  }


  login({ value, valid }: { value: Credentials, valid: boolean }) {
    this.loginProgress = true;
    this.submitted = true;
    this.errors = '';
    if (valid) {
      this.userService.login(value.firstname, value.surname, value.password)
        .subscribe(
        result => {
          this.loginProgress = false;
          if (result) {
            if (result.error) {
              this.errors = result.error;
            }
            else {
              $("#myLoginModal").modal('hide');
              localStorage.setItem('auth_token', result.auth_token);
              localStorage.setItem('role', result.role);
              localStorage.setItem('client_id', result.id);
              localStorage.setItem('username', value.firstname + value.surname);
              localStorage.setItem('firstname', value.firstname);
              localStorage.setItem('surname', value.surname);
              this.router.navigate(['/']);
            }
          }
        }, responseError => this.errors = responseError);
    }
  }

  navigateHome() {
    this.router.navigate(['/']);
  }
}
