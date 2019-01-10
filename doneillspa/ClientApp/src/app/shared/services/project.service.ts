import { Injectable } from '@angular/core';
import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Project } from '../../project';
import { Certificate } from '../../certificate';
import { Observable } from 'rxjs/Observable';
import { ProjectEffortDto } from '../../projecteffortdto';
import { HttpServiceBase } from './httpservicebase';
import { LabourRate } from '../../labourrate';

@Injectable()
export class ProjectService extends HttpServiceBase  {
  _projects: Project[];

  constructor(_httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    super(_httpClient, baseUrl); 
  }

  getLabourRates() {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<LabourRate[]>(this._baseurl + 'api/labourdetails/rates',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      })
      .catch(this.handleError);
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

  saveRate(rate: LabourRate) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.post(this._baseurl + 'api/labourdetails/rates', rate, {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', 'Bearer ' + authToken)
    }).catch(this.handleError);
  }

  saveProject(project: Project) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.post(this._baseurl + 'api/project', project, {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', 'Bearer ' + authToken)
    }).catch(this.handleError);
  }

  updateRate(rate: LabourRate) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.put(this._baseurl + 'api/labourdetails/rates', rate, {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', 'Bearer ' + authToken)
    }).catch(this.handleError);
  }
  updateProject(project: Project) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.put(this._baseurl + 'api/project', project, {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', 'Bearer ' + authToken)
    }).catch(this.handleError);
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

  deleteRate(rate: LabourRate) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.delete(this._baseurl + 'api/labourdetails/rates/' + rate.id, {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', 'Bearer ' + authToken)
    });
  }

  deleteProject(project: Project) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.delete(this._baseurl + 'api/project/' + project.id, {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', 'Bearer ' + authToken)
    });
  }
}
