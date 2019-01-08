import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { ProjectComponent } from './projects/projects.component';
import { TimesheetComponent } from './timesheets/timesheets.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { LabourComponent } from './labour/labour.componenet';
import { UserDashboardComponent } from './userdashboard/userdashboard.component';
import { RegistrationFormComponent } from './account/registration-form/registration-form.component';
import { ProjectHealthComponent } from './projecthealth/projecthealth.component';
import { LoginFormComponent } from './account/login-form/login-form.component';
import { AuthGuard } from './auth.guard';


const appRoutes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'register', component: RegistrationFormComponent },
  { path: 'login', component: LoginFormComponent },
  { path: 'counter', component: CounterComponent, canActivate: [AuthGuard] },
  { path: 'fetch-data', component: FetchDataComponent, canActivate: [AuthGuard] },
  { path: 'projects', component: ProjectComponent, canActivate: [AuthGuard] },
  { path: 'timesheets', component: TimesheetComponent, canActivate: [AuthGuard] },
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
  { path: 'labour', component: LabourComponent, canActivate: [AuthGuard] },
  { path: 'userdashboard', component: UserDashboardComponent, canActivate: [AuthGuard] },
  { path: 'projecthealth', component: ProjectHealthComponent, canActivate: [AuthGuard] }
];


export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes); 
