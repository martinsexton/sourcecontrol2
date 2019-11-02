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
  selector: 'timesheets',
  templateUrl: './timesheets.component.html',
  styleUrls: ['./timesheets.component.css']
})

export class TimesheetComponent {
  public timesheets: Timesheet[];
  public projects: Project[];
  public nonChargeableTime: NonChargeableTime[];
  public activeProjects: Project[];
  public timesheetExists = false;
  public loading = true;
  public selectedDate: string = null;
  public errors: string;

  public selectedUser: ApplicationUser = null;

  //Default to Monday
  public selectedDay: string = "Mon";

  public monEntries: Array<TimesheetEntry> = new Array();
  public tueEntries: Array<TimesheetEntry> = new Array();
  public wedEntries: Array<TimesheetEntry> = new Array();
  public thursEntries: Array<TimesheetEntry> = new Array();
  public friEntries: Array<TimesheetEntry> = new Array();
  public satEntries: Array<TimesheetEntry> = new Array();
  public sunEntries: Array<TimesheetEntry> = new Array();

  public daysOfWeek: Array<string> = new Array();

  activeTimeSheet: Timesheet;

  selectedMoment: moment.Moment = moment();

  public newEntry: TimesheetEntry;
  public timesheetEntryToEdit: TimesheetEntry;
  public contractors: ApplicationUser[];

  public monday: DayOfWeek;
  public tues: DayOfWeek;
  public wed: DayOfWeek;
  public thurs: DayOfWeek;
  public fri: DayOfWeek;
  public sat: DayOfWeek;
  public sun: DayOfWeek;

  displayAddTimesheet = false;

  //declare var $: any;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _projectService: ProjectService, private _timesheetService: TimesheetService, private _msuserService: MsUserService) {
    //Retrieve list of contractors
    this.retrieveContrators();

    //Setup the user to user in retrieving and saving timesheets
    this.setDefaultUser();

    //Populate available days of the week for entering timesheets
    this.populateDaysOfWeek();

    //Setup list of available projects
    this.setupListOfProjects();

    this.setupNonChargeableTime();

    //setup dates for each day of the week
    this.refreshCalendarDates();

    //Get the start of this working week
    var startOfWeek = this.getStartOfWeek();

    //Setting up default timesheet and timesheet entries
    this.activeTimeSheet = new Timesheet(0, this.selectedUser.firstName + this.selectedUser.surname, this.selectedUser.id, this.selectedUser.role, startOfWeek,'New');
    this.newEntry = new TimesheetEntry("", "", "", "", "");

