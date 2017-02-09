angular.module("applicationAdminModule").controller("accountForgotPasswordController", function ($scope, $state, $auth, helperService, accountRepository, GlobalInfo, $location) {

    $scope.forgotPassword = function (model) {

        model.resetUrl = GlobalInfo.resetUrl;

        accountRepository.forgotPassword(model).then(
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