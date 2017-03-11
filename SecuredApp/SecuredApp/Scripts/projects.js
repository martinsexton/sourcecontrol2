var postApp = angular.module('postApp', []);

postApp.controller('postController', ['$scope', '$http', function ($scope, $http) {
    $scope.project = { Name: "", StartDate: "", ContactNumber: "", Details: "" };
    $scope.saveProject = function () {
        var request = {
            method: 'POST',
            url: 'http://localhost:51745/api/projects',
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

        //return $http({
        //    method: "POST",
        //    url: "http://localhost:51745/api/projects",
        //    data: $scope.project,
        //    headers: { 'Content-Type': 'application/json' }
        //});
    }
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
            url: "http://localhost:51745/api/projects",
            headers: { 'Content-Type': 'application/json' }
        });
    }
}]);

//postApp.service('projectService', ['$http', function ($http) {
//    this.getProjects = function ($scope) {
//        return $http({
//            method: "GET",
//            url: "http://localhost:51745/api/projects",
//            headers: { 'Content-Type': 'application/json' }
//        }).success(function (data) {
//        }).error(function (data) {
//        });;
//    };
//}]);
