var timeApp = angular.module('timeApp', []);

timeApp.controller('listTimesheetController', ['$scope', '$http', 'timesheetService', function ($scope, $http, timesheetService) {
    $scope.showtimesheetdetails = false;
    timesheetService.getTimesheets().then(function mySucces(response) {
        $scope.timesheets = response.data;
    }, function myError(response) {
    })

    $scope.showTimesheetDetails = function (timesheet) {
        timesheetService.getTimesheetItems(timesheet).then(function mySucces(response) {
            $scope.timesheetitems = response.data;
            $scope.selectedtimesheet = timesheet;
            if ($scope.timesheetitems.length > 0) {
                $scope.showtimesheetdetails = true;
            }
        }, function myError(response) {
        })
    }
}]);

timeApp.service('timesheetService', ['$http', function ($http) {
    this.getTimesheets = function () {
        return $http({
            method: "GET",
            url: "http://doneillwebapi.azurewebsites.net/api/timesheet",
            headers: { 'Content-Type': 'application/json' }
        });
    }

    this.getTimesheetItems = function (timesheet) {
        return $http({
            method: "GET",
            params: {
                id: timesheet.identifier
            },
            url: "http://doneillwebapi.azurewebsites.net/api/timesheet",
            headers: { 'Content-Type': 'application/json' }
        });
    }
}]);

timeApp.controller('recordTimesheetController', ['$scope', '$http', 'projectService', function ($scope, $http, projectService) {
    $scope.showdetails = false;
    $scope.showsavebutton = false;
    $scope.recordedDays = [];
    $scope.recordedDay = { Day: "", ProjectName: "", dayStartTime: "", dayEndTime: "" };
    $scope.days = ["Mon", "Tue", "Wed", "Thurs", "Fri"];
    $scope.timesheet = {engineerName:"",weekEndDate:"",items:$scope.recordedDays}

    $scope.recordDay = function () {
        $scope.showdetails = true;
        $scope.recordedDays.push({ Day: $scope.recordedDay.Day, ProjectName: $scope.recordedDay.ProjectName, dayStartTime: $scope.recordedDay.dayStartTime, dayEndTime: $scope.recordedDay.dayEndTime })
        if ($scope.recordedDays.length == 5) {
            $scope.showsavebutton = true;
        }
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