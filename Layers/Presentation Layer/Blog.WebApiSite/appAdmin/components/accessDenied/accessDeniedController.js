angular.module("applicationAdminModule").controller('accessDeniedController', function ($location, $state, $auth, helperService) {
    helperService.activateView('home');

});
