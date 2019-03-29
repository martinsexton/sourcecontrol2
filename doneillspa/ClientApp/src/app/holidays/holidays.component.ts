import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { HolidayRequest } from '../holidayrequest';
import { MsUserService } from '../shared/services/msuser.service';

@Component({
  selector: 'holidays',
  templateUrl: './holidays.component.html'
})

export class HolidaysComponent {
  public holidayReqeust: HolidayRequest = new HolidayRequest(0, new Date(),0);

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _msuserService: MsUserService) {

  }

  submitHolidayRequest() {
    this._msuserService.addHolidayRequest(localStorage.getItem('client_id'), this.holidayReqeust).subscribe(result => {
      //Update the identifier of the newly created cert so if we delete it, it will be deleted on database
      this.holidayReqeust.id = result as number;
      this.holidayReqeust = new HolidayRequest(0, new Date(), 0);
    }, error => console.error(error));
  }
}
