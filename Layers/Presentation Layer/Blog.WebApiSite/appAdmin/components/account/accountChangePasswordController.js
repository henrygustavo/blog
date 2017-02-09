angular.module("applicationAdminModule").controller("accountChangePasswordController", function ($scope, $state, $auth, helperService, accountRepository, GlobalInfo, $location) {

    $scope.changePassowrd = function (model) {

        accountRepository.changePassowrd(model).then(
            function (response) {
                helperService.showAlertResponse(response);
                $auth.logout();
                $state.go('login');
            },
            function (response) {
                helperService.handlerError(response);
            }
        );
    };
});