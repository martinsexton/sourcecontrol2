import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ChartsModule } from 'ng2-charts';

import { routing } from './app.routing';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { ProjectComponent } from './projects/projects.component';
import { TimesheetComponent } from './timesheets/timesheets.component';
import { Timesheet2Component } from './timesheets2/timesheets2.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { UserDashboardComponent } from './userdashboard/userdashboard.component';
import { NotificationComponent } from './components/notification/notification.component';
import { UserDetailsComponent } from './components/userdetails/userdetails.component';

import { RegistrationFormComponent } from './account/registration-form/registration-form.component';
import { LoginFormComponent } from './account/login-form/login-form.component';

import { TimeSheetEntryListComponent } from './components/timesheetentrylist/timesheetentrylist.component';
import { TimeSheetListComponent } from './components/timesheetlist/timesheetlist.component';
import { ProjectsListComponent } from './components/projectlist/projectlist.component';


import { EmailValidator } from './directives/email.validator.directive';
import { PasswordValidator } from './directives/password.validator.directive';
import { EndtimeValidator } from './directives/endtime.validator.directive';
import { ProjectCodeValidator } from './directives/projectcode.validator.directive';
import { myFocus } from './directives/focus.directive';

/* Import Services */
import { ProjectService } from './shared/services/project.service';
import { NotificationService } from './shared/services/notification.service';
import { TimesheetService } from './shared/services/timesheet.service';
import { ConfigService } from './shared/utils/config.service';
import { MsUserService } from './shared/services/msuser.service';
import { AuthGuard } from './auth.guard';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    TimeSheetEntryListComponent,
    TimeSheetListComponent,
    ProjectsListComponent,
    RegistrationFormComponent,
    UserDetailsComponent,
    LoginFormComponent,
    ProjectComponent,
    TimesheetComponent,
    Timesheet2Component,
    DashboardComponent,
    UserDashboardComponent,
    NotificationComponent,
    EmailValidator,
    PasswordValidator,
    EndtimeValidator,
    ProjectCodeValidator,
    myFocus
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    routing,
    ChartsModule
  ],
  providers: [ProjectService, TimesheetService, NotificationService, MsUserService, AuthGuard, {
    provide: LOCALE_ID,
    useValue: 'en-IE'
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
