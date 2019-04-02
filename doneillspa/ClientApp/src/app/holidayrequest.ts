export class HolidayRequest {
  constructor(
    public id: number,
    public fromDate: Date,
    public requestedDate: Date,
    public days: number,
    public approverId: string,
    public status:string
  ) { }
}
