angular.module("applicationAdminModule").controller("accountVerificationTokenController", function (id,token, $scope, $state, $auth, helperService, accountRepository, GlobalInfo, $location) {

    $scope.message = "";
    $scope.response = "";

    var handlerEmailConfirmation = function (id,token) {

        accountRepository.verificationToken(id,token).then(

            function (response) {

                $scope.response = response;
            },
            function (response) {
                helperService.handlerError(response);
            }
        );
    };

    handlerEmailConfirmation(id,token);

});
