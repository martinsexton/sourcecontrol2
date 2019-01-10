export class LabourRate {
  constructor(
    public id: number,
    public effectiveFrom: Date,
    public effectiveTo: Date,
    public role: string,
    public ratePerHour: number
  ) { }
}
