"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.routing = void 0;
var router_1 = require("@angular/router");
var home_component_1 = require("./home/home.component");
var counter_component_1 = require("./counter/counter.component");
var fetch_data_component_1 = require("./fetch-data/fetch-data.component");
var projects_component_1 = require("./projects/projects.component");
var timesheets_component_1 = require("./timesheets/timesheets.component");
var holidays_component_1 = require("./holidays/holidays.component");
var dashboard_component_1 = require("./dashboard/dashboard.component");
var supervisor_component_1 = require("./supervisor/supervisor.component");
var labour_componenet_1 = require("./labour/labour.componenet");
var userdashboard_component_1 = require("./userdashboard/userdashboard.component");
var registration_form_component_1 = require("./account/registration-form/registration-form.component");
var projecthealth_component_1 = require("./projecthealth/projecthealth.component");
var assignmentdetails_component_1 = require("./assignmentdetails/assignmentdetails.component");
var login_form_component_1 = require("./account/login-form/login-form.component");
var auth_guard_1 = require("./auth.guard");
var appRoutes = [
    { path: '', component: home_component_1.HomeComponent },
    { path: 'register', component: registration_form_component_1.RegistrationFormComponent },
    { path: 'login', component: login_form_component_1.LoginFormComponent },
    { path: 'counter', component: counter_component_1.CounterComponent, canActivate: [auth_guard_1.AuthGuard] },
    { path: 'fetch-data', component: fetch_data_component_1.FetchDataComponent, canActivate: [auth_guard_1.AuthGuard] },
    { path: 'projects', component: projects_component_1.ProjectComponent, canActivate: [auth_guard_1.AuthGuard] },
    { path: 'timesheets', component: timesheets_component_1.TimesheetComponent, canActivate: [auth_guard_1.AuthGuard] },
    { path: 'holidays', component: holidays_component_1.HolidaysComponent, canActivate: [auth_guard_1.AuthGuard] },
    { path: 'dashboard', component: dashboard_component_1.DashboardComponent, canActivate: [auth_guard_1.AuthGuard] },
    { path: 'supervisor', component: supervisor_component_1.SupervisorComponent, canActivate: [auth_guard_1.AuthGuard] },
    { path: 'labour', component: labour_componenet_1.LabourComponent, canActivate: [auth_guard_1.AuthGuard] },
    { path: 'userdashboard', component: userdashboard_component_1.UserDashboardComponent, canActivate: [auth_guard_1.AuthGuard] },
    { path: 'projecthealth', component: projecthealth_component_1.ProjectHealthComponent, canActivate: [auth_guard_1.AuthGuard] },
    { path: 'assignment', component: assignmentdetails_component_1.AssignmentDetailsComponent, canActivate: [auth_guard_1.AuthGuard] }
];
exports.routing = router_1.RouterModule.forRoot(appRoutes);
//# sourceMappingURL=app.routing.js.map