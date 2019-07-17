import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { HolidayRequest } from '../holidayrequest';

import { MsUserService } from '../shared/services/msuser.service';
import { HolidayService } from '../shared/services/holiday.service';


declare var $: any;

@Component({
  selector: 'supervisor',
  templateUrl: './supervisor.component.html',
  styleUrls: ['./supervisor.component.css']
})

export class SupervisorComponent {
  public errors: string;
  public holidayRequests: HolidayRequest[] = [];
  public pendingHolidayRequests: HolidayRequest[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _msuserService: MsUserService, private _holidayService : HolidayService) {
    this._msuserService.getHolidayRequestsForApproval().subscribe(result => {
      this.holidayRequests = result;
      this.filterPendingHolidayRequests();
    }, error => this.errors = error)
  }

  filterPendingHolidayRequests() {
    this.pendingHolidayRequests = [];
    for (let item of this.holidayRequests) {
      if (item.status.toUpperCase() == 'NEW') {
        this.pendingHolidayRequests.push(item);
      }
    }
  }

  approveHolidayRequest(hr) {
    this._holidayService.approveHolidayRequest(hr).subscribe(
      res => {
        console.log(res);
      }, error => this.errors = error)
  }

  rejectHolidayRequest(hr) {
    this._holidayService.rejectHolidayRequest(hr).subscribe(
      res => {
        console.log(res);
      }, error => this.errors = error)
  }

  removeFromArrayList(h: HolidayRequest) {
    for (let item of this.pendingHolidayRequests) {
      if (h.id == item.id) {
        this.pendingHolidayRequests.splice(this.pendingHolidayRequests.indexOf(item), 1);
        break;
      }
    }
  }
}
