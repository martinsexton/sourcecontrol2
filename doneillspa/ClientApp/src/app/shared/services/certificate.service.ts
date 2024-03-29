
import {catchError, retry} from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Project } from '../../project';
import { Certificate } from '../../certificate';
import { Observable } from 'rxjs';
import { HttpServiceBase } from './httpservicebase';
import { HolidayRequest } from '../../holidayrequest';

@Injectable()
export class CertificateService extends HttpServiceBase{

  constructor(_httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    super(_httpClient, baseUrl); 
  }

  deleteCertification(crt: Certificate) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.delete(this._baseurl + 'api/certification/' + crt.id, {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', 'Bearer ' + authToken)
    }).pipe(
    retry(5),
    catchError(this.handleError),);
  }
}
