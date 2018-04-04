import { Injectable } from '@angular/core';
import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class ProjectService {
  _projects: Project[];
  _baseurl: String;

  constructor(private _httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this._baseurl = baseUrl;
  }

  getProjects(): Project[] {
    this._httpClient.get<Project[]>(this._baseurl + 'api/project').subscribe(result => {
      this._projects = result;
    }, error => console.error(error));

    return this._projects;
  } 
}

interface Project {
  name: string;
  id: number;
  isActive: boolean;
  details: string;
}
