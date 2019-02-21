import { Injectable } from '@angular/core';
import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Project } from '../../project';
import { Timesheet } from '../../timesheet';
import { TimesheetEntry } from '../../timesheetentry';
import { HttpServiceBase } from './httpservicebase';
import { LabourRate } from '../../labourrate';
import { LabourWeek } from '../../labourweek';

@Injectable()
export class TimesheetService extends HttpServiceBase{
  _projects: Project[];


  constructor(_httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    super(_httpClient, baseUrl); 
  }

  saveTimesheet(timesheet: Timesheet) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.post(this._baseurl + 'api/timesheet', timesheet, {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', 'Bearer ' + authToken)
    })
      .retry(5)
      .catch(this.handleError);
  }

  addTimesheetEntry(timesheetId: number, entry: TimesheetEntry) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.put(this._baseurl + 'api/timesheet/'+timesheetId, entry, {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', 'Bearer ' + authToken)
    })
      .retry(5)
      .catch(this.handleError);
  }

  deleteTimesheetEntry(tse: TimesheetEntry) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.delete(this._baseurl + 'api/timesheetentry/' + tse.id, {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', 'Bearer ' + authToken)
    })
    .retry(5)
    .catch(this.handleError);
  }

  getTimesheets() {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<Timesheet[]>(this._baseurl + 'api/timesheet',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      })
      .retry(5)
      .catch(this.handleError);
  }

  getLabourWeekDetails() {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<Timesheet[]>(this._baseurl + 'api/labourdetails',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      })
      .catch(this.handleError);
  }

  downloadReport(weeks: LabourWeek[]) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.post(this._baseurl + 'api/labourdetails/report', weeks,
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      })
      .retry(5)
      .catch(this.handleError);
  }

  getLabourWeekDetailsForProject(project) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<Timesheet[]>(this._baseurl + 'api/labourdetails/project/' + project,
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      })
      .retry(5)
      .catch(this.handleError);
  }

  getLabourRates() {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<LabourRate[]>(this._baseurl + 'api/labourdetails/rates',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      })
      .retry(5)
      .catch(this.handleError);
  }

  getTimesheet(year: number, month: number, day: number) {
    let authToken = localStorage.getItem('auth_token');
    let headers = new HttpHeaders();
    headers = headers.append('Content-Type', 'application/json');
    headers = headers.append('Authorization', 'Bearer ' + authToken);

    return this._httpClient.get<Timesheet[]>(this._baseurl + 'api/timesheet/week/' + year + '/' + month + '/' + day,
      {
        headers
      })
      .retry(5)
      .catch(this.handleError);
  }

  getTimesheetForUser(user:string) {
    let authToken = localStorage.getItem('auth_token');
    let headers = new HttpHeaders();
    headers = headers.append('Content-Type', 'application/json');
    headers = headers.append('Authorization', 'Bearer ' + authToken);

    return this._httpClient.get<Timesheet[]>(this._baseurl + 'api/timesheet/name/' + user,
      {
        headers
      });
  }
}
