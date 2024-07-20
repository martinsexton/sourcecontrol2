import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Timesheet } from '../timesheet';
import { TimesheetEntry } from '../timesheetentry';
import { TimesheetCode } from '../timesheetcode';
import { Project } from '../project';
import { NonChargeableTime } from '../nonchargeabletime';
import * as moment from 'moment';

import {
  ProjectService
} from '../shared/services/project.service';

import {
  TimesheetService
} from '../shared/services/timesheet.service';
import { ApplicationUser } from '../applicationuser';
import { MsUserService } from '../shared/services/msuser.service';

declare var $: any;

@Component({
  selector: 'timesheets2',
  templateUrl: './timesheets2.component.html',
  styleUrls: ['./timesheets2.component.css']
})

export class Timesheet2Component {
  selectedMoment: moment.Moment = moment();
  public monday: DayOfWeek;
  public tues: DayOfWeek;
  public wed: DayOfWeek;
  public thurs: DayOfWeek;
  public fri: DayOfWeek;
  public sat: DayOfWeek;
  public sun: DayOfWeek;
  public loading = false;
  public monthToDisplay: string = "";
  activeTimeSheet: Timesheet;
  public newEntry: TimesheetEntry;
  public selectedUser: ApplicationUser = null;
  public timesheets: Timesheet[];
  public timesheetExists = false;
  public errors: string;
  public selectedDay: string = "Mon";
  public activeTab: string = "mon";
  displayAddTimesheet = false;
  displayNotes = false;
  public userMessage: string;
  public selectedProjectName: string = '';

  public selectedDayEntries: Array<TimesheetEntry> = new Array();
  public monEntries: Array<TimesheetEntry> = new Array();
  public tueEntries: Array<TimesheetEntry> = new Array();
  public wedEntries: Array<TimesheetEntry> = new Array();
  public thursEntries: Array<TimesheetEntry> = new Array();
  public friEntries: Array<TimesheetEntry> = new Array();
  public satEntries: Array<TimesheetEntry> = new Array();
  public sunEntries: Array<TimesheetEntry> = new Array();
  public daysOfWeek: Array<string> = new Array();
  public timesheetEntryToEdit: TimesheetEntry;
  public timesheetEntryToView: TimesheetEntry;
  public timesheetCodes: TimesheetCode[];
  public activeProjects: Project[];
  public projects: Project[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService, private _timesheetService: TimesheetService, private _msuserService: MsUserService) {
    //Setup the user to user in retrieving and saving timesheets
    this.setDefaultUser();

    //Populate available days of the week for entering timesheets
    this.populateDaysOfWeek();

    //setup dates for each day of the week
    this.refreshCalendarDates();

    //Get the start of this working week
    var startOfWeek = this.getStartOfWeek();

    //Setting up default timesheet and timesheet entries
    this.activeTimeSheet = new Timesheet(0, this.selectedUser.firstName + this.selectedUser.surname, this.selectedUser.id, this.selectedUser.role, startOfWeek, null, 'New');
    this.newEntry = new TimesheetEntry("", "", "", "", "", "", true);

    //Retrieve timesheets for given date
    this.retrieveTimeSheetsForDate();

    this.loading = false;
  }
  onKey(event: any) { // without type info
    this.setupListOfProjectsByCode(event.target.value);
  }

  projectNameForDisplay(proj: TimesheetCode): string {
    if (proj.description.length > 40) {
      return proj.description.slice(0, 40) + '...';
    }
    else {
      return proj.description;
    }
  }

  refreshTimesheetCodes() {
    this.timesheetCodes = [];

    if (this.activeProjects) {
      for (let p of this.activeProjects) {
        let code = new TimesheetCode(p.code, p.name, true);
        this.timesheetCodes.push(code);
      }
    }
  }

  setupListOfProjectsByCode(code: string) {
    if (code.length > 0) {
      this._projectService.getActiveProjectsByCode(code).subscribe(result => {
        this.projects = result;
        if (this.projects) {
          this.setUpActiveProjects();
        }
      }, error => console.error(error));
    }
    else {
      //Clear list of active projects and refresh timesheet codes
      this.activeProjects = [];
      this.refreshTimesheetCodes();
    }

  }

