
import {throwError as observableThrowError,  Observable } from 'rxjs';
import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class HttpServiceBase {
  _baseurl: String;

  constructor(public _httpClient: HttpClient, baseUrl: string) {
    this._baseurl = baseUrl;
  }
  protected handleError(error: any) {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      return observableThrowError('An error occurred:', error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      return observableThrowError(`Backend returned code ${error.status}`);
    }
  }
}
