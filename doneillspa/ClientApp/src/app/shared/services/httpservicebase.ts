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
