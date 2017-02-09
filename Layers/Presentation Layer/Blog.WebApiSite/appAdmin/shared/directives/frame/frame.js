angular.module("applicationAdminModule").directive('frame', function () {
    return {
        restrict: 'A', //This menas that it will be used as an attribute and NOT as an element. I don't like creating custom HTML elements
        replace: true,
        templateUrl: "/appAdmin/shared/directives/frame/frameView.html",
        controller: ['$scope', '$state', '$auth', 'helperService', function ($scope, $state, $auth, helperService,$rootScope) {

                $scope.currentView = '';

                $scope.logout = function () {

				// clear redirect variables
                    if ($rootScope != undefined) {

                        if ($rootScope.returnToState != undefined)
                            $rootScope.returnToState = "";

                        if ($rootScope.returnToStateParams != undefined)
                            $rootScope.returnToStateParams = "";
                    }

                    $auth.logout();
                    $state.go('login', {});
                };

                var activateView = function (viewName) {

                    $scope.currentView = viewName;
                };

                $scope.isActive = function (viewName) {

                    return $scope.currentView == viewName;
                };

                $scope.isAuthenticated = function () {

                    return $auth.isAuthenticated();
                };


                $scope.$on('activateViewEvent', function (event, args) {
                    activateView(args.view);
                });

                $scope.$on('activateMenuEvent', function (event, args) {
                    activateMenu();
                });

                var activateMenu = function () {
                    if ($auth.isAuthenticated()) {
                        getAdminUrls();
                    }
                };

                var getAdminUrls = function () {
               
                    $scope.adminUrls = [
										{ "icon":"fa-home", "value":"home", "url":"home", "name": "DashBoard","permission": "AUTHORIZED"},									
										{ "icon":"fa-edit", "value":"personalInformation", "url":"personalInformationList", "name":"Personal Information","permission": "AUTHORIZED"},
										{ "icon":"fa-edit", "value":"blogEntry", "url":"blogEntryList", "name":"BlogEntry","permission": "AUTHORIZED"},
										{ "icon":"fa-edit", "value":"tag", "url":"tagList", "name":"Tag","permission": "AUTHORIZED"},
										{ "icon":"fa-edit", "value":"user", "url":"userList", "name":"User","permission": "ADMIN"},
										{ "icon":"fa-edit", "value":"role", "url":"roleList", "name":"Role","permission": "ADMIN"} 										
					];
                            
                };
                
                activateMenu();
            }]
    };
});