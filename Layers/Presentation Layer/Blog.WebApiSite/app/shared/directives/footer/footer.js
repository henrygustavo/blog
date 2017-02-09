angular.module("applicationModule").directive('footer', function () {
    return {
        restrict: 'A', //This menas that it will be used as an attribute and NOT as an element. I don't like creating custom HTML elements
        replace: true,
        templateUrl: "/app/shared/directives/footer/footerView.html",
                controller: ['$scope', '$filter', 'personalInformationRepository','helperService', function ($scope, $filter, personalInformationRepository,helperService) {

                    $scope.footer = {};

                var getPersonalInformation = function () {

                    personalInformationRepository.getPersonalInformation().then(
                            function (response) {

                                $scope.footer = response;
                                $scope.footer.year = new Date().getFullYear();
                            },
                            function (response) {
                                helperService.handlerError(response);
                            }
                    );
                };
                getPersonalInformation();

            }]
    };
});