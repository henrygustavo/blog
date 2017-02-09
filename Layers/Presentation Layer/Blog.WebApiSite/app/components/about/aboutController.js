angular.module("applicationModule").controller('aboutController', function ($scope,helperService,personalInformationRepository, $state) {
    helperService.activateView('about');
    $scope.personalInformation = {};

    $scope.renderHtml = function (html_code) {
        return helperService.renderHtml(html_code);
    };

    var getPersonalInformation = function () {

        personalInformationRepository.getPersonalInformation().then(
                function (response) {

                    $scope.personalInformation = response;
                },
                function (response) {
                    helperService.handlerError(response);
                }
        );
    };
    getPersonalInformation();

});