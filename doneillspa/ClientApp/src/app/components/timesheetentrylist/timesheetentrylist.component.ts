import { Component, Inject, Input } from '@angular/core';
import { Timesheet } from '../../timesheet';

import {
  TimesheetService
} from '../../shared/services/timesheet.service';
import { TimesheetEntry } from '../../timesheetentry';

@Component({
  selector: 'timesheet-entry-list',
  templateUrl: './timesheetentrylist.component.html'
})
export class TimeSheetEntryListComponent {
  @Input() owner: Timesheet;

  constructor(private _timesheetService: TimesheetService) { }

  calculateTotalPayableDuration() {
    let totalDuration: number = 0;

    let ts = this.owner;

    //We will need to separate timesheets into the differnt days and add
    //totals for each day and if >= 5 hours substract 30 minutes for lunch breaks
    let mondayMins: number = 0;
    let tueMins: number = 0;
    let wedMins: number = 0;
    let thursMins: number = 0;
    let friMins: number = 0;
    let satMins: number = 0;
    let sunMins: number = 0;

    for (let tse of ts.timesheetEntries) {
      //Exclude time recorded against Sick Days
      if (tse.code == 'NC4') {
        continue;
      }
      let day = tse.day;
      var start = new Date("2018-01-01 " + tse.startTime);
      var end = new Date("2018-01-01 " + tse.endTime);

      var elapsedTimeInSec = (end.getTime() - start.getTime()) / 1000;
      var elapsedTimeInMins = elapsedTimeInSec / 60;

      if (day == "Mon") {
        mondayMins += elapsedTimeInMins;
      }
      else if (day == "Tue") {
        tueMins += elapsedTimeInMins;
      }
      else if (day == "Wed") {
        wedMins += elapsedTimeInMins;
      }
      else if (day == "Thurs") {
        thursMins += elapsedTimeInMins;
      }
      else if (day == "Fri") {
        friMins += elapsedTimeInMins;
      }
      else if (day == "Sat") {
        satMins += elapsedTimeInMins;
      }
      else {
        sunMins += elapsedTimeInMins;
      }
    }

    //If worked >= 5 hours for a day subtract 30 mins.
    if (mondayMins >= (5 * 60)) {
      mondayMins = mondayMins - 30;
    }
    if (tueMins >= (5 * 60)) {
      tueMins = tueMins - 30;
    }
    if (wedMins >= (5 * 60)) {
      wedMins = wedMins - 30;
    }
    if (thursMins >= (5 * 60)) {
      thursMins = thursMins - 30;
    }
    if (friMins >= (5 * 60)) {
      friMins = friMins - 30;
    }
    if (satMins > (5 * 60)) {
      satMins = satMins - 30;
    }
    if (sunMins > (5 * 60)) {
      sunMins = sunMins - 30;
    }
    totalDuration = mondayMins + tueMins + wedMins + thursMins + friMins + satMins + sunMins;

    var hours = Math.floor(totalDuration / 60);
    var minutes = totalDuration % 60;

    return hours + ':' + minutes;
  }

