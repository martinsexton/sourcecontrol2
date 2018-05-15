import { ApplicationUser } from "./applicationuser";

export class Certificate {
  constructor(
    public userId: string,
    public id: number,
    public createdDate: Date,
    public expiry: Date,
    public description: string,
    public user: ApplicationUser
  ) { }
}
