import { Injectable } from '@angular/core';
import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ApplicationUser } from '../../applicationuser';
import { UserRegistration } from '../models/user.registration.interface';

import { Observable } from 'rxjs/Rx';
import { BehaviorSubject } from 'rxjs/Rx';
import { RequestOptions } from '@angular/http';
import { LoginResponse } from '../models/loginresponse.interface';
import { Certificate } from '../../certificate';

@Injectable()
export class MsUserService {
  _baseurl: String;
  private loggedIn = false;

  constructor(private _httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this._baseurl = baseUrl;
  }

  register(email: string, password: string, firstname :string, surname:string, role:string): Observable<UserRegistration>{
    let body = JSON.stringify({ email, password, firstname, surname, role });


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
      });
  }

  addCertificate(id:string, cert : Certificate) {
    return this._httpClient.put(this._baseurl + 'api/user/'+id, cert,
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
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

  protected handleError(error: any) {
    var applicationError = error.headers.get('Application-Error');

    // either applicationError in header or model error in body 
    if (applicationError) {
      return Observable.throw(applicationError);
    }
    var modelStateErrors: string = '';
    var serverError = error;

    if (!serverError.type) {
      for (var key in serverError) {
        if (key == "message") {
          modelStateErrors += serverError[key] + '\n';
          break;
        }
      }
    }
    modelStateErrors = modelStateErrors = '' ? null : modelStateErrors;
    return Observable.throw(modelStateErrors || 'Server error');
  }

}
