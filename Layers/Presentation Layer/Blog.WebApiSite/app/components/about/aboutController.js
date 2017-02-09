angular.module("applicationModule").controller('aboutController', function ($scope,helperService, $state) {
    helperService.activateView('about');
    $scope.model = {};

});