angular.module("applicationAdminModule").controller("roleDetailController", function (id, $scope, $state, helperService, roleRepository) {

    $scope.model = {};
    $scope.model.id = id;

	helperService.activateView('role');

    var getModel = function (idModel) {
        
        roleRepository.getModel(idModel).then(
            function (response) {
                $scope.model.name = response.name;
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