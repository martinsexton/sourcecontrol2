
import {map, catchError, retry} from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ApplicationUser } from '../../applicationuser';
import { UserRegistration } from '../models/user.registration.interface';
import 'rxjs/Rx';

import { Observable ,  BehaviorSubject } from 'rxjs';
import { RequestOptions } from '@angular/http';
import { LoginResponse } from '../models/loginresponse.interface';
import { Certificate } from '../../certificate';
import { IdentityRole } from '../../identityrole';
import { HttpServiceBase } from './httpservicebase';
import { EmailNotification } from '../../emailnotification';
import { HolidayRequest } from '../../holidayrequest';
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

  getUsersWithRole(role: string) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<ApplicationUser[]>(this._baseurl + 'api/user/role/' + role,
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  getUsers(): Observable<ApplicationUser[]>{
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<ApplicationUser[]>(this._baseurl + 'api/user/',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  getContractors() {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<ApplicationUser[]>(this._baseurl + 'api/contractor/',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  getUser(name:string) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<ApplicationUser[]>(this._baseurl + 'api/user/name/'+name,
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  getUserRoles() {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<IdentityRole[]>(this._baseurl + 'api/user/roles',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
      retry(5));
  }

  getHolidayRequests() {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<HolidayRequest[]>(this._baseurl + 'api/user/' + localStorage.getItem('client_id') + '/holidayrequests',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
      retry(5));
  }

  getHolidayRequestsForApproval() {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<HolidayRequest[]>(this._baseurl + 'api/supervisor/' + localStorage.getItem('client_id') + '/holidayrequests',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
      retry(5));
  }

  addHolidayRequest(id: string, request: HolidayRequest) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.put(this._baseurl + 'api/user/' + id + '/holidayrequests', request,
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

  addCertificate(id: string, cert: Certificate) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.put(this._baseurl + 'api/user/'+id+'/certificates', cert,
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

  retrieveTimesheets(id: string): Observable<Timesheet[]>{
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<Timesheet[]>(this._baseurl + 'api/user/' + id + '/timesheets',
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
