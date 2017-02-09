angular.module("applicationAdminModule").controller("blogEntryEditController", function (id, $scope, $state, helperService, blogEntryRepository, commonRepository, tagRepository, Lightbox) {

    $scope.model = {};
    $scope.model.id = id;
    $scope.model.author = "admin";
    $scope.model.tags = [];
    $scope.tagsList = [];
    $scope.currentImageFolder = '';
    $scope.currentFolder = '';

	helperService.activateView('blogEntry');

	$scope.isActive = function (headerName, type) {

	    switch (type) {
	        case "I":
	            return $scope.currentImageFolder == headerName;
	        case "F":
	            return $scope.currentFolder == headerName;
	    }
	}

	$scope.loadTags = function (query) {

	    return $scope.tagsList.filter(function (tag) {
	        return tag.name.toLowerCase().indexOf(query.toLowerCase()) != -1;
	    });
	};

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

	$scope.getFiles = function (idFolder) {
	    $scope.currentFileFolder = idFolder;
	    commonRepository.getFiles(idFolder).then(
                function (response) {

                    $scope.files = response;
                },
                function (response) {
                    helperService.handlerError(response);
                }
        );
	};

	$scope.getImages = function (idFolder) {
	    $scope.currentImageFolder = idFolder;
	    commonRepository.getFiles(idFolder).then(
                function (response) {

                    $scope.images = response;
                },
                function (response) {
                    helperService.handlerError(response);
                }
        );
	};

	var getImageFolder = function () {

	    commonRepository.getImagesFolder().then(
                function (response) {

                    $scope.imagesFolder = response;
                },
                function (response) {
                    helperService.handlerError(response);
                }
        );
	};

	var getFilesFolder = function () {

	    commonRepository.getFilesFolder().then(
                function (response) {

                    $scope.filesfolder = response;
                },
                function (response) {
                    helperService.handlerError(response);
                }
        );
	};

	var insert = function (model) {

	    blogEntryRepository.insert(model).then(
            function (response) {
                helperService.showAlertResponse(response);
                $state.go('blogEntryList');
            },
            function (response) {
                helperService.handlerError(response);
            }
        );
	};

	var update = function (model) {

	    blogEntryRepository.update(model).then(
            function (response) {
                helperService.showAlertResponse(response);
                $state.go('blogEntryList');
            },
            function (response) {
                helperService.handlerError(response);
            }
        );
	};

	var getModel = function (idModel) {

		blogEntryRepository.getModel(idModel).then(
			function (response) {
				$scope.model = response;
				getStates(response.state.toString());
			},
			function (response) {
				helperService.handlerError(response);
			}
		);
	};

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

	     getTags();
		 getImageFolder();
		 getFilesFolder();
	};

	initialLoad();
});