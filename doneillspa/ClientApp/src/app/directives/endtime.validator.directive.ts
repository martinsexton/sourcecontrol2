import { Directive, forwardRef, Input } from '@angular/core';
import { NG_VALIDATORS, FormControl, Validator, AbstractControl } from '@angular/forms';


function validateEndtimeFactory(startTime : string) {
  return (c: FormControl) => {
    //Need to add some code here to cast string 08:00 to some time, so we can check if value is > or < that time
    let start = startTime.split(":");
    let end = c.value.split(":");

    let startHr = parseInt(start[0]);
    let startMin = parseInt(start[1]);

    let endHr = parseInt(end[0]);
    let endMin = parseInt(end[1]);

    let startInMinutes = (startHr * 60) + startMin;
    let endInMinutes = (endHr * 60) + endMin;

    return endInMinutes > startInMinutes ? null : {
      validateEndtime: {
        valid: false
      }
    };
  };
}


@Directive({
  selector: '[validateEndtime]',
  providers: [
    { provide: NG_VALIDATORS, useExisting: forwardRef(() => EndtimeValidator), multi: true }
  ]
})
export class EndtimeValidator implements Validator {
  validator: Function;
  @Input() startTime: string;

  validate(control: FormControl): { [key: string]: any } | null {
    return this.startTime ? validateEndtimeFactory(this.startTime)(control)
      : null;
  }
} 
