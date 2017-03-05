﻿var app = angular.module('myApp', []);
app.directive("w3TestDirective", function () {
    return {
        template: "<h1>Made by a directive!</h1>"
    };
});
app.controller('myCtrl', function ($scope) {
    $scope.firstName = "John";
    $scope.lastName = "Doe";
});