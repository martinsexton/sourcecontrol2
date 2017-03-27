var employeeApp = angular.module('employeeApp', ['ngAnimate', 'ngAria', 'ngMaterial']);

employeeApp.controller('employeeController', ['$scope', '$mdToast', '$http', function ($scope, $mdToast, $http) {
    $scope.employee = { FirstName: "", Surname: ""};
    $scope.currentTab = "listemployees";
    $scope.showListEmployeesTab = true;
    $scope.showNewEmployeeTab = false;

    $scope.saveEmployee = function () {
        var request = {
            method: 'POST',
            url: 'http://doneillwebapi.azurewebsites.net/api/employee',
            data: JSON.stringify($scope.employee),
            headers: { 'Content-Type': 'application/json' }
        };

        // SEND THE FILES.
        $http(request)
            .success(function (d) {
                $scope.employee = { FirstName: "", Surname: "" };
                $mdToast.show(
                    $mdToast.simple('Successfully saved Employee!')
                    .position('left bottom')
                    .hideDelay(2000)
                );
            })
            .error(function () {
            });
    }

    $scope.setTab = function (tabname) {
        $scope.currentTab = tabname;
        if (tabname == "listemployees") {
            $scope.showListEmployeesTab = true;
            $scope.showNewEmployeeTab = false;
        }
        if (tabname == "newproject") {
            $scope.showListEmployeesTab = false;
            $scope.showNewEmployeeTab = true;
        }
    }
}]);

employeeApp.controller('listEmployeesController', ['$scope', '$mdToast', function ($scope, $mdToast) {
    $scope.currentTab = "listemployees";
}]);