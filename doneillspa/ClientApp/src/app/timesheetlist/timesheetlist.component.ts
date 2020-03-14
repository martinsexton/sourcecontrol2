import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Timesheet } from '../timesheet';

@Component({
  selector: 'timesheet-list',
  templateUrl: './timesheetlist.component.html',
  styleUrls: ['./timesheetlist.component.css']
})

export class TimeSheetListComponent {
  @Output() eventPublisher = new EventEmitter<Timesheet>();
  @Input() timesheets: Timesheet[];
  public selectedTimesheet: Timesheet;

  setSelectedTimesheet(ts) {
    this.selectedTimesheet = ts;
    this.eventPublisher.emit(ts);
  }
}
