import { Client } from './client';

export class Project{

  constructor(
    public id:number,
    public client: string,
    public name: string,
    public code: string,
    public details: string,
    public isActive: boolean,
    public startDate:Date
  ) { }

}
