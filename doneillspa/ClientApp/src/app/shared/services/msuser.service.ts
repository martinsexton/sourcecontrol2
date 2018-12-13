import { Injectable } from '@angular/core';
import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ApplicationUser } from '../../applicationuser';
import { UserRegistration } from '../models/user.registration.interface';
import 'rxjs/Rx';

import { Observable } from 'rxjs/Rx';
import { BehaviorSubject } from 'rxjs/Rx';
import { RequestOptions } from '@angular/http';
import { LoginResponse } from '../models/loginresponse.interface';
import { Certificate } from '../../certificate';
import { IdentityRole } from '../../identityrole';
import { HttpServiceBase } from './httpservicebase';
import { EmailNotification } from '../../emailnotification';

@Injectable()
export class MsUserService extends HttpServiceBase{
  private loggedIn = false;

  constructor(_httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    super(_httpClient, baseUrl);
  }

  register(email: string, password: string, firstname :string, surname:string, role:string, phone:string): Observable<UserRegistration>{
    let body = JSON.stringify({ email, password, firstname, surname, role, phone });


    return this._httpClient.post(this._baseurl + 'api/account', body,
      {
        headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
      })
      .catch(this.handleError);
  }

  login(firstname, surname, password): Observable<LoginResponse>{

    return this._httpClient.post<LoginResponse>(this._baseurl + 'api/auth/login', JSON.stringify({ firstname, surname, password }),
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
      })
      .map(res => res)
      .map(res => {
        if (!res.error) {
          this.loggedIn = true;
        }
        return res;
      })
      .catch(this.handleError);
  }

  getUsers() {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<ApplicationUser[]>(this._baseurl + 'api/user/',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      })
      .catch(this.handleError);
  }

  getUser(name:string) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<ApplicationUser[]>(this._baseurl + 'api/user/name/'+name,
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      })
      .catch(this.handleError);
  }

  getUserRoles() {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<IdentityRole[]>(this._baseurl + 'api/user/roles',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      });
  }

  addCertificate(id: string, cert: Certificate) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.put(this._baseurl + 'api/user/'+id+'/certificates', cert,
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      })
      .catch(this.handleError);
  }

  addEmailNotification(id: string, not: EmailNotification) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.put(this._baseurl + 'api/user/' + id + '/notifications', not,
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      })
      .catch(this.handleError);
  }

  retrieveTimesheets(id : string) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get(this._baseurl + 'api/user/timesheets/' + id,
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      })
      .catch(this.handleError);
  }

  logout() {
    localStorage.removeItem('auth_token');
    this.loggedIn = false;
  }

  isAdministrator() {
    return localStorage.getItem('role') == 'Administrator';
  }

  isLoggedIn() {
    return this.loggedIn;
  }

}
