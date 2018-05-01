import { TimesheetEntry } from "./timesheetentry";

export class Timesheet {
  public timesheetEntries: Array<TimesheetEntry> = new Array();

  constructor(
    public owner: string,
    public weekStarting: Date
  ) { }
}
