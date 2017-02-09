angular.module("applicationAdminModule").controller("personalInformationEditController", function (id, $scope, $state, helperService, personalInformationRepository, commonRepository, Lightbox) {

	$scope.model = {};
    $scope.model.id = id;

	helperService.activateView('personalInformation');

	$scope.openLightboxModal = function (imagenes, index) {
	    Lightbox.openModal(imagenes, index);
	};

	$scope.save = function (model) {

		 if (model.id != 0) {
	        update(model);
	    } else {
	        insert(model);
	    }
	};

	var insert = function (model) {

	    personalInformationRepository.insert(model).then(
            function (response) {
                helperService.showAlertResponse(response);
                $state.go('personalInformationList');
            },
            function (response) {
                helperService.handlerError(response);
            }
        );
	};

	var update = function (model) {

	    personalInformationRepository.update(model).then(
            function (response) {
                helperService.showAlertResponse(response);
                $state.go('personalInformationList');
            },
            function (response) {
                helperService.handlerError(response);
            }
        );
	};

	var getModel = function (idModel) {

		personalInformationRepository.getModel(idModel).then(
			function (response) {
				$scope.model = response;
			},
			function (response) {
				helperService.handlerError(response);
			}
		);
	};

	var getImageProfile = function () {

	    commonRepository.getImageProfile().then(
                function (response) {

                    $scope.model.imagesProfile = response;
                    if (response != undefined && response.length > 0) {
                        $scope.model.idPhoto = response[0].idStorage;
                    }
                },
                function (response) {
                    helperService.handlerError(response);
                }
        );
	};

	var initialLoad = function () {
		
		 if (id != 0){
			getModel(id);
		 }

		 getImageProfile();
	};

	initialLoad();
});