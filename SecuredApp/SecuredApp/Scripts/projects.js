var postApp = angular.module('postApp', ['ngAnimate', 'ngAria', 'ngMaterial']);

postApp.controller('postController', ['$scope', '$mdToast', '$http', function ($scope, $mdToast, $http) {
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
                $mdToast.show(
                    $mdToast.simple('Successfully saved Project!')
                    .position('left bottom')
                    .hideDelay(2000)
                );
            })
            .error(function () {
            });
    }
}]);

postApp.filter('offset', function () {
    return function (input, start) {
        if (input) {
            start = parseInt(start, 10);
            if (start < input.length) {
                return input.slice(start);
            }
        }
    };
});

postApp.controller('listProjectsController', ['$scope', '$mdToast', 'projectService', function ($scope, $mdToast, projectService) {
    $scope.sortType = 'Name'; // set the default sort type
    $scope.sortReverse = false;  // set the default sort order
    $scope.readOnlyMode = true;
    $scope.currentPage = 1;
    $scope.itemPerPage = 5;
    $scope.start = 0;
    $scope.showDetailsClicked = false;

    projectService.getProjects().then(function mySucces(response) {
        $scope.projects = response.data;
        if (response.data.length > 0) {
            $scope.total = $scope.projects.length;
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

    $scope.showDetails = function (project) {
        $scope.showDetailsClicked = true;
        $scope.item_details = project;
    }

    $scope.updateProject = function () {
        $scope.readOnlyMode = true;
        projectService.updateProject($scope.item_details).then(function mySucces(response) {
            $mdToast.show(
                $mdToast.simple('Successfully updated Project!')
                .position('left bottom')
                .hideDelay(2000)
            );
        }, function myError(response) {
            $mdToast.show(
                $mdToast.simple('Failed to update Project!')
                .position('left bottom')
                .hideDelay(2000)
            );
        })
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

    this.updateProject = function (projectdetails) {
        return $http({
            method: 'POST',
            url: 'http://doneillwebapi.azurewebsites.net/api/project',
            data: JSON.stringify(projectdetails),
            params: {
                id: projectdetails.identifier
            },
            headers: { 'Content-Type': 'application/json' }
        });
    }
}]);
