"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ApplicationUser = void 0;
var ApplicationUser = /** @class */ (function () {
    function ApplicationUser(id, firstName, surname, email, phoneNumber, role, isEnabled, tenantId) {
        this.id = id;
        this.firstName = firstName;
        this.surname = surname;
        this.email = email;
        this.phoneNumber = phoneNumber;
        this.role = role;
        this.isEnabled = isEnabled;
        this.tenantId = tenantId;
        this.emailNotifications = new Array();
    }
    return ApplicationUser;
}());
exports.ApplicationUser = ApplicationUser;
//# sourceMappingURL=applicationuser.js.map