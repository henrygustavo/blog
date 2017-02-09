angular.module("applicationAdminModule").factory('helperService', function ($rootScope, $timeout, $sce) {
	toastr.options = {        
		"positionClass": "toast-top-center"
	};
	
	 var activateViewFn = function (view) {

		$rootScope.$broadcast('activateViewEvent', {view: view});
	};
	
	var activateMenuFn = function () {

		$rootScope.$broadcast('activateMenuEvent', {});
	};

	return {
		handlerError: function (err) {
			toastr.error(getErrorMessages(err));
		},
		showAlertResponse: function (response) {

		    if (response.success) {
		        toastr.success(response.message);
		    } else {

		        toastr.error(response.Mmssage);
		    }
		},
		showAlert: function (message, className) {

			switch (className.toLowerCase()) {
				case "error":
					toastr.error(message);
					break;   
				case "success":
					toastr.success(message);
					break;
				case "info":
					toastr.info(message);
					break;
				case "warning":
					toastr.warning(message);
					break;     
			}
		},
		activateView: function (view) {

			$timeout(function () {
				activateViewFn(view);
			}, 300);
		},
		activateMenu: function () {

			$timeout(function () {
				activateMenuFn();
			}, 300);
		},
		renderHtml: function (html_code) {
			return $sce.trustAsHtml(html_code);
		}
	};
});

function getErrorMessages(errorResponse) {

	if (errorResponse == null)
		return "error";

	var errorMessage = '';

	if (errorResponse.statusText != undefined)
		errorMessage += errorResponse.statusText + "<br/>";

	if (errorResponse.message != undefined)
		errorMessage += errorResponse.message + "<br/>";

	if (errorResponse.exceptionMessage != undefined)
		errorMessage += errorResponse.exceptionMessage + "<br/>";

	for (var key in errorResponse.modelState) {
		for (var i = 0; i < errorResponse.modelState[key].length; i++) {
			errorMessage += errorResponse.modelState[key][i] + "<br/>";
		}
	}
	
	if (errorResponse.data != undefined && errorResponse.data.modelState != undefined) {
		
		for (var keymodel in errorResponse.data.modelState) {
			for (var j = 0; j < errorResponse.data.modelState[keymodel].length; j++) {
				errorMessage += errorResponse.data.modelState[keymodel][j] + "<br/>";
			}
		}
	}

	if (errorResponse.data != undefined && errorResponse.data.modelState == undefined) {

		errorMessage += errorResponse.data + "<br/>";
	}

	if (errorMessage == "") {

		if (errorResponse.length > 0) {
			errorMessage = errorResponse;
		} else {
			errorMessage = "Por favor intente más tarde... un error ha ocurrido";
		}
	}
	
	return errorMessage;
}