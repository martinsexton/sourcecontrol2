import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { UserRegistration } from '../../shared/models/user.registration.interface';
import { MsUserService } from '../../shared/services/msuser.service';
import { IdentityRole } from '../../identityrole';

declare var $: any;

@Component({
  selector: 'app-registration-form',
  templateUrl: './registration-form.component.html'
})
export class RegistrationFormComponent implements OnInit {
  errors: string;
  isRequesting: boolean;
  submitted: boolean = false;

  constructor(private userService: MsUserService, private router: Router) {}

  ngOnInit() {
    $("#myRegistrationModal").modal('show');
  }

  registerUser({ value, valid }: { value: UserRegistration, valid: boolean }) {
    this.submitted = true;
    this.errors = '';
    if (valid) {
      this.userService.register(value.email, value.password, value.firstname, value.surname, value.role, value.phone)
        .subscribe(
        result => {
          if (result) {
            $("#myRegistrationModal").modal('hide');
            this.router.navigate(['/login'], { queryParams: { brandNew: true, firstname: value.firstname, surname: value.surname } });
          }
        },
        errors => this.errors = errors);
    }
  }

  navigateHome() {
    this.router.navigate(['/']);
  }
} 
