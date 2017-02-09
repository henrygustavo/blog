angular.module("applicationModule").directive('sidebar', function () {
    return {
        restrict: 'A', //This menas that it will be used as an attribute and NOT as an element. I don't like creating custom HTML elements
        replace: true,
        templateUrl: "/app/shared/directives/sideBar/sideBarView.html",
        controller: ['$scope', '$filter', 'tagRepository', 'helperService', '$state', function ($scope, $filter, tagRepository, helperService, $state) {
                $scope.model = {};
                $scope.tagsList = [];
                $scope.searchText = "";

                var getTags = function () {

                    tagRepository.getTags().then(
                            function (response) {

                                $scope.tagsList = response;
                            },
                            function (response) {
                                helperService.handlerError(response);
                            }
                    );
                };

                $scope.search = function (searchText) {

                    $state.go('search', {searchText: searchText});
                };
                
                $scope.searchTags = function (searchTag) {

                    $state.go('searchTags', {searchTag: searchTag});
                };

                getTags();
            }]
    };
});