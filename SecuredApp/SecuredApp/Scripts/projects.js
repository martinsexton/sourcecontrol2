var postApp = angular.module('postApp', []);

postApp.controller('postController', ['$scope', '$http', function ($scope, $http) {
    $scope.project = { Name: "", StartDate: "", ContactNumber: "", Details: "" };
    $scope.saveProject = function () {
        var request = {
            method: 'POST',
            url: 'http://doneillwebapi.azurewebsites.net/api/project',
            data: JSON.stringify($scope.project),
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
}]);

postApp.controller('listProjectsController', ['$scope', 'projectService', function ($scope, projectService) {
    $scope.sortType = 'Name'; // set the default sort type
    $scope.sortReverse = false;  // set the default sort order

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
            url: "http://doneillwebapi.azurewebsites.net/api/project",
            headers: { 'Content-Type': 'application/json' }
        });
    }
}]);
