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
  public newNote: TimesheetNote = new TimesheetNote('', new Date());
  public selectedTimesheet: Timesheet;
  public selectedTsRow: number;
  public selectedUserRow: number;
  public selectedDate: string = null;
  public selectedTimesheetUser: string;
  public filterusername: string;
  public errors: string;
  public filterOnSubmittedTimesheets: boolean = false;
  public editable = false;

  public loading = true;
  public activeTab: string = "New";

  constructor(public signalRService: SignalRService, http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService, private _timesheetService: TimesheetService, private _certificationService: CertificateService) {
    this.loading = false;
    $('[data-toggle="tooltip"]').tooltip();
    //Retrieve Default list of tui Timesheets For display 
    this._timesheetService.getTimesheets().subscribe(result => {
      this.timesheets = result;
      this.newTabClicked();
      if (this.timesheets.length > 0) {
        this.setTimesheetsByState();
        this.selectedTsRow = 0;
      }
    }, error => this.errors = error);
  }

  ngOnInit(): void {
    this.signalRService.startConnection();
    this.signalRService.addTimesheetSubmittedListener();
  }

  newTabClicked() {
    this.activeTab = "New";
    this.setTimesheetsByState();
  }

  submittedTabClicked() {
    this.activeTab = "Submitted";
    this.setTimesheetsByState();
  }

  approvedTabClicked() {
    this.activeTab = "Approved";
    this.setTimesheetsByState();
  }

  rejectedTabClicked() {
    this.activeTab = "Rejected";
    this.setTimesheetsByState();
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

  removeTimesheetEntry(ts) {
      this._timesheetService.deleteTimesheetEntry(ts).subscribe(
        res => {
          $("#myEditTimesheetModal").modal('hide');

          this.removeFromTimesheetEntries(this.selectedTimesheet.timesheetEntries, ts);

        }, error => this.errors = error);
  }

  addTimesheetNote() {
    $("#myNewNoteModal").modal('show');
    this.timesheetToAddNoteTo = this.selectedTimesheet;
  }

  deleteNote(note: TimesheetNote) {
    this._timesheetService.deleteNote(note).subscribe(
      res => {
        //Remove note from array list
        this.removeFromArrayList(this.selectedTimesheet.timesheetNotes, note);
      }, error => this.errors = error);
  }

  removeFromTimesheetEntries(array: TimesheetEntry[], entry: TimesheetEntry) {
    for (let item of array) {
      if (item.id == entry.id) {
        array.splice(array.indexOf(item), 1);
        break;
      }
    }
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

        var notetosubmit = new TimesheetNote(this.newNote.details, new Date());
        notetosubmit.id = this.newNote.id;

        //Error with undefined timesheetnotes here.
        if (this.timesheetToAddNoteTo.timesheetNotes) {
          this.timesheetToAddNoteTo.timesheetNotes.push(notetosubmit);
        }
        else {
          this.timesheetToAddNoteTo.timesheetNotes = new Array();
          this.timesheetToAddNoteTo.timesheetNotes.push(notetosubmit);
        }
        
        this.newNote = new TimesheetNote('', new Date());

        $("#myNewNoteModal").modal('hide');

      }, error => this.errors = error);
  }

  retrieveNotesToDisplay(timesheet: Timesheet) {
    return timesheet.timesheetNotes
  }

  setTimesheetsByState() {
    this.filteredTimesheets = [];
    for (let item of this.timesheets) {
      if (item.status.toUpperCase() == this.activeTab.toUpperCase()) {
        this.filteredTimesheets.push(item);
      }
    }
    this.selectedTimesheet = null;
    if (this.filteredTimesheets) {
      this.selectedTimesheet = this.filteredTimesheets[0];
    }
  }

  rejectTimesheet() {
    this.selectedTimesheet.status = 'Rejected';
    this._timesheetService.updateTimesheet(this.selectedTimesheet).subscribe(
      res => {
        this.setTimesheetsByState();
        console.log(res);
      },
      error => this.errors = error);
  }

  approveTimesheet() {
    this.selectedTimesheet.status = 'Approved';
    this._timesheetService.updateTimesheet(this.selectedTimesheet).subscribe(
      res => {
        this.setTimesheetsByState();
        console.log(res);
      },
      error => this.errors = error);
  }

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

  calculateTotalDuration(): string{
    let totalDuration: number = 0;

    let ts = this.selectedTimesheet;

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

  timesheeExceedsWeeklyLimit() {
    let totalDuration: number = 0;

    let ts = this.selectedTimesheet;

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

  getTimesheetEntries() {
    if (this.selectedTimesheet) {
      const sorter = {
        "mon": 1,
        "tue": 2,
        "wed": 3,
        "thurs": 4,
        "fri": 5,
        "sat": 6,
        "sun": 7
      }

      let ts = this.selectedTimesheet

      //Order timesheet entries
      ts.timesheetEntries.sort(function sortByDay(a, b) {
        let day1 = a.day.toLowerCase();
        let day2 = b.day.toLowerCase();
        return sorter[day1] - sorter[day2];
      });

      //Return the sorted timesheet entries
      return ts.timesheetEntries;
    }
  }

  displayEntriesForSelectedTimesheet(ts) {
    this.selectedTimesheet = ts;
  }

  canApproveTimesheet() {
    if (this.selectedTimesheet) {
      return this.selectedTimesheet.status.toUpperCase() == 'SUBMITTED';
    }
  }
  canRejectTimesheet() {
    if (this.selectedTimesheet) {
      return this.selectedTimesheet.status.toUpperCase() == 'SUBMITTED';
    }
  }

  //getTimesheetOwner(index) {
  //  let ts = this.retrieveTimesheetsByState()[index];
  //  return ts.username;
  //}

  //getTimesheetStatus(index) {
  //  let ts = this.retrieveTimesheetsByState()[index];
  //  return ts.status;
  //}
}
