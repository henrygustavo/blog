angular.module("applicationAdminModule").controller("userEditController", function (id, $scope, $state, helperService, roleRepository, userRepository, GlobalInfo) {

    $scope.model = {};
    $scope.model.id = id;
    $scope.model.confirmUrl = GlobalInfo.confirmUrl;

	helperService.activateView('user');

	$scope.save = function (model) {

	    if (model.id != 0) {
	        update(model);
	    } else {
	        insert(model);
	    }
	};

	var insert = function (model) {

	    userRepository.insert(model).then(
            function (response) {
                helperService.showAlertResponse(response);
                $state.go('userList');
            },
            function (response) {
                helperService.handlerError(response);
            }
        );
	};

	var update = function (model) {

	    userRepository.update(model).then(
            function (response) {
                helperService.showAlertResponse(response);
                $state.go('userList');
            },
            function (response) {
                helperService.handlerError(response);
            }
        );
	};

    var getRoles = function (value) {

        roleRepository.getAllList().then(
           function (response) {
               $scope.model.roles = response;
               $scope.model.idRole = value;
           },
           function (response) {

               helperService.handlerError(response);
           }
       );
    };

    var getModel = function(idModel) {

        userRepository.getModel(idModel).then(
            function (response) {
                $scope.model.userName = response.userName;
                $scope.model.email = response.email;
                getRoles(response.idRole.toString());
                $scope.model.lockoutEnabled = response.lockoutEnabled;
                $scope.model.disabled = response.disabled;
            },
            function (response) {
                helperService.handlerError(response);
            }
        );     
    };

    var initialLoad = function() {

        if (id != 0) {

            getModel(id);
        } else {
          
            getRoles("0");
        }
    };

    initialLoad();

});