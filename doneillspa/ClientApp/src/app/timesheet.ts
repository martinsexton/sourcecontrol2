import { TimesheetEntry } from "./timesheetentry";

export class Timesheet {
  public timesheetEntries: Array<TimesheetEntry> = new Array();

  constructor(
    public id:number,
    public owner: string,
    public weekStarting: Date
  ) { }
}
