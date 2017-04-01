var timeApp = angular.module('timeApp', ['ngAnimate', 'ngAria', 'ngMaterial']);

timeApp.controller('listTimesheetController', ['$scope', '$http', 'timesheetService', function ($scope, $http, timesheetService) {
    $scope.sortType = 'engineerName'; // set the default sort type
    $scope.sortReverse = false;  // set the default sort order
    $scope.showtsdetails = false;

    timesheetService.getTimesheets().then(function mySucces(response) {
        $scope.timesheets = response.data;
    }, function myError(response) {
    })

    $scope.showTimesheetDetails = function (timesheet) {
        timesheetService.getTimesheetItems(timesheet).then(function mySucces(response) {
            $scope.timesheetitems = response.data;
            $scope.selectedtimesheet = timesheet;
            if ($scope.timesheetitems.length > 0) {
                $scope.showtsdetails = true;
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

timeApp.controller('recordTimesheetController', ['$scope', '$mdToast', '$http', 'projectService', function ($scope, $mdToast, $http, projectService) {
    $scope.showdetails = false;
    $scope.showsavebutton = false;
    $scope.days = ["Mon", "Tue", "Wed", "Thurs", "Fri"];

    $scope.mondayDetails = [];
    $scope.mondayTotalHours = 0;

    $scope.tuesdayDetails = [];
    $scope.tuesdayTotalHours = 0;

    $scope.wednesdayDetails = [];
    $scope.wednesdayTotalHours = 0;

    $scope.thursdayDetails = [];
    $scope.thursdayTotalHours = 0;

    $scope.fridayDetails = [];
    $scope.fridayTotalHours = 0;

    $scope.saturdayDetails = [];
    $scope.saturdayTotalHours = 0;

    $scope.sundayDetails = [];
    $scope.sundayTotalHours = 0;

    $scope.timesheet = { engineerName: "", weekEndDate: "", mondayItems: $scope.mondayDetails, tuesdayItems : $scope.tuesdayDetails, wednesdayItems : $scope.wednesdayDetails, thursdayItems : $scope.thursdayDetails, fridayItems : $scope.fridayDetails, saturdayItems: $scope.saturdayDetails, sundayItems : $scope.sundayDetails }

    $scope.displayMonDetails = false;
    $scope.displayTueDetails = false;
    $scope.displayWedDetails = false;
    $scope.displayThursDetails = false;
    $scope.displayFriDetails = false;
    $scope.displaySatDetails = false;
    $scope.displaySunDetails = false;

    $scope.recordMondayDetails = function () {
        $scope.showsavebutton = true;
        $difference = $scope.recordedMonday.dayEndTime - $scope.recordedMonday.dayStartTime;
        $scope.mondayTotalHours = $scope.mondayTotalHours + $difference;

        $scope.mondayDetails.push({ Day: "Monday", ProjectName: $scope.recordedMonday.ProjectName, dayStartTime: $scope.recordedMonday.dayStartTime, dayEndTime: $scope.recordedMonday.dayEndTime })
    }
    $scope.recordTuesdayDetails = function () {
        $scope.showsavebutton = true;
        $difference = $scope.recordedTuesday.dayEndTime - $scope.recordedTuesday.dayStartTime;
        $scope.tuesdayTotalHours = $scope.tuesdayTotalHours + $difference;

        $scope.tuesdayDetails.push({ Day: "Tuesday", ProjectName: $scope.recordedTuesday.ProjectName, dayStartTime: $scope.recordedTuesday.dayStartTime, dayEndTime: $scope.recordedTuesday.dayEndTime })
    }
    $scope.recordWednesdayDetails = function () {
        $scope.showsavebutton = true;
        $difference = $scope.recordedWednesday.dayEndTime - $scope.recordedWednesday.dayStartTime;
        $scope.wednesdayTotalHours = $scope.wednesdayTotalHours + $difference;

        $scope.wednesdayDetails.push({ Day: "Wednesday", ProjectName: $scope.recordedWednesday.ProjectName, dayStartTime: $scope.recordedWednesday.dayStartTime, dayEndTime: $scope.recordedWednesday.dayEndTime })
    }
    $scope.recordThursdayDetails = function () {
        $scope.showsavebutton = true;
        $difference = $scope.recordedThursday.dayEndTime - $scope.recordedThursday.dayStartTime;
        $scope.thursdayTotalHours = $scope.thursdayTotalHours + $difference;

        $scope.thursdayDetails.push({ Day: "Thursday", ProjectName: $scope.recordedThursday.ProjectName, dayStartTime: $scope.recordedThursday.dayStartTime, dayEndTime: $scope.recordedThursday.dayEndTime })
    }
    $scope.recordFridayDetails = function () {
        $scope.showsavebutton = true;
        $difference = $scope.recordedFriday.dayEndTime - $scope.recordedFriday.dayStartTime;
        $scope.fridayTotalHours = $scope.fridayTotalHours + $difference;

        $scope.fridayDetails.push({ Day: "Friday", ProjectName: $scope.recordedFriday.ProjectName, dayStartTime: $scope.recordedFriday.dayStartTime, dayEndTime: $scope.recordedFriday.dayEndTime })
    }
    $scope.recordSaturdayDetails = function () {
        $scope.showsavebutton = true;
        $difference = $scope.recordedSaturday.dayEndTime - $scope.recordedSaturday.dayStartTime;
        $scope.saturdayTotalHours = $scope.saturdayTotalHours + $difference;

        $scope.saturdayDetails.push({ Day: "Saturday", ProjectName: $scope.recordedSaturday.ProjectName, dayStartTime: $scope.recordedSaturday.dayStartTime, dayEndTime: $scope.recordedSaturday.dayEndTime })
    }
    $scope.recordSundayDetails = function () {
        $scope.showsavebutton = true;
        $difference = $scope.recordedSunday.dayEndTime - $scope.recordedSunday.dayStartTime;
        $scope.sundaydayTotalHours = $scope.sundaydayTotalHours + $difference;

        $scope.sundayDetails.push({ Day: "Sunday", ProjectName: $scope.recordedSunday.ProjectName, dayStartTime: $scope.recordedSunday.dayStartTime, dayEndTime: $scope.recordedSunday.dayEndTime })
    }

    $scope.showMon = function () {
        $scope.displayMonDetails = true;
        $scope.displayTueDetails = false;
        $scope.displayWedDetails = false;
        $scope.displayThursDetails = false;
        $scope.displayFriDetails = false;
        $scope.displaySatDetails = false;
        $scope.displaySunDetails = false;
    }
    $scope.showTue = function () {
        $scope.displayMonDetails = false;
        $scope.displayTueDetails = true;
        $scope.displayWedDetails = false;
        $scope.displayThursDetails = false;
        $scope.displayFriDetails = false;
        $scope.displaySatDetails = false;
        $scope.displaySunDetails = false;
    }
    $scope.showWed = function () {
        $scope.displayMonDetails = false;
        $scope.displayTueDetails = false;
        $scope.displayWedDetails = true;
        $scope.displayThursDetails = false;
        $scope.displayFriDetails = false;
        $scope.displaySatDetails = false;
        $scope.displaySunDetails = false;
    }
    $scope.showThurs = function () {
        $scope.displayMonDetails = false;
        $scope.displayTueDetails = false;
        $scope.displayWedDetails = false;
        $scope.displayThursDetails = true;
        $scope.displayFriDetails = false;
        $scope.displaySatDetails = false;
        $scope.displaySunDetails = false;
    }
    $scope.showFri = function () {
        $scope.displayMonDetails = false;
        $scope.displayTueDetails = false;
        $scope.displayWedDetails = false;
        $scope.displayThursDetails = false;
        $scope.displayFriDetails = true;
        $scope.displaySatDetails = false;
        $scope.displaySunDetails = false;
    }
    $scope.showSat = function () {
        $scope.displayMonDetails = false;
        $scope.displayTueDetails = false;
        $scope.displayWedDetails = false;
        $scope.displayThursDetails = false;
        $scope.displayFriDetails = false;
        $scope.displaySatDetails = true;
        $scope.displaySunDetails = false;
    }
    $scope.showSun = function () {
        $scope.displayMonDetails = false;
        $scope.displayTueDetails = false;
        $scope.displayWedDetails = false;
        $scope.displayThursDetails = false;
        $scope.displayFriDetails = false;
        $scope.displaySatDetails = false;
        $scope.displaySunDetails = true;
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
                $mdToast.show(
                    $mdToast.simple('Successfully saved Timesheet!')
                    .position('left bottom')
                    .hideDelay(2000)
                );

                $scope.timesheet.engineerName = "";
                $scope.timesheet.weekEndDate = "";

                $scope.displayMonDetails = false;
                $scope.displayTueDetails = false;
                $scope.displayWedDetails = false;
                $scope.displayThursDetails = false;
                $scope.displayFriDetails = false;
                $scope.displaySatDetails = false;
                $scope.displaySunDetails = false;

                $scope.mondayDetails.length = 0;
                $scope.tuesdayDetails.length = 0;
                $scope.wednesdayDetails.length = 0;
                $scope.thursdayDetails.length = 0;
                $scope.fridayDetails.length = 0;
                $scope.saturdayDetails.length = 0;
                $scope.sundayDetails.length = 0;

                $scope.recordedMonday.ProjectName = "";
                $scope.recordedMonday.dayEndTime = "";
                $scope.recordedMonday.dayStartTime = "";

                $scope.recordedTuesday.ProjectName = "";
                $scope.recordedTuesday.dayEndTime = "";
                $scope.recordedTuesday.dayStartTime = "";

                $scope.recordedWednesday.ProjectName = "";
                $scope.recordedWednesday.dayEndTime = "";
                $scope.recordedWednesday.dayStartTime = "";

                $scope.recordedThursday.ProjectName = "";
                $scope.recordedThursday.dayEndTime = "";
                $scope.recordedThursday.dayStartTime = "";

                $scope.recordedFriday.ProjectName = "";
                $scope.recordedFriday.dayEndTime = "";
                $scope.recordedFriday.dayStartTime = "";

                $scope.recordedSaturday.ProjectName = "";
                $scope.recordedSaturday.dayEndTime = "";
                $scope.recordedSaturday.dayStartTime = "";

                $scope.recordedSunday.ProjectName = "";
                $scope.recordedSunday.dayEndTime = "";
                $scope.recordedSunday.dayStartTime = "";
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