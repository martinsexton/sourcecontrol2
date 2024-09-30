
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
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<Client[]>(this._baseurl + 'api/client' + '/' + filter + '/' + activeClients + '/' + page + '/' + pageSize,
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
        retry(5),
        catchError(this.handleError),);
  }

  getClients(activeClients:boolean, page: number, pageSize: number): Observable<Client[]>{
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<Client[]>(this._baseurl + 'api/client' + '/' + activeClients + '/' + page + '/' + pageSize,
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
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

  getProjects() {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<Project[]>(this._baseurl + 'api/project',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  getProjectsForClient(clientId: number, page: number, pageSize: number) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<Project[]>(this._baseurl + 'api/project/client/' + clientId + '/' + page + '/' + pageSize,
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
        retry(5),
        catchError(this.handleError),);
  }

  getActiveProjects() {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<Project[]>(this._baseurl + 'api/activeproject',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  getActiveProjectsByCode(code: string) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<Project[]>(this._baseurl + 'api/activeproject/' + code,
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
        retry(5),
        catchError(this.handleError),);
  }

  getTimesheetEntriesForProjectAndWeek(code: string, week: string): Observable<TimesheetEntry[]>{
    console.log('Find timesheet entries for ' + code + ' and week: ' + week);
    let authToken = localStorage.getItem('auth_token');

    console.log('Get timesheet entries for week: ' + week);

    return this._httpClient.get<TimesheetEntry[]>(this._baseurl + 'api/labourdetails/project/timesheetentries/' + code + '/' + week,
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  addProject(id: number, proj: Project) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.put(this._baseurl + 'api/client/' + id + '/projects', proj,
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  saveClient(c: Client) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.post(this._baseurl + 'api/client', c, {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', 'Bearer ' + authToken)
    }).pipe(
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
