import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { HolidayRequest } from '../holidayrequest';
import { MsUserService } from '../shared/services/msuser.service';
import { HolidayService } from '../shared/services/holiday.service';
import { ApplicationUser } from '../applicationuser';

@Component({
  selector: 'holidays',
  templateUrl: './holidays.component.html'
})

export class HolidaysComponent {
  public holidayRequest: HolidayRequest = new HolidayRequest(0, new Date(), new Date(), 0, '', 'New');
  public holidayRequests: HolidayRequest[] = [];
  public supervisors: ApplicationUser[];
  public errors: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _msuserService: MsUserService, private _holidayService : HolidayService) {
    this._msuserService.getUsersWithRole('Supervisor').subscribe(result => {
      this.supervisors = result;
    }, error => this.errors = error)

    this._msuserService.getHolidayRequests().subscribe(result => {
      this.holidayRequests = result;
    }, error => this.errors = error)
  }

  submitHolidayRequest() {
    this._msuserService.addHolidayRequest(localStorage.getItem('client_id'), this.holidayRequest).subscribe(result => {
      //Update the identifier of the newly created cert so if we delete it, it will be deleted on database
      this.holidayRequest.id = result as number;
      this.holidayRequests.push(this.holidayRequest);

      //Create fresh version
      this.holidayRequest = new HolidayRequest(0, new Date(), new Date(), 0, '','New');
    }, error => console.error(error));
  }

  deleteHolidayRequest(hr) {
    this._holidayService.deleteHolidayRequest(hr).subscribe(
      res => {
        console.log(res);
        this.removeFromArrayList(hr);
      }, error => this.errors = error)
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
