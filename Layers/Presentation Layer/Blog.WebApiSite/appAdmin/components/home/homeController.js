angular.module("applicationAdminModule").controller('homeController', function ($location, $state, homeRepository,$auth,helperService) {

	helperService.activateView('home');

   var verifyIsisAuthenticated = function () {

        if (!$auth.isAuthenticated()) {
            $state.go('login');
        }
    }

    verifyIsisAuthenticated();
});
