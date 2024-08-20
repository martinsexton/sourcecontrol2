
import {catchError, retry} from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Project } from '../../project';
import { Timesheet } from '../../timesheet';
import { TimesheetEntry } from '../../timesheetentry';
import { TimesheetNote } from '../../timesheetnote';
import { HttpServiceBase } from './httpservicebase';
import { Observable } from 'rxjs';
import { TimesheetReport } from '../../TimesheetReport';
import { Report } from '../../Report';

@Injectable()
export class TimesheetService extends HttpServiceBase{
  _projects: Project[];


  constructor(_httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    super(_httpClient, baseUrl); 
  }

  orderReport(report: TimesheetReport) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.post(this._baseurl + 'api/timesheet/report', report, {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', 'Bearer ' + authToken)
    }).pipe(
      retry(5),
      catchError(this.handleError),);
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

  getUserSubmittedTimesheets(userId: string) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<Timesheet[]>(this._baseurl + 'api/usersubmittedtimesheet',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken),
        params: new HttpParams()
          .set("userId", userId)
      }).pipe(
        retry(5),
        catchError(this.handleError));
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

  getUserApprovedTimesheets(userId: string) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<Timesheet[]>(this._baseurl + 'api/userapprovedtimesheet',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken),
        params: new HttpParams()
          .set("userId", userId)
      }).pipe(
        retry(5),
        catchError(this.handleError));
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

  getUserArchievedTimesheetsForRange(userId: string, fromData: string, toDate: string) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<Timesheet[]>(this._baseurl + 'api/userarchievedtimesheetforrange',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken),
        params: new HttpParams()
          .set("userId", userId)
          .set("fromDate", fromData)
          .set("toDate", toDate)
      }).pipe(
        retry(5),
        catchError(this.handleError));
  }

  getTimesheetReports() {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<Report[]>(this._baseurl + 'api/timesheetreports',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
      }).pipe(
        retry(5),
        catchError(this.handleError));
  }

  downloadFile(filename : string) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get(this._baseurl + 'api/timesheetreport/' + filename,
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken)
        , responseType: 'text' 
      },).pipe(
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

  getUserRejectedTimesheets(userId: string) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.get<Timesheet[]>(this._baseurl + 'api/userrejectedtimesheet',
      {
        headers: new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', 'Bearer ' + authToken),
        params: new HttpParams()
          .set("userId", userId)
      }).pipe(
        retry(5),
        catchError(this.handleError));
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
