angular.module("applicationAdminModule").controller("tagDetailController", function (id, $scope, $state, helperService, tagRepository,commonRepository) {

	$scope.model = {};
	$scope.model.id = id;

	helperService.activateView('tag');

	var getModel = function (idModel) {

		tagRepository.getModel(idModel).then(
			function (response) {
				$scope.model = response;
				getSetting(response.state);
							},
			function (response) {
				helperService.handlerError(response);
			}
		);
	};


	var getSetting = function (value) {
	    commonRepository.getSetting(value).then(
			function (response) {
			   $scope.model.stateName = response.name;
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