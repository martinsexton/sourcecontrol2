import { Injectable } from '@angular/core';
import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Project } from '../../project';
import { Certificate } from '../../certificate';
import { Observable } from 'rxjs/Observable';
import { HttpServiceBase } from './httpservicebase';

@Injectable()
export class CertificateService extends HttpServiceBase{

  constructor(_httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    super(_httpClient, baseUrl); 
  }

  getCertifications(userId: string) {
    let authToken = localStorage.getItem('auth_token');
    let headers = new HttpHeaders();
    headers = headers.append('Content-Type', 'application/json');
    headers = headers.append('Authorization', 'Bearer ' + authToken);

    return this._httpClient.get<Certificate[]>(this._baseurl + 'api/certification/user/' + userId,
      {
        headers
      })
      .catch(this.handleError);
  }

  deleteCertification(crt: Certificate) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.delete(this._baseurl + 'api/certification/' + crt.id, {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', 'Bearer ' + authToken)
    })
    .catch(this.handleError);
  }
}
