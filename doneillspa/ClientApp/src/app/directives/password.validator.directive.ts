import { Directive, forwardRef } from '@angular/core';
import { NG_VALIDATORS, FormControl } from '@angular/forms';


function validatePasswordFactory() {
  return (c: FormControl) => {
    let PWD_REGEXP = /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-+]).{8,}$/i;

    return PWD_REGEXP.test(c.value) ? null : {
      validateEmail: {
        valid: false

      }

    };

  };
}

@Directive({
  selector: '[validatePassword][ngModel],[validatePassword][formControl]',
  providers: [
    { provide: NG_VALIDATORS, useExisting: forwardRef(() => PasswordValidator), multi: true }
  ]
})
export class PasswordValidator {
  validator: Function;

  constructor() {
    this.validator = validatePasswordFactory();

  }

  validate(c: FormControl) {
    return this.validator(c);

  }
} 
