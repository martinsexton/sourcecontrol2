import { Injectable } from '@angular/core';
import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Project } from '../../project';
import { Certificate } from '../../certificate';

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
      });
  }

  saveProject(project: Project) {
    let authToken = localStorage.getItem('auth_token');

    return this._httpClient.post(this._baseurl + 'api/project', project, {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', 'Bearer ' + authToken)
    });
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

  getCertifications(userId : string) {
    let authToken = localStorage.getItem('auth_token');
    let headers = new HttpHeaders();
    headers = headers.append('Content-Type', 'application/json');
    headers = headers.append('Authorization', 'Bearer ' + authToken);

    return this._httpClient.get<Certificate[]>(this._baseurl + 'api/certification/user/' + userId,
      {
        headers
      });
  }
}
