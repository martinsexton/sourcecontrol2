
import { catchError, retry } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Project } from '../../project';
import { HttpServiceBase } from './httpservicebase';
import { Tenant } from '../../Tenant';

@Injectable()
export class TenantService extends HttpServiceBase {

  constructor(_httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    super(_httpClient, baseUrl);
  }

 
  getTenants() {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<Tenant[]>(this._baseurl + 'api/tenant',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
        retry(5),
        catchError(this.handleError),);
  }
}
