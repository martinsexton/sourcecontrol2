var employeeApp = angular.module('employeeApp', ['ngAnimate', 'ngAria', 'ngMaterial']);

employeeApp.controller('employeeController', ['$scope', '$mdToast', '$http', function ($scope, $mdToast, $http) {
    $scope.currentTab = "listemployees";
    $scope.showListEmployeesTab = true;
    $scope.showNewEmployeeTab = false;

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