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
import * as moment from 'moment';

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

  public timesheetsForCurrentPage: Timesheet[];
  public timesheetsCurrentPage: number = 1;
  public pageLimit: number = 10;

  constructor(public signalRService: SignalRService, http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService, private _timesheetService: TimesheetService, private _certificationService: CertificateService) {
    this.loading = false;
    $('[data-toggle="tooltip"]').tooltip();
    //Retrieve Default list of tui Timesheets For display 
    this._timesheetService.getTimesheets().subscribe(result => {
      this.timesheets = result;
      //this.newTabClicked();
      this.submittedTabClicked();
      if (this.timesheets.length > 0) {
        this.setTimesheetsByState();
        this.selectedTsRow = 0;
      }
    }, error => this.errors = error);
  }

  onTimesheetSelected(ts: Timesheet) {
    this.selectedTimesheet = ts;
  }

  ngOnInit(): void {
    this.signalRService.startConnection();
    this.signalRService.addTimesheetSubmittedListener();
  }

  determinePageCount() {
    var pageCount = 1;
    if (this.filteredTimesheets) {
      var numberOfTimesheets = this.filteredTimesheets.length;

      if (numberOfTimesheets > 0) {
        var totalPages_pre = Math.floor((numberOfTimesheets / this.pageLimit));
        pageCount = (numberOfTimesheets % this.pageLimit) == 0 ? totalPages_pre : totalPages_pre + 1
      }
      return pageCount;
    }
  }

  setupTimehseetsForCurrentPage() {
    var startingIndex = 0;
    var index = 0;

    this.selectedTimesheet = null;

    //reset client current page array
    this.timesheetsForCurrentPage = [];

    if (this.timesheetsCurrentPage > 1) {
      startingIndex = (this.timesheetsCurrentPage - 1) * this.pageLimit;
    }

    for (let ts of this.filteredTimesheets) {
      if (index >= startingIndex && index < (startingIndex + this.pageLimit)) {
        this.timesheetsForCurrentPage.push(ts);

        //Setup selected client as the firt client on current page.
        if (this.selectedTimesheet == null) {
          this.selectedTimesheet = ts;
        }
      }
      index = index + 1;
    }
  }

  previousPage() {
    this.timesheetsCurrentPage = this.timesheetsCurrentPage - 1;
    this.setupTimehseetsForCurrentPage();
  }

  nextPage() {
    this.timesheetsCurrentPage = this.timesheetsCurrentPage + 1;
    this.setupTimehseetsForCurrentPage();
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

  archievedTabClicked() {
    this.activeTab = "Archieved";
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
    if (this.activeTab == "Archieved") {
      var monthAgo = moment().subtract(1, "months");

      for (let item of this.timesheets) {
        if (item.status.toUpperCase() == "APPROVED") {
          //if older than x weeks let them all appear under archieved tabs
          var weekStarting = moment(item.weekStarting);

          if (moment(weekStarting).isBefore(monthAgo)) {
            this.filteredTimesheets.push(item);
          }
        }
      }
    }
    else if (this.activeTab == "Approved") {
      var monthAgo = moment().subtract(1, "months");

      for (let item of this.timesheets) {
        if (item.status.toUpperCase() == "APPROVED") {
          //if older than x weeks let them all appear under archieved tabs
          var weekStarting = moment(item.weekStarting);

          if (moment(weekStarting).isAfter(monthAgo)) {
            this.filteredTimesheets.push(item);
          }
        }
      }
    }
    else {
      for (let item of this.timesheets) {
        if (item.status.toUpperCase() == this.activeTab.toUpperCase()) {
          this.filteredTimesheets.push(item);
        }
      }
    }

    this.selectedTimesheet = null;
    if (this.filteredTimesheets) {
      this.selectedTimesheet = this.filteredTimesheets[0];
    }
    //Reset page count and refresh.
    this.timesheetsCurrentPage = 1;
    this.setupTimehseetsForCurrentPage();
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

  canApproveTimesheet() {
    if (this.selectedTimesheet) {
      return this.selectedTimesheet.status.toUpperCase() == 'SUBMITTED';
    }
  }
  canRejectTimesheet() {
    if (this.selectedTimesheet) {
      return this.selectedTimesheet.status.toUpperCase() == 'SUBMITTED' || this.selectedTimesheet.status.toUpperCase() == 'APPROVED';
    }
  }
}
