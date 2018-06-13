export class Project{

  constructor(
    public client: string,
    public name: string,
    public details: string,
    public isactive: boolean,
    public startDate:Date
  ) { }

}
