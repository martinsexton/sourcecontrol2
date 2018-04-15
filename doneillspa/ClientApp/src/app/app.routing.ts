import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { ProjectComponent } from './projects/projects.component';
import { RegistrationFormComponent } from './account/registration-form/registration-form.component';


const appRoutes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'register', component: RegistrationFormComponent },
  { path: 'counter', component: CounterComponent },
  { path: 'fetch-data', component: FetchDataComponent },
  { path: 'projects', component: ProjectComponent }
];


export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes); 
