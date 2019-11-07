import { Injectable } from '@angular/core';
import { Component, Inject } from '@angular/core';
import * as signalR from "@aspnet/signalr";
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { HttpServiceBase } from './httpservicebase';

@Injectable()
export class SignalRService extends HttpServiceBase {
  private hubConnection: signalR.HubConnection
  public _timsheetsubmittedmessage: string;

  constructor(_httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    super(_httpClient, baseUrl);
  }

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this._baseurl + '/signalr')
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err))
  }

  public addTimesheetSubmittedListener = () => {
    this.hubConnection.on('timesheetsubmitted', (data) => {
      this._timsheetsubmittedmessage = data;
      console.log(data);
    });
  }

  public clearMessages() {
    this._timsheetsubmittedmessage = null;
  }
}
