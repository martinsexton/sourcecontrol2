"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.TimesheetEntry = void 0;
var TimesheetEntry = /** @class */ (function () {
    function TimesheetEntry(code, day, startTime, endTime, details, userName, chargeable) {
        this.code = code;
        this.day = day;
        this.startTime = startTime;
        this.endTime = endTime;
        this.details = details;
        this.userName = userName;
        this.chargeable = chargeable;
    }
    return TimesheetEntry;
}());
exports.TimesheetEntry = TimesheetEntry;
//# sourceMappingURL=timesheetentry.js.map