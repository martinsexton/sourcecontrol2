
import {catchError, retry} from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Project } from '../../project';
import { Client } from '../../client';
import { Observable } from 'rxjs';
import { ProjectCostDto } from '../../projectcostdto';
import { HttpServiceBase } from './httpservicebase';
import { NonChargeableTime } from '../../nonchargeabletime';
import { TimesheetEntry } from '../../timesheetentry';

@Injectable()
export class ProjectService extends HttpServiceBase  {
  _projects: Project[];

  constructor(_httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    super(_httpClient, baseUrl); 
  }

  getClientsForFilter(filter: string, activeClients: boolean, page: number, pageSize: number): Observable<Client[]> {
    const httpOptions = {
      headers: { 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + localStorage.getItem('auth_token') },
      params: { 'tenant': localStorage.getItem('tenant') }
    };

    return this._httpClient.get<Client[]>(this._baseurl + 'api/client' + '/' + filter + '/' + activeClients + '/' + page + '/' + pageSize,
      httpOptions).pipe(
        retry(5),
        catchError(this.handleError),);
  }

  getClients(activeClients:boolean, page: number, pageSize: number): Observable<Client[]>{
    const httpOptions = {
      headers: { 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + localStorage.getItem('auth_token') },
      params: { 'tenant': localStorage.getItem('tenant') }
    };

    return this._httpClient.get<Client[]>(this._baseurl + 'api/client' + '/' + activeClients + '/' + page + '/' + pageSize,
      httpOptions).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  getNonChargeableTime() {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<NonChargeableTime[]>(this._baseurl + 'api/nonchargeabletime',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  getActiveProjectsByCode(code: string) {
    const httpOptions = {
      headers: { 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + localStorage.getItem('auth_token') },
      params: { 'tenant': localStorage.getItem('tenant') }
    };

    return this._httpClient.get<Project[]>(this._baseurl + 'api/activeproject/' + code,
      httpOptions).pipe(
        retry(5),
        catchError(this.handleError),);
  }

  addProject(id: number, proj: Project) {
    const httpOptions = {
      headers: { 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + localStorage.getItem('auth_token') }
    };

    return this._httpClient.put(this._baseurl + 'api/client/' + id + '/projects', proj,
      httpOptions).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  saveClient(c: Client) {
    const httpOptions = {
      headers: { 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + localStorage.getItem('auth_token') }
    };

    return this._httpClient.post(this._baseurl + 'api/client', c,
      httpOptions).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  updateClient(c: Client) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.put(this._baseurl + 'api/client', c, {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', 'Bearer ' + authToken)
    }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  saveProject(project: Project) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.post(this._baseurl + 'api/project', project, {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', 'Bearer ' + authToken)
    }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  updateProject(project: Project) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.put(this._baseurl + 'api/project', project, {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', 'Bearer ' + authToken)
    }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  getUserName(id) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<string>(this._baseurl + 'api/user/' + id,
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
      retry(5));
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
