angular.module("applicationAdminModule").controller("blogEntryDetailController", function (id, $scope, $state, helperService, blogEntryRepository,commonRepository) {

	$scope.model = {};
	$scope.model.id = id;
	$scope.model.tags = [];

	helperService.activateView('blogEntry');

	var getModel = function (idModel) {

		blogEntryRepository.getModel(idModel).then(
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