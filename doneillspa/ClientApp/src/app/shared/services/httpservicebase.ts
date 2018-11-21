import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class HttpServiceBase {
  _baseurl: String;

  constructor(public _httpClient: HttpClient, baseUrl: string) {
    this._baseurl = baseUrl;
  }
  protected handleError(error: any) {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      return Observable.throw('An error occurred:', error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      return Observable.throw(`Backend returned code ${error.status}`);
    }
  }
}
