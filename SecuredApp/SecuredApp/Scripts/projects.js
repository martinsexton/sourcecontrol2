var postApp = angular.module('postApp', []);

postApp.controller('postController', ['$scope', '$http', function ($scope, $http) {
    $scope.project = { projName: "", fromDate: "", contactNumber: "", details: "" };
}]);

postApp.controller('listProjectsController', ['$scope', 'projectService', function ($scope, projectService) {
    projectService.getProjects().then(function mySucces(response) {
        $scope.projects = response.data;
        if (response.data.length > 0) {
            $scope.item_details = response.data[0];
        }
    }, function myError(response) {
    })

    $scope.showDetails = function (project) {
        $scope.item_details = project;
    }
}]);

postApp.service('projectService', ['$http', function ($http) {
    this.getProjects = function () {
        return $http({
            method: "GET",
            url: "http://doneillserver.azurewebsites.net/api/projects",
            headers: { 'Content-Type': 'application/json' }
        });
    }
}]);

postApp.service('projectService', ['$http', function ($http) {
    this.getProjects = function ($scope) {
        return $http({
            method: "GET",
            url: "http://doneillserver.azurewebsites.net/api/projects",
            headers: { 'Content-Type': 'application/json' }
        }).success(function (data) {
        }).error(function (data) {
        });;
    };
}]);
