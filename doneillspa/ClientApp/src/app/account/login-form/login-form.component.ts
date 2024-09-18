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
              this.populateSessionData(result, value);
              this.populateTenantInformation(result);
              this.navigateToNextComponent(result);
            }
          }
        }, responseError => this.errors = responseError);
    }
  }

  navigateToNextComponent(result: LoginResponse) {
    if (result.role === "Administrator") {
      //Navigate to choose tenant component
      this.router.navigate(['/choosetenant']);
    }
    else {
      //As an engineer we want to navigate directly to timesheet page
      this.router.navigate(['/timesheets2']);
    }
  }

  populateTenantInformation(result: LoginResponse) {
    if (result.role === "Administrator") {
      //Reset tenant details on login
      localStorage.setItem('tenant', '1');
      localStorage.setItem('tenantname', 'Electrical Business Unit');
    }
    else {
      localStorage.setItem('tenant', result.tenantId);
    }
  }

  populateSessionData(result: LoginResponse, value: Credentials) {
    localStorage.setItem('auth_token', result.auth_token);
    localStorage.setItem('role', result.role);
    localStorage.setItem('client_id', result.id);
    localStorage.setItem('username', value.firstname + value.surname);
    localStorage.setItem('firstname', value.firstname);
    localStorage.setItem('surname', value.surname);
  }

  navigateHome() {
    this.router.navigate(['/']);
  }
}
