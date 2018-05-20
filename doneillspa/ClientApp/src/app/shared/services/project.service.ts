import { Injectable } from '@angular/core';
import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Project } from '../../project';
import { Certificate } from '../../certificate';
import { Observable } from 'rxjs/Observable';
import { ProjectEffortDto } from '../../projecteffortdto';
import { ProjectCostDto } from '../../projectcost.dto';

@Injectable()
export class ProjectService {
  _projects: Project[];
  _baseurl: String;

  constructor(private _httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this._baseurl = baseUrl;
  }

  getProjects() {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<Project[]>(this._baseurl + 'api/project',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      })
      .catch(this.handleError);
  }

  getProjectEffort() {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<ProjectEffortDto[]>(this._baseurl + 'api/projecteffort',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      })
      .catch(this.handleError);
  }

  getProjectCost() {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<ProjectCostDto[]>(this._baseurl + 'api/projectcost',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      })
      .catch(this.handleError);
  }

  saveProject(project: Project) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.post(this._baseurl + 'api/project', project, {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', 'Bearer ' + authToken)
    }).catch(this.handleError);;
  }

  getUserName(id) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<string>(this._baseurl + 'api/user/' + id,
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      });
  }

  //getCertifications(userId : string) {
  //  let authToken = localStorage.getItem('auth_token');
  //  let headers = new HttpHeaders();
  //  headers = headers.append('Content-Type', 'application/json');
  //  headers = headers.append('Authorization', 'Bearer ' + authToken);

  //  return this._httpClient.get<Certificate[]>(this._baseurl + 'api/certification/user/' + userId,
  //    {
  //      headers
  //    });
  //}

  //deleteCertification(crt: Certificate) {
  //  let authToken = localStorage.getItem('auth_token');

  //  return this._httpClient.delete(this._baseurl + 'api/certification/' + crt.id, {
  //    headers: new HttpHeaders()
  //      .set('Content-Type', 'application/json')
  //      .set('Authorization', 'Bearer ' + authToken)
  //  });
  //}

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
