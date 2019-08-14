import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID } from '@angular/core';
import { FormsModule } from '@angular/forms';
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
import { HolidaysComponent } from './holidays/holidays.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { SupervisorComponent } from './supervisor/supervisor.component';
import { UserDashboardComponent } from './userdashboard/userdashboard.component';
import { RegistrationFormComponent } from './account/registration-form/registration-form.component';
import { LoginFormComponent } from './account/login-form/login-form.component';
import { ProjectHealthComponent } from './projecthealth/projecthealth.component';
import { LabourComponent } from './labour/labour.componenet';

import { EmailValidator } from './directives/email.validator.directive';
import { PasswordValidator } from './directives/password.validator.directive';
import { EndtimeValidator } from './directives/endtime.validator.directive';
import { myFocus } from './directives/focus.directive';

/* Import Services */
import { ProjectService } from './shared/services/project.service';
import { CertificateService } from './shared/services/certificate.service';
import { HolidayService } from './shared/services/holiday.service';
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
    RegistrationFormComponent,
    ProjectHealthComponent,
    LoginFormComponent,
    ProjectComponent,
    TimesheetComponent,
    HolidaysComponent,
    DashboardComponent,
    SupervisorComponent,
    LabourComponent,
    UserDashboardComponent,
    EmailValidator,
    PasswordValidator,
    EndtimeValidator,
    myFocus
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    routing,
    ChartsModule
  ],
  providers: [ProjectService, TimesheetService, CertificateService, HolidayService, NotificationService, MsUserService, AuthGuard, {
    provide: LOCALE_ID,
    useValue: 'en-IE'
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
