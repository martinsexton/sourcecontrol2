import { Injectable } from '@angular/core';
import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { UserRegistration } from '../models/user.registration.interface';

import { Observable } from 'rxjs/Rx';
import { BehaviorSubject } from 'rxjs/Rx';
import { RequestOptions } from '@angular/http';

@Injectable()
export class MsUserService {
  _baseurl: String;

  constructor(private _httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this._baseurl = baseUrl;
  }

  register(email: string, password: string, firstName: string, lastName: string, username: string): Observable<UserRegistration>{
    let body = JSON.stringify({ email, password, firstName, lastName, username });


    return this._httpClient.post(this._baseurl + 'api/account', body,
      {
        headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
      })
      .catch(this.handleError);
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
        if (serverError[key])
          modelStateErrors += serverError[key] + '\n';

      }

    }


    modelStateErrors = modelStateErrors = '' ? null : modelStateErrors;
    return Observable.throw(modelStateErrors || 'Server error');

  }

}
