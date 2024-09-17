"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.routing = void 0;
var router_1 = require("@angular/router");
var home_component_1 = require("./home/home.component");
var counter_component_1 = require("./counter/counter.component");
var fetch_data_component_1 = require("./fetch-data/fetch-data.component");
var projects_component_1 = require("./projects/projects.component");
var timesheets2_component_1 = require("./timesheets2/timesheets2.component");
var dashboard_component_1 = require("./dashboard/dashboard.component");
var userdashboard2_component_1 = require("./userdashboard2/userdashboard2.component");
var registration_form_component_1 = require("./account/registration-form/registration-form.component");
var login_form_component_1 = require("./account/login-form/login-form.component");
var choosetenant_component_1 = require("./components/choosetenant/choosetenant.component");
var auth_guard_1 = require("./auth.guard");
var appRoutes = [
    { path: '', component: home_component_1.HomeComponent },
    { path: 'register', component: registration_form_component_1.RegistrationFormComponent },
    { path: 'login', component: login_form_component_1.LoginFormComponent },
    { path: 'counter', component: counter_component_1.CounterComponent, canActivate: [auth_guard_1.AuthGuard] },
    { path: 'fetch-data', component: fetch_data_component_1.FetchDataComponent, canActivate: [auth_guard_1.AuthGuard] },
    { path: 'projects', component: projects_component_1.ProjectComponent, canActivate: [auth_guard_1.AuthGuard] },
    { path: 'timesheets2', component: timesheets2_component_1.Timesheet2Component, canActivate: [auth_guard_1.AuthGuard] },
    { path: 'dashboard', component: dashboard_component_1.DashboardComponent, canActivate: [auth_guard_1.AuthGuard] },
    { path: 'userdashboard2', component: userdashboard2_component_1.UserDashboard2Component, canActivate: [auth_guard_1.AuthGuard] },
    { path: 'choosetenant', component: choosetenant_component_1.ChooseTenantComponent, canActivate: [auth_guard_1.AuthGuard] }
];
exports.routing = router_1.RouterModule.forRoot(appRoutes);
//# sourceMappingURL=app.routing.js.map