"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Project = void 0;
var Project = /** @class */ (function () {
    function Project(id, client, name, code, details, isActive, startDate) {
        this.id = id;
        this.client = client;
        this.name = name;
        this.code = code;
        this.details = details;
        this.isActive = isActive;
        this.startDate = startDate;
    }
    return Project;
}());
exports.Project = Project;
//# sourceMappingURL=project.js.map