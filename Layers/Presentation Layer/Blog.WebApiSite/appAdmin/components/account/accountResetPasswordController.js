angular.module("applicationAdminModule").controller('accountResetPasswordController', function (token,$scope, $location, $state, accountRepository, helperService) {

    $scope.resetPassword = function (model) {

        if (token != undefined) {

            $scope.model.code = token;

            accountRepository.resetPassword(model).then(
                function(response) {
                    helperService.showAlertResponse(response);
                    $state.go('login');
                },
                function(response) {
                    helperService.handlerError(response);
                }
            );          
        }
    };
});