  calculateTotalDuration(): string {
    let totalDuration: number = 0;

    let ts = this.owner;

    //We will need to separate timesheets into the differnt days and add
    //totals for each day and if >= 5 hours substract 30 minutes for lunch breaks
    let mondayMins: number = 0;
    let tueMins: number = 0;
    let wedMins: number = 0;
    let thursMins: number = 0;
    let friMins: number = 0;
    let satMins: number = 0;
    let sunMins: number = 0;

    for (let tse of ts.timesheetEntries) {
      let day = tse.day;
      var start = new Date("2018-01-01 " + tse.startTime);
      var end = new Date("2018-01-01 " + tse.endTime);

      var elapsedTimeInSec = (end.getTime() - start.getTime()) / 1000;
      var elapsedTimeInMins = elapsedTimeInSec / 60;

      if (day == "Mon") {
        mondayMins += elapsedTimeInMins;
      }
      else if (day == "Tue") {
        tueMins += elapsedTimeInMins;
      }
      else if (day == "Wed") {
        wedMins += elapsedTimeInMins;
      }
      else if (day == "Thurs") {
        thursMins += elapsedTimeInMins;
      }
      else if (day == "Fri") {
        friMins += elapsedTimeInMins;
      }
      else if (day == "Sat") {
        satMins += elapsedTimeInMins;
      }
      else {
        sunMins += elapsedTimeInMins;
      }
    }

    //If worked >= 5 hours for a day subtract 30 mins.
    if (mondayMins >= (5 * 60)) {
      mondayMins = mondayMins - 30;
    }
    if (tueMins >= (5 * 60)) {
      tueMins = tueMins - 30;
    }
    if (wedMins >= (5 * 60)) {
      wedMins = wedMins - 30;
    }
    if (thursMins >= (5 * 60)) {
      thursMins = thursMins - 30;
    }
    if (friMins >= (5 * 60)) {
      friMins = friMins - 30;
    }
    if (satMins > (5 * 60)) {
      satMins = satMins - 30;
    }
    if (sunMins > (5 * 60)) {
      sunMins = sunMins - 30;
    }
    totalDuration = mondayMins + tueMins + wedMins + thursMins + friMins + satMins + sunMins;

    var hours = Math.floor(totalDuration / 60);
    var minutes = totalDuration % 60;

    return hours + ':' + minutes;
  }

  getCodeToDisplay(tse: TimesheetEntry) {
    if (tse.code == 'NC1') {
      return "Annual Leave";
    }
    else if (tse.code == 'NC2') {
      return "Bank Holiday";
    }
    else if (tse.code == 'NC3') {
      return "DON.ELEC STAFF / MGMT MEETINGS";
    }
    else if (tse.code == 'NC4') {
      return "Sick Day";
    }
    else if (tse.code == 'NC5') {
      return "Medical Appointment";
    }
    else if (tse.code == 'NC6') {
      return "Unpaid Leave";
    }
    else if (tse.code == 'NC7') {
      return "Training";
    }
    else {
      return tse.code;
    }
  }

  getSortedTimesheetEntries() {
      const sorter = {
        "mon": 100,
        "tue": 200,
        "wed": 300,
        "thurs": 400,
        "fri": 500,
        "sat": 600,
        "sun": 700
      }
      let ts = this.owner;

      //Order timesheet entries by day
      ts.timesheetEntries.sort(function sortByDay(a, b) {
        let day1 = a.day.toLowerCase();
        let day2 = b.day.toLowerCase();

        var firstTime = new Date("2018-01-01 " + a.startTime);
        var secondTime = new Date("2018-01-01 " + b.startTime);

        return (sorter[day1] + firstTime.getHours()) - (sorter[day2] + secondTime.getHours());
      });

      //Return the sorted timesheet entries
      return ts.timesheetEntries;
  }

  removeTimesheetEntry(ts) {
    this._timesheetService.deleteTimesheetEntry(ts).subscribe(
      res => {
        this.removeFromTimesheetEntries(this.owner.timesheetEntries, ts);
      });
  }

  removeFromTimesheetEntries(array: TimesheetEntry[], entry: TimesheetEntry) {
    for (let item of array) {
      if (item.id == entry.id) {
        array.splice(array.indexOf(item), 1);
        break;
      }
    }
  }

  timesheeExceedsWeeklyLimit() {
    let totalDuration: number = 0;

    let ts = this.owner;

    for (let tse of ts.timesheetEntries) {
      var start = new Date("2018-01-01 " + tse.startTime);
      var end = new Date("2018-01-01 " + tse.endTime);

      var elapsedTimeInSec = (end.getTime() - start.getTime()) / 1000;
      var elapsedTimeInMins = elapsedTimeInSec / 60;
      totalDuration += elapsedTimeInMins;
    }
    //2250 mins = 37.5 hours.
    return totalDuration > 2250;
  }
}
