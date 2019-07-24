import { Project } from './project';

export class Client {
  public projects: Array<Project> = new Array();

  constructor(
    public id: number,
    public name: string
  ) { }

}