    //Retrieve timesheets for given date
    this.retrieveTimeSheetsForDate(startOfWeek);
  }

  retrieveContrators() {
    this._msuserService.getContractors().subscribe(result => {
      this.contractors = result;
    })
  }

  retrieveListOfUsers() {
    let users: Array<ApplicationUser> = new Array();
    //Need to create an application user to represent the logged on user.
    let defaultUser: ApplicationUser = new ApplicationUser(localStorage.getItem('client_id'), localStorage.getItem('firstname'), localStorage.getItem('surname'), "", "", localStorage.getItem('role'), true);
    users.push(defaultUser);
    if (this.contractors) {
      for (let c of this.contractors) {
        users.push(c);
      }
    }
    return users;
  }

  setDefaultUser() {
    this.selectedUser = new ApplicationUser(localStorage.getItem('client_id'), localStorage.getItem('firstname'), localStorage.getItem('surname'), "", "", localStorage.getItem('role'), true);
  }

  setUser(user: ApplicationUser) {
    this.selectedUser = user;

    //Update active timesheet with details of switched user
    this.activeTimeSheet.owner = this.selectedUser.id;
    this.activeTimeSheet.role = this.selectedUser.role;
    this.activeTimeSheet.username = this.selectedUser.firstName + this.selectedUser.surname;

    //Get the start of this working week
    var startOfWeek = this.getStartOfWeek();
    this.retrieveTimeSheetsForDate(startOfWeek);
    this.LoadWeek();
  }

  ngOnInit() {
    $('[data-toggle="tooltip"]').tooltip(); 
  }

  retrieveTimeCodes() {
    let codes: Array<TimesheetCode> = new Array();


    if (this.activeProjects && this.nonChargeableTime) {
      for (let p of this.activeProjects) {
        let code = new TimesheetCode(p.code, p.name, true);
        codes.push(code);
      }
      for (let nc of this.nonChargeableTime) {
        let code = new TimesheetCode(nc.code, nc.description, false);
        codes.push(code);
      }    
    }
    return codes;
  }

  setUpActiveProjects() {
    this.activeProjects = [];
    for (let item of this.projects) {
      if (item.isActive == true) {
        this.activeProjects.push(item);
      }
    }

  }

  retrieveNotesToDisplay() {
    return this.activeTimeSheet.timesheetNotes
  }

  retrieveTimeSheetsForDate(startOfWeek: Date) {
    this._timesheetService.getTimesheetForUser(startOfWeek.getFullYear(), (startOfWeek.getMonth() + 1), startOfWeek.getDate(), this.selectedUser.firstName + this.selectedUser.surname).subscribe(result => {
      this.loading = false;
      this.timesheets = result;
      this.populateEntriesForDaysOfWeek(this.timesheets, startOfWeek);
    }, error => {
      this.errors = error
      this.loading = false;
    });
  }

  getStartOfWeek(): Date {
    var baseDate = this.selectedMoment.toDate();
    let day = baseDate.getDay();
    var startOfWeek = new Date(this.selectedMoment.toDate());
    startOfWeek.setDate(baseDate.getDate() - (day - 1))
    return startOfWeek;
  }

  setupNonChargeableTime() {
    this._projectService.getNonChargeableTime().subscribe(result => {
      this.nonChargeableTime = result;
    }, error => console.error(error)); 
  }

  setupListOfProjects() {
    this._projectService.getProjects().subscribe(result => {
      this.projects = result;
      if (this.projects) {
        this.setUpActiveProjects();
        if (this.activeProjects.length > 0) {
          this.newEntry.code = this.activeProjects[0].name;
        }
      }
    }, error => console.error(error));  
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

  nextWeek() {
    this.selectedMoment.add(7, 'days');
    this.LoadWeek();
  }

  previousWeek() {
    this.selectedMoment.add(-7, 'days');
    this.LoadWeek();
  }

  isSupervisor() {
    return this._msuserService.isLoggedIn() && this._msuserService.isSupervisor();
  }

  LoadWeek() {
    this.refreshCalendarDates();

    var startOfWeek = this.getStartOfWeek();

    this._timesheetService.getTimesheetForUser(startOfWeek.getFullYear(), (startOfWeek.getMonth() + 1), startOfWeek.getDate(), this.selectedUser.firstName + this.selectedUser.surname).subscribe(result => {
      this.loading = false;
      this.timesheets = result;
      this.populateEntriesForDaysOfWeek(this.timesheets, startOfWeek);
    }, error => this.errors = error);
  }

  setActiveDay(index) {
    this.selectedDay = this.daysOfWeek[index];;
  }

  retrieveTimesheetsForIndex(index) {
    let day = this.daysOfWeek[index];
    return this.retrieveTimesheetsForDay(day);
  }
  retrieveTimesheetsForDay(day:string) {
    if (day == "Mon") {
      return this.monEntries;
    }
    else if (day == "Tue") {
      return this.tueEntries;
    }
    else if (day == "Wed") {
      return this.wedEntries;
    }
    else if (day == "Thurs") {
      return this.thursEntries;
    }
    else if (day == "Fri") {
      return this.friEntries;
    }
    else if (day == "Sat") {
      return this.satEntries;
    }
    else {
      return this.sunEntries;
    }
  }

  populateEntriesForDaysOfWeek(array: Timesheet[], ws : Date) {
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

  retrieveDateForDay(day: string) : Date{
    if (day == "Mon") {
      return this.monday.date;
    }
    else if (day == "Tue") {
      return this.tues.date;
    }
    else if (day == "Wed") {
      return this.wed.date;
    }
    else if (day == "Thurs") {
      return this.thurs.date;
    }
    else if (day == "Fri") {
      return this.fri.date;
    }
    else if (day == "Sat") {
      return this.sat.date;
    }
    else {
      return this.sun.date;
    }
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

  toggleDisplayAddTimesheet() {
    this.displayAddTimesheet = !this.displayAddTimesheet;
    if (this.displayAddTimesheet) {
      $("#myNewTimesheetModal").modal('show');
    } else {
      $("#myNewTimesheetModal").modal('hide');
    }
  }

  showEditTimesheet(entry: TimesheetEntry) {
    this.timesheetEntryToEdit = entry;
    $("#myEditTimesheetModal").modal('show');
  }

  updateTimesheetEntry() {
    console.log('updating timesheetEntryToEdit')
    $("#myEditTimesheetModal").modal('hide');
    this._timesheetService.updateTimesheetEntry(this.timesheetEntryToEdit).subscribe(
      res => {
      }, error => this.errors = error);
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

  removeFromArrayList(array: TimesheetEntry[], ts: TimesheetEntry) {
    for (let item of array) {
      if (item.code == ts.code && item.startTime == ts.startTime && item.endTime == ts.endTime) {
        array.splice(array.indexOf(item), 1);
        break;
      }
    }
  }

  addTimesheetEntry() {
    let entry: TimesheetEntry = new TimesheetEntry(this.newEntry.code, this.selectedDay, this.newEntry.startTime, this.newEntry.endTime, this.newEntry.details);
    if (this.timesheetExists) {
      this._timesheetService.addTimesheetEntry(this.activeTimeSheet.id, entry).subscribe(
        res => {
          //Update the entry with the primary key that has come back from server
          entry.id = res as number;
          this.activeTimeSheet.timesheetEntries.push(entry);

          this.pushTimesheetToCalendarDays(entry);

        }, error => this.errors = error);
    }
    else {
      this.pushTimesheetToCalendarDays(entry);
      //Automatically save timesheet if it has not been saved already
      this.saveTimesheet();
    }

    this.toggleDisplayAddTimesheet();
  }

  pushTimesheetToCalendarDays(entry:TimesheetEntry) {
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

  populateTimesheet(entries: TimesheetEntry[]) {
    for (let item of entries) {
      this.activeTimeSheet.timesheetEntries.push(item);
    }
  }
  saveTimesheet() {
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
        console.log(res);
        this.timesheetExists = true;
        this.activeTimeSheet.id = res as number;
        this.timesheets.push(this.activeTimeSheet);
        $("#myNewTimesheetModal").modal('hide');
      },
      error => this.errors = error);
  }

  submitTimesheet() {
    this.activeTimeSheet.status = "Submitted";
    this._timesheetService.updateTimesheet(this.activeTimeSheet).subscribe(
      res => {
        console.log(res);
      },
      error => this.errors = error);
  }
}

export class DayOfWeek {
  constructor(
    public nameOfDay: string,
    public date: Date
  ) { }
} 
