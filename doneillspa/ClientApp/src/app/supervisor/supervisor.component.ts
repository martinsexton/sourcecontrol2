import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { HolidayRequest } from '../holidayrequest';

import { MsUserService } from '../shared/services/msuser.service';
import { HolidayService } from '../shared/services/holiday.service';


declare var $: any;

@Component({
  selector: 'supervisor',
  templateUrl: './supervisor.component.html'
})

export class SupervisorComponent {
  public errors: string;
  public holidayRequests: HolidayRequest[] = [];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _msuserService: MsUserService, private _holidayService : HolidayService) {
    this._msuserService.getHolidayRequestsForApproval().subscribe(result => {
      this.holidayRequests = result;
    }, error => this.errors = error)
  }

  approveHolidayRequest(hr) {
    this._holidayService.approveHolidayRequest(hr).subscribe(
      res => {
        console.log(res);
      }, error => this.errors = error)
  }
}
