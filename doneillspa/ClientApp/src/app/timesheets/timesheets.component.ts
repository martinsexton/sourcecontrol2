import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Timesheet } from '../timesheet';
import { TimesheetEntry } from '../timesheetentry';
import { Project } from '../project';

import {
  ProjectService
} from '../project.service';

declare var $: any;

@Component({
  selector: 'timesheets',
  templateUrl: './timesheets.component.html'
})

export class TimesheetComponent {
  public timesheets: Timesheet[];
  public projects: Project[];

  //Default to Monday
  public selectedDay = "Mon";

  public monEntries: Array<TimesheetEntry> = new Array();
  public tueEntries: Array<TimesheetEntry> = new Array();
  public wedEntries: Array<TimesheetEntry> = new Array();
  public thursEntries: Array<TimesheetEntry> = new Array();
  public friEntries: Array<TimesheetEntry> = new Array();
  public satEntries: Array<TimesheetEntry> = new Array();

  newTimesheet: Timesheet = new Timesheet(localStorage.getItem('client_id'), new Date());
  public newEntry: TimesheetEntry = new TimesheetEntry("", "", "", "");

  currentDate: Date;

  monday: DayOfWeek;
  tues: DayOfWeek;
  wed: DayOfWeek;
  thurs: DayOfWeek;
  fri: DayOfWeek;
  sat: DayOfWeek;

  displayAddTimesheet = false;
  
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService) {
    this.currentDate = new Date();
    this.refreshCalendarTabs();

    this._projectService.getProjects().subscribe(result => {
      this.projects = result;
    }, error => console.error(error));

    this._projectService.getTimesheets().subscribe(result => {
      this.timesheets = result;
    }, error => console.error(error));
  }

  previousWeek() {
    //move back one week
    this.currentDate.setDate(this.currentDate.getDate() - 7);
    this.refreshCalendarTabs();
  }

  nextWeek() {
    //move forward one week
    this.currentDate.setDate(this.currentDate.getDate() + 7);
    this.refreshCalendarTabs();
  }

  refreshCalendarTabs() {
    let day = this.currentDate.getDay();

    var monday = new Date();
    monday.setDate(this.currentDate.getDate() - (day - 1))
    this.monday = new DayOfWeek("Mon", monday);

    var tuesday = new Date();
    tuesday.setDate(monday.getDate() + 1);
    this.tues = new DayOfWeek("Tue", tuesday);

    var wednesday = new Date();
    wednesday.setDate(tuesday.getDate() + 1);
    this.wed = new DayOfWeek("Wed", wednesday);

    var thursday = new Date();
    thursday.setDate(wednesday.getDate() + 1);
    this.thurs = new DayOfWeek("Thurs", thursday);

    var friday = new Date();
    friday.setDate(thursday.getDate() + 1);
    this.fri = new DayOfWeek("Fri", friday);

    var saturday = new Date();
    saturday.setDate(friday.getDate() + 1);
    this.sat = new DayOfWeek("Sat", saturday);
  }

  toggleDisplayAddTimesheet() {
    this.displayAddTimesheet = !this.displayAddTimesheet;
    if (this.displayAddTimesheet) {
      $("#myNewTimesheetModal").modal('show');
    } else {
      $("#myNewTimesheetModal").modal('hide');
    }
  }

  addTimesheetEntry() {
    if (this.selectedDay == "Mon") {
      this.monEntries.push(new TimesheetEntry(this.newEntry.project, this.newEntry.startTime, this.newEntry.endTime, this.newEntry.equipment));
    }
    else if (this.selectedDay == "Tue") {
      this.tueEntries.push(new TimesheetEntry(this.newEntry.project, this.newEntry.startTime, this.newEntry.endTime, this.newEntry.equipment));
    }
    else if (this.selectedDay == "Wed") {
      this.wedEntries.push(new TimesheetEntry(this.newEntry.project, this.newEntry.startTime, this.newEntry.endTime, this.newEntry.equipment));
    }
    else if (this.selectedDay == "Thurs") {
      this.thursEntries.push(new TimesheetEntry(this.newEntry.project, this.newEntry.startTime, this.newEntry.endTime, this.newEntry.equipment));
    }
    else if (this.selectedDay == "Fri") {
      this.friEntries.push(new TimesheetEntry(this.newEntry.project, this.newEntry.startTime, this.newEntry.endTime, this.newEntry.equipment));
    }
    else {
      this.satEntries.push(new TimesheetEntry(this.newEntry.project, this.newEntry.startTime, this.newEntry.endTime, this.newEntry.equipment));
    }

    $("#myNewTimesheetModal").modal('hide');
  }

  paintMonTab() {
    this.selectedDay = "Mon";
  }

  paintTueTab() {
    this.selectedDay = "Tue";
  }

  paintWedTab() {
    this.selectedDay = "Wed";
  }

  paintThursTab() {
    this.selectedDay = "Thurs";
  }

  paintFriTab() {
    this.selectedDay = "Fri";
  }

  paintSatTab() {
    this.selectedDay = "Sat";
  }

  retrieveTimesheetsForTab(): Array<TimesheetEntry> {
    if (this.selectedDay == "Mon") {
      return this.monEntries;
    }
    else if (this.selectedDay == "Tue") {
      return this.tueEntries;
    }
    else if (this.selectedDay == "Wed") {
      return this.wedEntries;
    }
    else if (this.selectedDay == "Thurs") {
      return this.thursEntries;
    }
    else if (this.selectedDay == "Fri") {
      return this.friEntries;
    }
    else {
      return this.satEntries;
    }
  }

  saveTimesheet() {
    this._projectService.saveTimesheet(this.newTimesheet).subscribe(
      res => {
        console.log(res);
        this.timesheets.push(this.newTimesheet);
        $("#myNewTimesheetModal").modal('hide');
      },
      (err: HttpErrorResponse) => {
        console.log(err.error);
        console.log(err.name);
        console.log(err.message);
        console.log(err.status);
      }
    );
  }
}

export class DayOfWeek {
  constructor(
    public nameOfDay: string,
    public date: Date
  ) { }
} 