  setUpActiveProjects() {
    var max = 10;
    var index = 1;

    this.activeProjects = [];
    for (let item of this.projects) {
      if (item.isActive == true) {
        if (index == 10) {
          break;
        }
        this.activeProjects.push(item);
        index = index + 1;
      }
    }

    this.refreshTimesheetCodes();

  }

  showEditTimesheet(entry: TimesheetEntry) {
    this.timesheetEntryToEdit = new TimesheetEntry("", "", "", "", "", "", true);

    this.timesheetEntryToEdit.code = entry.code;
    this.timesheetEntryToEdit.day = entry.day;
    this.timesheetEntryToEdit.details = entry.details;
    this.timesheetEntryToEdit.endTime = entry.endTime;
    this.timesheetEntryToEdit.id = entry.id;
    this.timesheetEntryToEdit.startTime = entry.startTime;
    this.timesheetEntryToEdit.userName = entry.userName;

    $("#myEditTimesheetModal").modal('show');
  }

  selectProject(proj: TimesheetCode) {
    this.selectedProjectName = proj.description;
    this.newEntry.code = proj.code;
    $("#dropdown_coins").dropdown('toggle');
  }

  selectProjectOnEdit(proj: TimesheetCode) {
    this.selectedProjectName = proj.description;
    this.timesheetEntryToEdit.code = proj.code;
    $("#dropdown_coins_edit").dropdown('toggle');
  }

  updateTimesheetEntry() {
    $("#myEditTimesheetModal").modal('hide');
    this._timesheetService.updateTimesheetEntry(this.timesheetEntryToEdit).subscribe(
      res => {
        //Clear list of timesheet codes
        this.timesheetCodes = [];
        var startOfWeek = this.getStartOfWeek();
        //Retrieve timesheets for given date
        this.retrieveTimeSheetsForDate();

      }, error => this.errors = error);
  }

  submitTimesheet() {
    this.activeTimeSheet.status = "Submitted";
    this._timesheetService.updateTimesheet(this.activeTimeSheet).subscribe(
      res => {
        console.log(res);
      },
      error => this.errors = error);
  }

  removeTimesheetEntry(ts) {
    //We can only delete timesheet entries while the timesheet is in a status of new.
    if (this.activeTimeSheet.status == 'New' || this.activeTimeSheet.status == 'Rejected') {
      this._timesheetService.deleteTimesheetEntry(ts).subscribe(
        res => {
          $("#myEditTimesheetModal").modal('hide');

          if (this.selectedDay == "Mon") {
            this.removeFromArrayList(this.monEntries, ts);
          }
          else if (this.selectedDay == "Tue") {
            this.removeFromArrayList(this.tueEntries, ts);
          }
          else if (this.selectedDay == "Wed") {
            this.removeFromArrayList(this.wedEntries, ts);
          }
          else if (this.selectedDay == "Thurs") {
            this.removeFromArrayList(this.thursEntries, ts);
          }
          else if (this.selectedDay == "Fri") {
            this.removeFromArrayList(this.friEntries, ts);
          }
          else if (this.selectedDay == "Sat") {
            this.removeFromArrayList(this.satEntries, ts);
          }
          else {
            this.removeFromArrayList(this.sunEntries, ts);
          }
        }, error => this.errors = error);
    }
  }

  toggleDisplayAddTimesheet() {
    this.displayAddTimesheet = !this.displayAddTimesheet;
    if (this.displayAddTimesheet) {
      $("#myNewTimesheetModal").modal('show');
    } else {
      $("#myNewTimesheetModal").modal('hide');
    }
  }

  toggleDisplayNotes() {
    this.displayNotes = !this.displayNotes;
    if (this.displayNotes) {
      $("#myViewNotesModal").modal('show');
    } else {
      $("#myViewNotesModal").modal('hide');
    }
  }

  showViewTimesheet(entry: TimesheetEntry) {
    this.timesheetEntryToView = entry;
    $("#myViewTimesheetModal").modal('show');
  }

  addTimesheetEntry() {
    let entry: TimesheetEntry = new TimesheetEntry(this.newEntry.code, this.selectedDay, this.newEntry.startTime, this.newEntry.endTime, this.newEntry.details, '', true);
    if (this.timesheetExists) {
      this.persistTimesheetEntry(entry);
    }
    else {
      //Automatically save timesheet and persist entry if it has not been saved already
      this.saveTimesheetAndEntry(entry);
    }
  }

