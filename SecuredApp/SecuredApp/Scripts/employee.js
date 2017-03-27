var employeeApp = angular.module('employeeApp', ['ngAnimate', 'ngAria', 'ngMaterial']);

employeeApp.controller('employeeController', ['$scope', '$mdToast', '$http', function ($scope, $mdToast, $http) {
    $scope.employee = { FirstName: "", Surname: "", Category: ""};
    $scope.currentTab = "listemployees";
    $scope.showListEmployeesTab = true;
    $scope.showNewEmployeeTab = false;
    $scope.employeeCategories = ["1st Year Apprentice", "2nd Year Apprentice", "Electrician"];

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

employeeApp.controller('listEmployeesController', ['$scope', '$mdToast', 'employeeService', function ($scope, $mdToast, employeeService) {
    $scope.sortType = 'Name'; // set the default sort type
    $scope.sortReverse = false;  // set the default sort order
    $scope.readOnlyMode = true;
    $scope.currentPage = 1;
    $scope.itemPerPage = 5;
    $scope.start = 0;
    $scope.showDetailsClicked = false;

    employeeService.getEmployees().then(function mySucces(response) {
        $scope.employees = response.data;
        if (response.data.length > 0) {
            $scope.total = $scope.employees.length;
            $scope.item_details = response.data[0];
        }
    }, function myError(response) {
    })

    $scope.nextPage = function () {
        if (($scope.itemPerPage * $scope.currentPage) <= $scope.projects.length) {
            $scope.currentPage = $scope.currentPage + 1;
        }
    };

    $scope.previousPage = function () {
        if ($scope.currentPage > 1) {
            $scope.currentPage = $scope.currentPage - 1;
        }
    };

    $scope.showDetails = function (employee) {
        $scope.showDetailsClicked = true;
        $scope.item_details = employee;
    }
}]);

employeeApp.service('employeeService', ['$http', function ($http) {
    this.getEmployees = function () {
        return $http({
            method: "GET",
            url: "http://doneillwebapi.azurewebsites.net/api/employee",
            headers: { 'Content-Type': 'application/json' }
        });
    }
}]);