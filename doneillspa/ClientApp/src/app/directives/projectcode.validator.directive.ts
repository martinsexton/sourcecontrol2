import { Directive, forwardRef, Input } from '@angular/core';
import { NG_VALIDATORS, FormControl, Validator, AbstractControl } from '@angular/forms';
import { Client } from '../client';


function validateProjectCodeFactory(codes: string[]) {
  return (c: FormControl) => {
    //Compare c.value with the entries in codes to see if it already exists
    let codeExists = false;

    if (c.value) {
      if (codes.length > 0) {
        codeExists = codes.indexOf(c.value.toUpperCase()) > -1
      }
    }

    return !codeExists ? null : {
      validateProjectCode: {
        valid: false
      }
    };
  };
}


@Directive({
  selector: '[validateProjectCode]',
  providers: [
    { provide: NG_VALIDATORS, useExisting: forwardRef(() => ProjectCodeValidator), multi: true }
  ]
})
export class ProjectCodeValidator implements Validator {
  validator: Function;
  @Input() codes: string[];

  validate(control: FormControl): { [key: string]: any } | null {
    return this.codes ? validateProjectCodeFactory(this.codes)(control)
      : null;
  }
} 