  persistTimesheetEntry(entry: TimesheetEntry) {
    this._timesheetService.addTimesheetEntry(this.activeTimeSheet.id, entry).subscribe(
      res => {
        //Clear list of timesheet codes
        this.timesheetCodes = [];

        //Update the entry with the primary key that has come back from server
        entry.id = res as number;
        this.activeTimeSheet.timesheetEntries.push(entry);

        this.pushTimesheetToCalendarDays(entry);
        this.toggleDisplayAddTimesheet();


        //Clear times. Leave Projects as it might be of benefit.
        this.newEntry.startTime = null;
        this.newEntry.endTime = null;
        this.newEntry.details = null;

      }, error => {
        this.errors = error;
        this.userMessage = "Failed to save entry. Please check for duplicates."

        //Clear times details on error, as this might have been caused by overlap
        this.newEntry.startTime = null;
        this.newEntry.endTime = null;

        $('.toast').toast('show');
      });
  }

  saveTimesheetAndEntry(entry: TimesheetEntry) {
    //Populate timesheet object with entries
    this.populateTimesheet(this.monEntries);
    this.populateTimesheet(this.tueEntries);
    this.populateTimesheet(this.wedEntries);
    this.populateTimesheet(this.thursEntries);
    this.populateTimesheet(this.friEntries);
    this.populateTimesheet(this.satEntries);
    this.populateTimesheet(this.sunEntries);

    this._timesheetService.saveTimesheet(this.activeTimeSheet).subscribe(
      res => {
        //Clear list of timesheet codes
        this.timesheetCodes = [];

        console.log(res);
        this.timesheetExists = true;
        this.activeTimeSheet.id = res as number;
        this.timesheets.push(this.activeTimeSheet);

        this.persistTimesheetEntry(entry);
        $("#myNewTimesheetModal").modal('hide');
      },
      error => this.errors = error);
  }

  populateTimesheet(entries: TimesheetEntry[]) {
    for (let item of entries) {
      this.activeTimeSheet.timesheetEntries.push(item);
    }
  }

  pushTimesheetToCalendarDays(entry: TimesheetEntry) {
    if (this.selectedDay == "Mon") {
      this.monEntries.push(entry);
    }
    else if (this.selectedDay == "Tue") {
      this.tueEntries.push(entry);
    }
    else if (this.selectedDay == "Wed") {
      this.wedEntries.push(entry);
    }
    else if (this.selectedDay == "Thurs") {
      this.thursEntries.push(entry);
    }
    else if (this.selectedDay == "Fri") {
      this.friEntries.push(entry);
    }
    else if (this.selectedDay == "Sat") {
      this.satEntries.push(entry);
    }
    else {
      this.sunEntries.push(entry);
    }
  }

  removeFromArrayList(array: TimesheetEntry[], ts: TimesheetEntry) {
    for (let item of array) {
      if (item.code == ts.code && item.startTime == ts.startTime && item.endTime == ts.endTime) {
        array.splice(array.indexOf(item), 1);
        break;
      }
    }
  }

