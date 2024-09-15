"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Client = void 0;
var Client = /** @class */ (function () {
    function Client(id, name, isActive, tenantId) {
        this.id = id;
        this.name = name;
        this.isActive = isActive;
        this.tenantId = tenantId;
        this.projects = new Array();
    }
    return Client;
}());
exports.Client = Client;
//# sourceMappingURL=client.js.map