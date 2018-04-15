import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { UserRegistration } from '../../shared/models/user.registration.interface';
import { MsUserService } from '../../shared/services/msuser.service';

@Component({
  selector: 'app-registration-form',
  templateUrl: './registration-form.component.html'
})
export class RegistrationFormComponent implements OnInit {
  errors: string;
  isRequesting: boolean;
  submitted: boolean = false;

  constructor(private userService: MsUserService, private router: Router) {

  }

  ngOnInit() {

  }

  registerUser({ value, valid }: { value: UserRegistration, valid: boolean }) {
    this.submitted = true;
    this.errors = '';
    if (valid) {
      this.userService.register(value.email, value.password, value.firstName, value.lastName, value.username)
        .subscribe(
        result => {
          if (result) {
            //this.router.navigate(['/login'], { queryParams: { brandNew: true, email: value.email } });
            console.log("Successfully registreed user");
          }
        },
        errors => this.errors = errors);

      //this.userService.register(value.email, value.password, value.firstName, value.lastName, value.location)
      //  .finally(() => this.isRequesting = false)
      //  .subscribe(
      //    result => {
      //      if (result) {
      //        this.router.navigate(['/login'], { queryParams: { brandNew: true, email: value.email } });
      //      }
      //    },
      //    errors => this.errors = errors);
    }

  }
} 
