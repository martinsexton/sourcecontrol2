import { TimesheetEntry } from "./timesheetentry";

export class Timesheet {
  public timesheetEntries: Array<TimesheetEntry> = new Array();

  constructor(
    public id: number,
    public username:string,
    public owner: string,
    public role:string,
    public weekStarting: Date,
    public status: string
  ) { }
}
