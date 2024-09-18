import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Timesheet } from '../timesheet';
import { TimesheetEntry } from '../timesheetentry';
import { TimesheetNote } from '../timesheetnote';
import { Report } from '../Report';

import {
  ProjectService
} from '../shared/services/project.service';

import {
  TimesheetService
} from '../shared/services/timesheet.service';
import * as moment from 'moment';
import { FormBuilder, FormGroup, FormArray, FormControl } from '@angular/forms';
import { formatDate } from '@angular/common';
import { TimesheetReport } from '../TimesheetReport';

declare var $: any;

@Component({
  selector: 'dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})

export class DashboardComponent implements OnInit {
  public timesheets: Timesheet[];
  public timesheetToAddNoteTo: Timesheet;
  public filteredTimesheets: Timesheet[];
  public users: string[];
  public reports: Report[];
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
  public activeTab: string = "Submitted";

  public timesheetsForCurrentPage: Timesheet[];
  public timesheetsCurrentPage: number = 1;
  public pageLimit: number = 10;
  //vendorInformationForm: FormGroup;
  searchFromDate = new FormControl('');
  searchToDate = new FormControl('');
  reportDate = new FormControl('');
  selectedMoment: moment.Moment = moment();

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService, private _timesheetService: TimesheetService, private _formBuilder: FormBuilder) {
    $('[data-toggle="tooltip"]').tooltip();
    //Retrieve Default list of Timesheets For display  
    this._timesheetService.getSubmittedTimesheets().subscribe(result => {
      this.timesheets = result;
      this.submittedTabClicked();
      if (this.timesheets.length > 0) {
        this.setTimesheetsByState();
        this.selectedTsRow = 0;
      }
    }, error => this.errors = error);
  }

  ngOnInit() {
    this.initForm();
  }

  numberOfNotes() : number {
    if (this.selectedTimesheet.timesheetNotes) {
      return this.selectedTimesheet.timesheetNotes.length;
    }
    return 0;
  }

  initForm() {
    var toDate = this.selectedMoment.toDate();
    var fromDate = this.selectedMoment.subtract(3, 'months').toDate();
    
    this.searchFromDate.setValue(formatDate(fromDate, "yyyy-MM-dd", "en"));
    this.reportDate.setValue(formatDate(fromDate, "yyyy-MM-dd", "en"));
    this.searchToDate.setValue(formatDate(toDate, "yyyy-MM-dd", "en")); 
  }

  onTimesheetSelected(ts: Timesheet) {
    this.selectedTimesheet = ts;
  }

  filterArchievedTab() {
    this._timesheetService.getArchievedTimesheetsForRange(this.searchFromDate.value, this.searchToDate.value).subscribe(result => {
      this.timesheets = result;
      this.activeTab = "Archieved";
      this.setTimesheetsByState();
    }, error => this.errors = error);
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
    this.loading = true;

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
    this.loading = false;
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
    //Retrieve Default list of tui Timesheets For display 
    this._timesheetService.getSubmittedTimesheets().subscribe(result => {
      this.timesheets = result;
      this.activeTab = "Submitted";
      this.setTimesheetsByState();
    }, error => this.errors = error);
  }

  approvedTabClicked() {
    //Retrieve Default list of tui Timesheets For display 
    this._timesheetService.getApprovedTimesheets().subscribe(result => {
      this.timesheets = result;
      this.activeTab = "Approved";
      this.setTimesheetsByState();
    }, error => this.errors = error);

  }

  rejectedTabClicked() {
    this._timesheetService.getRejectedTimesheets().subscribe(result => {
      this.timesheets = result;
      this.activeTab = "Rejected";
      this.setTimesheetsByState();
    }, error => this.errors = error);
  }

  archievedTabClicked() {
    this._timesheetService.getArchievedTimesheetsForRange(this.searchFromDate.value, this.searchToDate.value).subscribe(result => {
      this.timesheets = result;
      this.activeTab = "Archieved";
      this.setTimesheetsByState();
    }, error => this.errors = error);
  }

  reportsTabClicked() {
    this._timesheetService.getTimesheetReports().subscribe(result => {
      this.activeTab = "Reports";
      this.reports = result;
    }, error => this.errors = error);
  }

  orderReport() {
    let report = new TimesheetReport(this.reportDate.value);

    this._timesheetService.orderReport(report).subscribe(result => {
    }, error => this.errors = error);
  }

  download(filename: string) {
    let thefile = new Blob();

    this._timesheetService.downloadFile(filename).subscribe(result => {
      let file = new File([result], filename, { type: 'application/octet-stream' });
      let url = window.URL.createObjectURL(file);
      var link = document.createElement('a');
      link.href = url;
      link.download = filename;
      link.click();

      //window.open(url);

      //thefile = new Blob([result as BlobPart], { type: "application/octet-stream" })
      //let url = window.URL.createObjectURL(thefile);
      //window.open(url);

    }, error => this.errors = error);
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
