import { Certificate } from './certificate';

export class ApplicationUser {
  public certifications: Array<Certificate> = new Array();

  constructor(
    public id: string,
    public firstName: string,
    public surname: string,
    public email: string,
    public phoneNumber: string,
    public role: string
  ) { }
}
