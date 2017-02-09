angular.module("applicationAdminModule").controller("tagEditController", function (id, $scope, $state, helperService, tagRepository,commonRepository) {

	$scope.model = {};
    $scope.model.id = id;

	helperService.activateView('tag');


	$scope.save = function (model) {

		 if (model.id != 0) {
	        update(model);
	    } else {
	        insert(model);
	    }
	};

	var insert = function (model) {

	    tagRepository.insert(model).then(
            function (response) {
                helperService.showAlertResponse(response);
                $state.go('tagList');
            },
            function (response) {
                helperService.handlerError(response);
            }
        );
	};

	var update = function (model) {

	    tagRepository.update(model).then(
            function (response) {
                helperService.showAlertResponse(response);
                $state.go('tagList');
            },
            function (response) {
                helperService.handlerError(response);
            }
        );
	};

	var getModel = function (idModel) {

		tagRepository.getModel(idModel).then(
			function (response) {
				$scope.model = response;
				getStates(response.state.toString());
			},
			function (response) {
				helperService.handlerError(response);
			}
		);
	};


	var getStates = function (value) {

	    commonRepository.getStates().then(
		   function (response) {
		       $scope.model.states = response;

		       $scope.model.state = value;
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
		else{
			getStates('0');
		}
	};

	initialLoad();
});