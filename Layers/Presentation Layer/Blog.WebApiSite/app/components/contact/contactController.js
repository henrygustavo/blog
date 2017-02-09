angular.module("applicationModule").controller('contactController', function ($scope,helperService, $state) {
    helperService.activateView('contact');
    $scope.model = {};

});