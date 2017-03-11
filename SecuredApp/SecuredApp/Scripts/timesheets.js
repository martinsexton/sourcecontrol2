var timeApp = angular.module('timeApp', []);

timeApp.controller('recordTimesheetController', ['$scope', '$http', 'projectService', function ($scope, $http, projectService) {
    $scope.recordedDays = [];
    $scope.recordedDay = { Day: "", dayStartTime: "", dayEndTime: "" };
    $scope.days = ["Mon", "Tue", "Wed", "Thurs", "Fri"];
    $scope.timesheet = {engineerName:"",weekEndDate:"",timesheetProject:"",items:$scope.recordedDays}

    $scope.recordDay = function () {
        $scope.recordedDays.push({ Day: $scope.recordedDay.Day, dayStartTime: $scope.recordedDay.dayStartTime, dayEndTime: $scope.recordedDay.dayEndTime })
        var index = $scope.days.indexOf($scope.recordedDay.Day);
        $scope.days.splice(index, 1);
    }

    $scope.saveTimesheet = function() {
        var request = {
            method: 'POST',
            url: 'http://doneillwebapi.azurewebsites.net/api/timesheet',
            data: JSON.stringify($scope.timesheet),
            headers: { 'Content-Type': 'application/json' }
        };

        // SEND THE FILES.
        $http(request)
            .success(function (d) {
                $scope.project = { Name: "", StartDate: "", ContactNumber: "", Details: "" };
            })
            .error(function () {
            });
    }

    projectService.getProjects().then(function mySucces(response) {
        $scope.projects = response.data;
    }, function myError(response) {
    })
}]);

timeApp.service('projectService', ['$http', function ($http) {
    this.getProjects = function () {
        return $http({
            method: "GET",
            url: "http://doneillwebapi.azurewebsites.net/api/project",
            headers: { 'Content-Type': 'application/json' }
        });
    }
}]);