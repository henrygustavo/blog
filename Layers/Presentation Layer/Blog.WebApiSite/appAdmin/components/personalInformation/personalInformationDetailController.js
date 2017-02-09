angular.module("applicationAdminModule").controller("personalInformationDetailController", function (id, $scope, $state, helperService, personalInformationRepository, commonRepository, Lightbox) {

	$scope.model = {};
	$scope.model.id = id;

	$scope.openLightboxModal = function (imagenes, index) {
	    Lightbox.openModal(imagenes, index);
	};

	helperService.activateView('personalInformation');

	var getModel = function (idModel) {

		personalInformationRepository.getModel(idModel).then(
			function (response) {
			    $scope.model = response;
			    getImageProfile($scope.model.idPhoto);
			},
			function (response) {
				helperService.handlerError(response);
			}
		);
	};

	var getImageProfile = function (idPhoto) {

	    commonRepository.getFile(idPhoto).then(
                function (response) {

                    $scope.model.imagesProfile = [];

                    $scope.model.imagesProfile.push(response);
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