import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Timesheet } from '../timesheet';
import { TimesheetEntry } from '../timesheetentry';
import { TimesheetNote } from '../timesheetnote';
import { Project } from '../project';
import { SignalRService } from '../shared/services/signalrservice';

import {
  ProjectService
} from '../shared/services/project.service';

import {
  TimesheetService
} from '../shared/services/timesheet.service';
import { CertificateService } from '../shared/services/certificate.service';

declare var $: any;

@Component({
  selector: 'dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})

export class DashboardComponent implements OnInit{
  public timesheets: Timesheet[];
  public timesheetToAddNoteTo: Timesheet;
  public filteredTimesheets: Timesheet[];
  public users: string[];
  public newNote: TimesheetNote = new TimesheetNote('');
  public selectedTimesheet: Timesheet;
  public selectedTsRow: number;
  public selectedUserRow: number;
  public selectedDate: string = null;
  public selectedTimesheetUser: string;
  public filterusername: string;
  public errors: string;
  public filterOnSubmittedTimesheets: boolean = false;

  public loading = true;

  constructor(public signalRService: SignalRService, http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService, private _timesheetService: TimesheetService, private _certificationService: CertificateService) {
    this.loading = false;
    $('[data-toggle="tooltip"]').tooltip();
    //Retrieve Default list of tui Timesheets For display 
    this._timesheetService.getTimesheets().subscribe(result => {
      this.timesheets = result;
      if (this.timesheets.length > 0) {
        this.selectedTimesheet = this.timesheets[0];
        this.selectedTsRow = 0;
      }
    }, error => this.errors = error);
  }

  ngOnInit(): void {
    this.signalRService.startConnection();
    this.signalRService.addCertificateCreatedListener();
  }

  clearSignalRMessages(){
    this.signalRService.clearMessages();
  }

  toggleTimesheetView() {
    this.filterOnSubmittedTimesheets = !this.filterOnSubmittedTimesheets;
  }

  nameForButton() {
    if (this.filterOnSubmittedTimesheets) {
      return "All Timesheets";
    }
    else {
      return "Submitted Timesheets Only";
    }
  }

  retrieveTimesheetsForDisplay() {
    if (this.filterOnSubmittedTimesheets) {
      return this.retrieveSubmittedTimesheets();
    }
    else {
      return this.timesheets;
    }
  }

  addTimesheetNote(timesheet:Timesheet) {
    $("#myNewNoteModal").modal('show');
    this.timesheetToAddNoteTo = timesheet;
  }

  deleteNote(note: TimesheetNote, ts : Timesheet) {
    this._timesheetService.deleteNote(note).subscribe(
      res => {
        //Remove note from array list
        this.removeFromArrayList(ts.timesheetNotes, note);
      }, error => this.errors = error);
  }

  removeFromArrayList(array: TimesheetNote[], note: TimesheetNote) {
    for (let item of array) {
      if (item.id == note.id) {
        array.splice(array.indexOf(item), 1);
        break;
      }
    }
  }

  saveTimesheetNote() {
    this._timesheetService.addTimesheetNote(this.timesheetToAddNoteTo.id, this.newNote).subscribe(
      res => {
        //Update the entry with the primary key that has come back from server
        this.newNote.id = res as number;

        var notetosubmit = new TimesheetNote(this.newNote.details);
        notetosubmit.id = this.newNote.id;

        //Error with undefined timesheetnotes here.
        if (this.timesheetToAddNoteTo.timesheetNotes) {
          this.timesheetToAddNoteTo.timesheetNotes.push(notetosubmit);
        }
        else {
          this.timesheetToAddNoteTo.timesheetNotes = new Array();
          this.timesheetToAddNoteTo.timesheetNotes.push(notetosubmit);
        }
        
        this.newNote = new TimesheetNote('');

        $("#myNewNoteModal").modal('hide');

      }, error => this.errors = error);
  }

  retrieveNotesToDisplay(timesheet: Timesheet) {
    return timesheet.timesheetNotes
  }

  retrieveSubmittedTimesheets() {
    this.filteredTimesheets = [];
    for (let item of this.timesheets) {
      if (item.status.toUpperCase() == 'SUBMITTED') {
        this.filteredTimesheets.push(item);
      }
    }
    if (this.filteredTimesheets) {
      return this.filteredTimesheets;
    }
    else {
      return this.timesheets;
    }
  }

  rejectTimesheet(ts: Timesheet) {
    ts.status = 'Rejected';
    this._timesheetService.updateTimesheet(ts).subscribe(
      res => {
        console.log(res);
      },
      error => this.errors = error);
  }

  approveTimesheet(ts: Timesheet) {
    ts.status = 'Approved';
    this._timesheetService.updateTimesheet(ts).subscribe(
      res => {
        console.log(res);
      },
      error => this.errors = error);
  }

  //retrieveTimesheetsForDisplay() {
  //  if (this.filteredTimesheets) {
  //    return this.filteredTimesheets;
  //  }
  //  else {
  //    return this.timesheets;
  //  }
  //}

  setSelectedDate() {
    let newDate = new Date(this.selectedDate);

    var startOfWeek = new Date();
    startOfWeek.setDate(newDate.getDate() - (newDate.getDay() - 1));
    startOfWeek.setMonth(newDate.getMonth());
    startOfWeek.setFullYear(newDate.getFullYear());

    //Retrieve Timesheets For display
    this._timesheetService.getTimesheet(startOfWeek.getFullYear(), (startOfWeek.getMonth() + 1), startOfWeek.getDate()).subscribe(result => {
      this.timesheets = result;
      //Clear filters
      this.filteredTimesheets = null;
      if (this.timesheets.length > 0) {
        //this.refreshListOfUsers();
        this.selectedTimesheet = this.timesheets[0];
        this.selectedTsRow = 0;
      } else {
        //Clear the selected timesheet if no result found.
        this.selectedTimesheet = null;
        this.selectedTsRow = 0;
      }
    }, error => this.errors = error);
  }

  calculateTotalDuration(index) : string{
    let totalDuration: number = 0;

    let ts = this.retrieveTimesheetsForDisplay()[index];

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
    if (satMins >= (5 * 60)) {
      satMins = satMins - 30;
    }
    if (sunMins >= (5 * 60)) {
      sunMins = sunMins - 30;
    }
    totalDuration = mondayMins + tueMins + wedMins + thursMins + friMins + satMins + sunMins;

    var hours = Math.floor(totalDuration / 60);
    var minutes = totalDuration % 60;

    return hours + ':' + minutes;
  }

  timesheeExceedsWeeklyLimit(index) {
    let totalDuration: number = 0;

    let ts = this.retrieveTimesheetsForDisplay()[index];

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

  calculateDuration(entry: TimesheetEntry): string {
    var start = new Date("2018-01-01 " + entry.startTime);
    var end = new Date("2018-01-01 " + entry.endTime);

    var elapsedTimeInSec = (end.getTime() - start.getTime()) / 1000;
    var elapsedTimeInMins = elapsedTimeInSec / 60;

    var hours = Math.floor(elapsedTimeInMins / 60);
    var minutes = elapsedTimeInMins % 60;

    return hours+ ':' + minutes;
  }

  displaySelectedTimesheetDetails(timesheet, index) {
    this.selectedTimesheet = timesheet;
    this.selectedTsRow = index
  }

  getTimesheetEntries(index) {
    let ts = this.retrieveTimesheetsForDisplay()[index];
    return ts.timesheetEntries;
  }

  canApproveTimesheet(ts: Timesheet) {
    return ts.status.toUpperCase() == 'SUBMITTED';
  }
  canRejectTimesheet(ts: Timesheet) {
    return ts.status.toUpperCase() == 'SUBMITTED';
  }

  getTimesheetOwner(index) {
    let ts = this.retrieveTimesheetsForDisplay()[index];
    return ts.username;
  }

  getTimesheetStatus(index) {
    let ts = this.retrieveTimesheetsForDisplay()[index];
    return ts.status;
  }
}
