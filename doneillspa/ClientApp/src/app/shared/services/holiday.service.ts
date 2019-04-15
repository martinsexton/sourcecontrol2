import { Injectable } from '@angular/core';
import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Project } from '../../project';
import { HolidayRequest } from '../../holidayrequest';
import { Observable } from 'rxjs/Observable';
import { HttpServiceBase } from './httpservicebase';

@Injectable()
export class HolidayService extends HttpServiceBase {

  constructor(_httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    super(_httpClient, baseUrl);
  }

  updateHolidayRequest(h: HolidayRequest) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.put(this._baseurl + 'api/holidayrequest/', h, {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', 'Bearer ' + authToken)
    })
      .retry(5)
      .catch(this.handleError);
  }

  approveHolidayRequest(h: HolidayRequest) {
    h.status = 'Approved';
    return this.updateHolidayRequest(h);
  }

  rejectHolidayRequest(h: HolidayRequest) {
    h.status = 'Rejected';
    return this.updateHolidayRequest(h);
  }

  deleteHolidayRequest(h: HolidayRequest) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.delete(this._baseurl + 'api/holidayrequest/' + h.id, {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', 'Bearer ' + authToken)
    })
      .retry(5)
      .catch(this.handleError);
  }
}
