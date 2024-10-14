"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Timesheet = void 0;
var Timesheet = /** @class */ (function () {
    function Timesheet(id, username, owner, role, weekStarting, dateSubmitted, status) {
        this.id = id;
        this.username = username;
        this.owner = owner;
        this.role = role;
        this.weekStarting = weekStarting;
        this.dateSubmitted = dateSubmitted;
        this.status = status;
        this.timesheetEntries = new Array();
        this.timesheetNotes = new Array();
    }
    return Timesheet;
}());
exports.Timesheet = Timesheet;
//# sourceMappingURL=timesheet.js.map