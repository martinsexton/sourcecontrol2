export class TimesheetEntry {
  public id: number;
  constructor(
    public code: string,
    public day:string,
    public startTime: string,
    public endTime: string,
    public details:string
  ) { }
}
