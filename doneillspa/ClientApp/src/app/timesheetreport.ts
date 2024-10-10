export class TimesheetReport {

  constructor(
    public id: number,
    public reportDate: Date,
    public createdDate: Date,
    public fileReference: string,
    public status : string
  ) { }

}
