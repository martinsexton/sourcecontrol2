import { TimesheetEntry } from "./timesheetentry";
import { TimesheetNote } from "./timesheetnote";

export class Timesheet {
  public timesheetEntries: Array<TimesheetEntry> = new Array();
  public timesheetNotes: Array<TimesheetNote> = new Array();

  constructor(
    public id: number,
    public username:string,
    public owner: string,
    public role:string,
    public weekStarting: Date,
    public dateSubmitted: Date,
    public status: string
  ) { }
}
