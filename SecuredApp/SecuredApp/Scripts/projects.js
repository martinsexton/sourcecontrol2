var postApp = angular.module('postApp', []);

postApp.controller('postController', ['$scope', '$http', function ($scope, $http) {
    //Create empty project object
    $scope.project = { projName: "", fromDate: "", contactNumber: "", details: "" };
    $scope.GetAllData = function () {
        return $http({
            method: "GET",
            url: "http://localhost:51745/api/projects",
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {
            alert("success");
        }, function errorCallback(response) {
            alert("failed");
        });
        /*return $http({
            method: "GET",
            url: "http://localhost:51745/api/projects",
            headers: { 'Content-Type': 'application/json' }
        }).success(function (data) {
            //$scope.employees = data;
            alert("success");
        }).error(function (data) {
            alert("failure");
        });;*/
    };
    //projectService.getProjects($scope);
}]);

postApp.service('projectService', ['$http', function ($http) {
    this.getProjects = function ($scope) {
        return $http({
            method: "GET",
            url: "http://localhost:51745/api/projects",
            headers: { 'Content-Type': 'application/json' }
        }).success(function (data) {
            //$scope.employees = data;
            alert("success");
        }).error(function (data) {
            alert("failure");
        });;
    };
}]);
