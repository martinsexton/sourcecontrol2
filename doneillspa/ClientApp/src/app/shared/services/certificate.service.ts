import { Injectable } from '@angular/core';
import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Project } from '../../project';
import { Certificate } from '../../certificate';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class CertificateService {
  _baseurl: String;

  constructor(private _httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this._baseurl = baseUrl;
  }

  getCertifications(userId: string) {
    let authToken = localStorage.getItem('auth_token');
    let headers = new HttpHeaders();
    headers = headers.append('Content-Type', 'application/json');
    headers = headers.append('Authorization', 'Bearer ' + authToken);

    return this._httpClient.get<Certificate[]>(this._baseurl + 'api/certification/user/' + userId,
      {
        headers
      });
  }

  deleteCertification(crt: Certificate) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.delete(this._baseurl + 'api/certification/' + crt.id, {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', 'Bearer ' + authToken)
    });
  }
}
