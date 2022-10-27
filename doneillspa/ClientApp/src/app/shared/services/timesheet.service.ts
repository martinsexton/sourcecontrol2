
import {catchError, retry} from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Project } from '../../project';
import { Timesheet } from '../../timesheet';
import { TimesheetEntry } from '../../timesheetentry';
import { TimesheetNote } from '../../timesheetnote';
import { HttpServiceBase } from './httpservicebase';
import { LabourRate } from '../../labourrate';
import { LabourWeek } from '../../labourweek';
import { Observable } from 'rxjs';
import { ProjectAssignment } from '../../projectassignment';

@Injectable()
export class TimesheetService extends HttpServiceBase{
  _projects: Project[];


  constructor(_httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    super(_httpClient, baseUrl); 
  }

  updateTimesheet(timesheet: Timesheet) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.put(this._baseurl + 'api/timesheet', timesheet, {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', 'Bearer ' + authToken)
    }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  saveTimesheet(timesheet: Timesheet) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.post(this._baseurl + 'api/timesheet', timesheet, {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', 'Bearer ' + authToken)
    }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  deleteNote(note: TimesheetNote) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.delete(this._baseurl + 'api/note/' + note.id, {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', 'Bearer ' + authToken)
    }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  addTimesheetNote(timesheetId: number, note: TimesheetNote) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.put(this._baseurl + 'api/timesheet/' + timesheetId + '/note', note, {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', 'Bearer ' + authToken)
    }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  addTimesheetEntry(timesheetId: number, entry: TimesheetEntry) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.put(this._baseurl + 'api/timesheet/'+timesheetId + '/timesheetentry', entry, {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', 'Bearer ' + authToken)
    }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  updateTimesheetEntry(entry: TimesheetEntry) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.put(this._baseurl + 'api/timesheetentry',entry, {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', 'Bearer ' + authToken)
    }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  deleteTimesheetEntry(tse: TimesheetEntry) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.delete(this._baseurl + 'api/timesheetentry/' + tse.id, {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', 'Bearer ' + authToken)
    }).pipe(
    retry(5),
    catchError(this.handleError),);
  }

  getTimesheets() {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<Timesheet[]>(this._baseurl + 'api/timesheet',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  getSubmittedTimesheets() {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<Timesheet[]>(this._baseurl + 'api/submittedtimesheet',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  getApprovedTimesheets() {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<Timesheet[]>(this._baseurl + 'api/approvedtimesheet',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  getArchievedTimesheets() {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<Timesheet[]>(this._baseurl + 'api/archievedtimesheet',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  getArchievedTimesheetsForRange(fromData:string, toDate:string) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<Timesheet[]>(this._baseurl + 'api/archievedtimesheetforrange',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken),
        params: new HttpParams()
          .set("fromDate", fromData)
          .set("toDate", toDate)
      }).pipe(
        retry(5),
        catchError(this.handleError));

  }

  getRejectedTimesheets() {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<Timesheet[]>(this._baseurl + 'api/rejectedtimesheet',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  downloadReport(project: string) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get(this._baseurl + 'api/labourdetails/report/' + project,
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  downloadFullReport() {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get(this._baseurl + 'api/labourdetails/report',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  getLabourWeekDetailsForProject(project): Observable<LabourWeek[]>{
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<LabourWeek[]>(this._baseurl + 'api/labourdetails/project/' + project,
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  getLabourRates() {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<LabourRate[]>(this._baseurl + 'api/labourdetails/rates',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  getProjectAssignments(year: number, month: number, day: number) {
    let authToken = localStorage.getItem('auth_token');
    let headers = new HttpHeaders();
    headers = headers.append('Content-Type', 'application/json');
    headers = headers.append('Authorization', 'Bearer ' + authToken);

    return this._httpClient.get<ProjectAssignment[]>(this._baseurl + 'api/projectassignments/week/' + year + '/' + month + '/' + day,
      {
        headers
      }).pipe(
      retry(5),
      catchError(this.handleError),);
  }

  getTimesheetForUser(year: number, month: number, day: number, user:string) {
    let authToken = localStorage.getItem('auth_token');
    let headers = new HttpHeaders();
    headers = headers.append('Content-Type', 'application/json');
    headers = headers.append('Authorization', 'Bearer ' + authToken);

    return this._httpClient.get<Timesheet[]>(this._baseurl + 'api/timesheet/name/' + user + '/week/' + year + '/' + month + '/' + day,
      {
        headers
      });
  }
}
