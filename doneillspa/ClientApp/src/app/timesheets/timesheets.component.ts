import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'timesheets',
  templateUrl: './timesheets.component.html'
})

export class TimesheetComponent {
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {

  }
}
