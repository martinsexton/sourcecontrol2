
import {map, catchError, retry} from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ApplicationUser } from '../../applicationuser';
import { UserRegistration } from '../models/user.registration.interface';
import 'rxjs/Rx';

import { Observable ,  BehaviorSubject } from 'rxjs';
import { LoginResponse } from '../models/loginresponse.interface';
import { IdentityRole } from '../../identityrole';
import { HttpServiceBase } from './httpservicebase';
import { EmailNotification } from '../../emailnotification';
import { PasswordReset } from '../../passwordreset';
import { Timesheet } from '../../timesheet';

@Injectable()
export class MsUserService extends HttpServiceBase{
  private loggedIn = false;

  constructor(_httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    super(_httpClient, baseUrl);
  }

  register(email: string, password: string, firstname :string, surname:string, role:string, phone:string) {
    let body = JSON.stringify({ email, password, firstname, surname, role, phone });


    return this._httpClient.post(this._baseurl + 'api/account', body,
      {
        headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
      }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  resetPassword(details: PasswordReset) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.put(this._baseurl + 'api/user/reset', details,
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  login(firstname, surname, password): Observable<LoginResponse>{

    return this._httpClient.post<LoginResponse>(this._baseurl + 'api/auth/login', JSON.stringify({ firstname, surname, password }),
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
      }).pipe(
      map(res => res),
      map(res => {
        if (!res.error) {
          this.loggedIn = true;
        }
        return res;
      }),
      retry(5),
      catchError(this.handleError),);
  }

  getUsers(inactiveUsers:boolean, page:number, pageSize:number): Observable<ApplicationUser[]>{
    const httpOptions = {
      headers: { 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + localStorage.getItem('auth_token') },
      params: { 'tenant': localStorage.getItem('tenant') }
    };

    return this._httpClient.get<ApplicationUser[]>(this._baseurl + 'api/user/' + inactiveUsers + '/' + page + '/' + pageSize,
      httpOptions).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  getUsersBasedOnFilter(inactiveUsers:boolean, filter:string, page: number, pageSize: number): Observable<ApplicationUser[]> {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<ApplicationUser[]>(this._baseurl + 'api/user/' + inactiveUsers + '/' + filter + '/' + page + '/' + pageSize,
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
        retry(5),
        catchError(this.handleError),);
  }

  updateUser(user: ApplicationUser) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.put(this._baseurl + 'api/user', user,
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  addEmailNotification(id: string, not: EmailNotification) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.put(this._baseurl + 'api/user/' + id + '/notifications', not,
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  logout() {
    localStorage.removeItem('auth_token');
    this.loggedIn = false;
  }

  isAdministrator() {
    return localStorage.getItem('role') == 'Administrator';
  }

  isSupervisor() {
    return localStorage.getItem('role') == 'Supervisor';
  }

  isLoggedIn() {
    return this.loggedIn;
  }

}
