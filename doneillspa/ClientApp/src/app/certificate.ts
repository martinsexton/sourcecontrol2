import { ApplicationUser } from "./applicationuser";

export class Certificate {
  constructor(
    public id: number,
    public createdDate: Date,
    public expiry: Date,
    public description: string,
  ) { }
}
