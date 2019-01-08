export class LabourWeek {
  constructor(
    public week: Date,
    public administratorHours: number,
    public supervisorHours: number,
    public chargehandHours: number,
    public elecR1Hours: number,
    public elecR2Hours: number,
    public elecR3Hours: number,
    public tempHours: number,
    public firstYearApprenticeHours: number,
    public secondYearApprenticeHours: number,
    public thirdYearApprenticeHours: number,
    public fourthYearApprenticeHours: number
  ) { }
}
