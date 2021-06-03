import { Component, Inject, Input } from '@angular/core';
import { Project } from '../../project';
import { LabourWeek } from '../../labourweek';
import { TimesheetService } from '../../shared/services/timesheet.service';


@Component({
  selector: 'labour-details',
  templateUrl: './labourdetails.component.html'
})

export class LabourDetailsComponent {
  public errors: string;

  @Input() lb: LabourWeek[];

  constructor(private _timesheetService: TimesheetService) {}

  calculateTotalCostToDate() {
    let totalCostToDate: number = 0;

    for (let lw of this.lb) {
      totalCostToDate = totalCostToDate + lw.totalCost;
    }
    return totalCostToDate;
  }
}
