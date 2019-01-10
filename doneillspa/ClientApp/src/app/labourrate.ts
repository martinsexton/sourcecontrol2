export class LabourRate {
  constructor(
    public effectiveFrom: Date,
    public effectiveTo: Date,
    public role: string,
    public ratePerHour: number
  ) { }
}