  calculateTotalDuration(): string {
    let totalDuration: number = 0;

    //We will need to separate timesheets into the differnt days and add
    //totals for each day and if >= 5 hours substract 30 minutes for lunch breaks
    let mondayMins: number = 0;
    let tueMins: number = 0;
    let wedMins: number = 0;
    let thursMins: number = 0;
    let friMins: number = 0;
    let satMins: number = 0;
    let sunMins: number = 0;

    for (let item of this.monEntries) {
      mondayMins += this.deriveElapsedTimeInMins(item.startTime, item.endTime);
    }

    for (let item of this.tueEntries) {
      tueMins += this.deriveElapsedTimeInMins(item.startTime, item.endTime);;
    }

    for (let item of this.wedEntries) {
      wedMins += this.deriveElapsedTimeInMins(item.startTime, item.endTime);;
    }

    for (let item of this.thursEntries) {
      thursMins += this.deriveElapsedTimeInMins(item.startTime, item.endTime);;
    }

    for (let item of this.friEntries) {
      friMins += this.deriveElapsedTimeInMins(item.startTime, item.endTime);;
    }

    for (let item of this.satEntries) {
      satMins += this.deriveElapsedTimeInMins(item.startTime, item.endTime);;
    }

    for (let item of this.sunEntries) {
      sunMins += this.deriveElapsedTimeInMins(item.startTime, item.endTime);;
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

  calculateTotalDurationForEntry(item: TimesheetEntry): string {
    let totalDuration: number = this.deriveElapsedTimeInMins(item.startTime, item.endTime);

    var hours = Math.floor(totalDuration / 60);
    var minutes = totalDuration % 60;

    return hours + ' hrs ' + minutes + ' mins';
  }

  deriveElapsedTimeInMins(startTime: string, endTime: string): number {
    var start = new Date("2018-01-01 " + startTime);
    var end = new Date("2018-01-01 " + endTime);

    var elapsedTimeInSec = (end.getTime() - start.getTime()) / 1000;
    var elapsedTimeInMins = elapsedTimeInSec / 60;

    return elapsedTimeInMins;
  }

  populateDaysOfWeek() {
    this.daysOfWeek.push("Mon");
    this.daysOfWeek.push("Tue");
    this.daysOfWeek.push("Wed");
    this.daysOfWeek.push("Thurs");
    this.daysOfWeek.push("Fri");
    this.daysOfWeek.push("Sat");
    this.daysOfWeek.push("Sun");
  }

  mondaySelected() {
    this.activeTab = "mon";
    this.setActiveDay(0);
  }

  tuesdaySelected() {
    this.activeTab = "tue";
    this.setActiveDay(1);
  }

  wednesdaySelected() {
    this.activeTab = "wed";
    this.setActiveDay(2);
  }

  thursdaySelected() {
    this.activeTab = "thurs";
    this.setActiveDay(3);
  }

  fridaySelected() {
    this.activeTab = "fri";
    this.setActiveDay(4);
  }

  saturdaySelected() {
    this.activeTab = "sat";
    this.setActiveDay(5);
  }

  sundaySelected() {
    this.activeTab = "sun";
    this.setActiveDay(6);
  }

  setActiveDay(index) {
    this.selectedDay = this.daysOfWeek[index];
    this.newEntry.day = this.selectedDay;
  }

  setDefaultUser() {
    this.selectedUser = new ApplicationUser(localStorage.getItem('client_id'), localStorage.getItem('firstname'), localStorage.getItem('surname'), "", "", localStorage.getItem('role'), true);
  }

  retrieveTimesheetsForDay() {
    if (this.selectedDay == "Mon") {
      this.monEntries.sort(function sortByStartTime(a, b) {
        let day1 = parseInt(a.startTime.split(':')[0]);
        let day2 = parseInt(b.startTime.split(':')[0]);
        return day1 - day2;
      });
      return this.monEntries;
    }
    else if (this.selectedDay == "Tue") {
      this.tueEntries.sort(function sortByStartTime(a, b) {
        let day1 = parseInt(a.startTime.split(':')[0]);
        let day2 = parseInt(b.startTime.split(':')[0]);
        return day1 - day2;
      });
      return this.tueEntries;
    }
    else if (this.selectedDay == "Wed") {
      this.wedEntries.sort(function sortByStartTime(a, b) {
        let day1 = parseInt(a.startTime.split(':')[0]);
        let day2 = parseInt(b.startTime.split(':')[0]);
        return day1 - day2;
      });
      return this.wedEntries;
    }
    else if (this.selectedDay == "Thurs") {
      this.thursEntries.sort(function sortByStartTime(a, b) {
        let day1 = parseInt(a.startTime.split(':')[0]);
        let day2 = parseInt(b.startTime.split(':')[0]);
        return day1 - day2;
      });
      return this.thursEntries;
    }
    else if (this.selectedDay == "Fri") {
      this.friEntries.sort(function sortByStartTime(a, b) {
        let day1 = parseInt(a.startTime.split(':')[0]);
        let day2 = parseInt(b.startTime.split(':')[0]);
        return day1 - day2;
      });
      return this.friEntries;
    }
    else if (this.selectedDay == "Sat") {
      this.satEntries.sort(function sortByStartTime(a, b) {
        let day1 = parseInt(a.startTime.split(':')[0]);
        let day2 = parseInt(b.startTime.split(':')[0]);
        return day1 - day2;
      });
      return this.satEntries;
    }
    else {
      this.sunEntries.sort(function sortByStartTime(a, b) {
        let day1 = parseInt(a.startTime.split(':')[0]);
        let day2 = parseInt(b.startTime.split(':')[0]);
        return day1 - day2;
      });
      return this.sunEntries;
    }
  }

  populateEntriesForDaysOfWeek(array: Timesheet[], ws: Date) {
    //Clear Arrays before loading
    this.monEntries.length = 0;
    this.tueEntries.length = 0;
    this.wedEntries.length = 0;
    this.thursEntries.length = 0;
    this.friEntries.length = 0;
    this.satEntries.length = 0;
    this.sunEntries.length = 0;

    //reset flag that determines if timesheet exists or not for the given date
    this.timesheetExists = false;
    this.activeTimeSheet.id = 0;
    this.activeTimeSheet.status = "New";
    this.activeTimeSheet.weekStarting = ws;
    //Clear the timesheet entries in the existing in memory timesheet
    this.activeTimeSheet.timesheetEntries.length = 0;
    this.activeTimeSheet.timesheetNotes.length = 0;

    for (let ts of array) {
      this.activeTimeSheet.weekStarting = ts.weekStarting;
      this.activeTimeSheet.id = ts.id;
      this.activeTimeSheet.status = ts.status;
      this.activeTimeSheet.timesheetNotes = ts.timesheetNotes;
      this.timesheetExists = true;

      for (let item of ts.timesheetEntries) {
        if (item.day == "Mon") {
          this.monEntries.push(item);
        }
        else if (item.day == "Tue") {
          this.tueEntries.push(item);
        }
        else if (item.day == "Wed") {
          this.wedEntries.push(item);
        }
        else if (item.day == "Thurs") {
          this.thursEntries.push(item);
        }
        else if (item.day == "Fri") {
          this.friEntries.push(item);
        }
        else if (item.day == "Sat") {
          this.satEntries.push(item);
        }
        else {
          this.sunEntries.push(item);
        }
      }
      break;
    }
  }

  retrieveTimeSheetsForDate() {
    let startOfWeek = this.getStartOfWeek();
    this._timesheetService.getTimesheetForUser(startOfWeek.getFullYear(), (startOfWeek.getMonth() + 1), startOfWeek.getDate(), this.selectedUser.firstName + this.selectedUser.surname).subscribe(result => {
      this.timesheets = result;
      this.populateEntriesForDaysOfWeek(this.timesheets, startOfWeek);
    }, error => {
      this.errors = error
    });
  }

  getStartOfWeek(): Date {
    var baseDate = this.selectedMoment.toDate();
    let day = baseDate.getDay();
    var startOfWeek = new Date(this.selectedMoment.toDate());
    startOfWeek.setDate(baseDate.getDate() - (day - 1))
    return startOfWeek;
  }

  nextWeek() {
    this.selectedMoment.add(7, 'days');
    this.refreshCalendarDates();
    this.retrieveTimeSheetsForDate();
  }

  previousWeek() {
    this.selectedMoment.add(-7, 'days');
    this.refreshCalendarDates();
    this.retrieveTimeSheetsForDate();
    
  }

  refreshCalendarDates() {
    var baseDate = this.selectedMoment.toDate();
    let day = baseDate.getDay();

    var monday = new Date(this.selectedMoment.toDate());
    monday.setDate(baseDate.getDate() - (day - 1))
    this.monday = new DayOfWeek("Mon", monday);

    var tuesday = new Date(this.selectedMoment.toDate());
    tuesday.setDate(baseDate.getDate() - (day - 2));
    this.tues = new DayOfWeek("Tue", tuesday);

    var wednesday = new Date(this.selectedMoment.toDate());
    wednesday.setDate(baseDate.getDate() - (day - 3));
    this.wed = new DayOfWeek("Wed", wednesday);

    var thursday = new Date(this.selectedMoment.toDate());
    thursday.setDate(baseDate.getDate() - (day - 4));
    this.thurs = new DayOfWeek("Thurs", thursday);

    var friday = new Date(this.selectedMoment.toDate());
    friday.setDate(baseDate.getDate() - (day - 5));
    this.fri = new DayOfWeek("Fri", friday);

    var saturday = new Date(this.selectedMoment.toDate());
    saturday.setDate(baseDate.getDate() - (day - 6));
    this.sat = new DayOfWeek("Sat", saturday);

    var sunday = new Date(this.selectedMoment.toDate());
    sunday.setDate(baseDate.getDate() - (day - 7));
    this.sun = new DayOfWeek("Sun", sunday);
  }

  getMondayDate() : string {
    if (this.monday) {
      return this.monday.date.getDate().toString();
    }
    else {
      return "";
    }
  }

  getHoursForTab() : string{
    if (this.activeTab === 'mon') {
      return this.getHoursForMon();
    }
    else if (this.activeTab === 'tue') {
      return this.getHoursForTue();
    }
    else if (this.activeTab === 'wed') {
      return this.getHoursForWed();
    }
    else if (this.activeTab === 'thurs') {
      return this.getHoursForThurs();
    }
    else if (this.activeTab === 'fri') {
      return this.getHoursForFri();
    }
    else if (this.activeTab === 'sat') {
      return this.getHoursForSat();
    }
    else if (this.activeTab === 'sun') {
      return this.getHoursForSun();
    }
    else {
      return "";
    }
  }

  getHoursForMon(): string {
    return this.getHoursForDay(this.monEntries);
  }


  getHoursForTue(): string {
    return this.getHoursForDay(this.tueEntries);
  }

  getHoursForWed(): string {
    return this.getHoursForDay(this.wedEntries);
  }

  getHoursForThurs(): string {
    return this.getHoursForDay(this.thursEntries);
  }

  getHoursForFri(): string {
    return this.getHoursForDay(this.friEntries);
  }

  getHoursForSat(): string {
    return this.getHoursForDay(this.satEntries);
  }

  getHoursForSun(): string {
    return this.getHoursForDay(this.sunEntries);
  }

  getHoursForDay(entries :TimesheetEntry[]): string {
    let totalMins: number = 0;
    for (let item of entries) {
      totalMins += this.deriveElapsedTimeInMins(item.startTime, item.endTime);
    }
    if (totalMins > 0) {
      var hours = Math.floor(totalMins / 60);
      var minutes = totalMins % 60;

      return hours + '.' + minutes;
    }
    else {
      return "";
    }
  }

  getHoursForTimesheetEntry(entry: TimesheetEntry): string {
    let totalMins: number = this.deriveElapsedTimeInMins(entry.startTime, entry.endTime);

    if (totalMins > 0) {
      var hours = Math.floor(totalMins / 60);
      var minutes = totalMins % 60;

      let result: string = hours + '.' + minutes;

      return result;
      //return result.replace(".0", "");
    }
    else {
      return "";
    }
  }

  getTuesdayDate(): string {
    if (this.tues) {
      return this.tues.date.getDate().toString();
    }
    else {
      return "";
    }
  }

  getWednesdayDate(): string {
    if (this.wed) {
      return this.wed.date.getDate().toString();
    }
    else {
      return "";
    }
  }

  getThursdayDate(): string {
    if (this.thurs) {
      return this.thurs.date.getDate().toString();
    }
    else {
      return "";
    }
  }

  getFridayDate(): string {
    if (this.fri) {
      return this.fri.date.getDate().toString();
    }
    else {
      return "";
    }
  }

  getSaturdayDate(): string {
    if (this.sat) {
      return this.sat.date.getDate().toString();
    }
    else {
      return "";
    }
  }

  getSundayDate(): string {
    if (this.sun) {
      return this.sun.date.getDate().toString();
    }
    else {
      return "";
    }
  }

  getMonthToDisplay(): string {
    let month: string = "";

    if (!this.loading) {
      switch (this.getStartOfWeek().getMonth()) {
        case 0:
          month = "January";
          break;
        case 1:
          month = "February";
          break;
        case 2:
          month = "March";
          break;
        case 3:
          month = "April";
          break;
        case 4:
          month = "May";
          break;
        case 5:
          month = "June";
          break;
        case 6:
          month = "July";
          break;
        case 7:
          month = "August";
          break;
        case 8:
          month = "September";
          break;
        case 9:
          month = "October";
          break;
        case 10:
          month = "November";
          break;
        case 11:
          month = "December";
          break;
        default:
          break;
      }
    }
    
    return month;
  }
}

export class DayOfWeek {
  constructor(
    public nameOfDay: string,
    public date: Date
  ) { }
} 
