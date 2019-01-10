export class LabourWeek {
  constructor(
    public week: Date,
    public administratorMinutes: number,
    public administratorCost: number,
    public supervisorMinutes: number,
    public supervisorCost: number,
    public chargehandMinutes: number,
    public chargehandCost: number,
    public elecR1Minutes: number,
    public elecR1Cost: number,
    public elecR2Minutes: number,
    public elecR2Cost: number,
    public elecR3Minutes: number,
    public elecR3Cost: number,
    public tempMinutes: number,
    public tempCost: number,
    public firstYearMinutes: number,
    public firstYearCost: number,
    public secondYearApprenticeMinutes: number,
    public secondYearCost: number,
    public thirdYearApprenticeMinutes: number,
    public thirdYearCost: number,
    public fourthYearApprenticeMinutes: number,
    public fourthYearCost: number,
    public totalCost:number
  ) { }
}
