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
      this.userService.register(value.email, value.password, value.username, value.role)
        .subscribe(
        result => {
          if (result) {
            this.router.navigate(['/login'], { queryParams: { brandNew: true, username: value.username } });
          }
        },
        errors => this.errors = errors);
    }

  }
} 
