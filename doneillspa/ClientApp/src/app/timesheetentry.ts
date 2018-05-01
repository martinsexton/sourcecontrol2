export class TimesheetEntry {

  constructor(
    public project: string,
    public day:string,
    public startTime: string,
    public endTime: string,
    public equipment:string
  ) { }

  duration(): number {
    return 1;
  }
}
