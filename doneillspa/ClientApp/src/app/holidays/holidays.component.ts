import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { HolidayRequest } from '../holidayrequest';
import { MsUserService } from '../shared/services/msuser.service';
import { HolidayService } from '../shared/services/holiday.service';
import { ApplicationUser } from '../applicationuser';

declare var $: any;

@Component({
  selector: 'holidays',
  templateUrl: './holidays.component.html',
  styleUrls: ['./holidays.component.css']
})

export class HolidaysComponent {
  public holidayRequest: HolidayRequest = new HolidayRequest(0, new Date(), new Date(), 0, '', 'New');
  public holidayRequests: HolidayRequest[];// = [];
  public supervisors: ApplicationUser[];
  public errors: string;
  public loadingHolidays: Boolean = false;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _msuserService: MsUserService, private _holidayService: HolidayService) {
    this.loadingHolidays = true;
    this._msuserService.getUsersWithRole('Supervisor').subscribe(result => {
      this.loadingHolidays = false;
      this.supervisors = result;
    }, error => {
      this.loadingHolidays = false;
      this.errors = "Failed to retrieve holiday requests"
      $('.toast').toast('show');
    })

    this._msuserService.getHolidayRequests().subscribe(result => {
      this.holidayRequests = result;
      this.loadingHolidays = false;
    }, error => {
      this.loadingHolidays = false;
      this.errors = "Failed to retrieve holiday requests"
      $('.toast').toast('show');
    })
  }

  submitHolidayRequest() {
    this._msuserService.addHolidayRequest(localStorage.getItem('client_id'), this.holidayRequest).subscribe(result => {
      $("#myNewHolidayModal").modal('hide');
      //Update the identifier of the newly created cert so if we delete it, it will be deleted on database
      this.holidayRequest.id = result as number;
      this.holidayRequests.push(this.holidayRequest);

      //Create fresh version
      this.holidayRequest = new HolidayRequest(0, new Date(), new Date(), 0, '','New');
    }, error => {
      $("#myNewHolidayModal").modal('hide');
      this.errors = "Failed to submit holiday request"
      $('.toast').toast('show');
    })
  }

  deleteHolidayRequest(hr) {
    this._holidayService.deleteHolidayRequest(hr).subscribe(
      res => {
        console.log(res);
        this.removeFromArrayList(hr);
      }, error => {
        this.errors = "Unable to delete holiday request"
        $('.toast').toast('show');
      })
  }


  removeFromArrayList(h: HolidayRequest) {
    for (let item of this.holidayRequests) {
      if (h.id == item.id) {
        this.holidayRequests.splice(this.holidayRequests.indexOf(item), 1);
        break;
      }
    }
  }
}
