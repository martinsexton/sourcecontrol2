import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { ProjectComponent } from './projects/projects.component';
import { TimesheetComponent } from './timesheets/timesheets.component';
import { Timesheet2Component } from './timesheets2/timesheets2.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { UserDashboardComponent } from './userdashboard/userdashboard.component';
import { UserDashboard2Component } from './userdashboard2/userdashboard2.component';
import { RegistrationFormComponent } from './account/registration-form/registration-form.component';
import { LoginFormComponent } from './account/login-form/login-form.component';
import { ChooseTenantComponent } from './components/choosetenant/choosetenant.component';
import { AuthGuard } from './auth.guard';


const appRoutes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'register', component: RegistrationFormComponent },
  { path: 'login', component: LoginFormComponent },
  { path: 'counter', component: CounterComponent, canActivate: [AuthGuard] },
  { path: 'fetch-data', component: FetchDataComponent, canActivate: [AuthGuard] },
  { path: 'projects', component: ProjectComponent, canActivate: [AuthGuard] },
  { path: 'timesheets', component: TimesheetComponent, canActivate: [AuthGuard] },
  { path: 'timesheets2', component: Timesheet2Component, canActivate: [AuthGuard] },
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
  { path: 'userdashboard', component: UserDashboardComponent, canActivate: [AuthGuard] },
  { path: 'userdashboard2', component: UserDashboard2Component, canActivate: [AuthGuard] },
  { path: 'choosetenant', component: ChooseTenantComponent, canActivate: [AuthGuard] }
  
];


export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes); 
