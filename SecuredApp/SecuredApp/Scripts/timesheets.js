var timeApp = angular.module('timeApp', []);

timeApp.controller('recordTimesheetController', ['$scope', 'projectService', function ($scope, projectService) {
    projectService.getProjects().then(function mySucces(response) {
        $scope.projects = response.data;
    }, function myError(response) {
    })
}]);

timeApp.service('projectService', ['$http', function ($http) {
    this.getProjects = function () {
        return $http({
            method: "GET",
            url: "http://localhost:51745/api/projects",
            headers: { 'Content-Type': 'application/json' }
        });
    }
}]);