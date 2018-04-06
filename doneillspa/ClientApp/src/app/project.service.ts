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

  getProjects() {
    return this._httpClient.get<Project[]>(this._baseurl + 'api/project');
  }
}

interface Project {
  name: string;
  id: number;
  isActive: boolean;
  details: string;
}
