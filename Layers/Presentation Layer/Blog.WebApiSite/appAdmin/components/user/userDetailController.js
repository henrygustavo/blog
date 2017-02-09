angular.module("applicationAdminModule").controller("userDetailController", function (id, $scope, $state,$filter, helperService, roleRepository, userRepository) {

    $scope.model = {};
    $scope.model.id = id;
    
	helperService.activateView('user');

    var getRole = function (idRole) {
        roleRepository.getModel(idRole).then(
            function (response) {
                $scope.model.role = response.name;
            },
            function (response) {
                helperService.handlerError(response);
            }
        );
    };

    var getModel = function (idModel) {
    
        userRepository.getModel(idModel).then(
            function (response) {
                $scope.model.userName = response.userName;
                $scope.model.email = response.email;
                getRole(response.idRole);
                $scope.model.lockoutEnabled = response.lockoutEnabled;
                $scope.model.disabled = response.disabled;
            },
            function (response) {
                helperService.handlerError(response);
            }
        );     
    };

    var initialLoad = function () {
        
        getModel(id);
    };

    initialLoad();
});
