angular.module("applicationAdminModule").factory('accountRepository', function($http, $q, GlobalInfo) {
    return {
        create: function (model) {
            var deferred = $q.defer();

            $http.post(GlobalInfo.apiUrl + '/Account/Register', model)
                .success(function(response) {
                    deferred.resolve(response);
                })
                .error(function(response) {
                    deferred.reject(response);
                });

            return deferred.promise;
        }, 
        login: function (model) {
            var deferred = $q.defer();

            $http.post(GlobalInfo.apiUrl + '/Account/Login', model)
                .success(function (response) {
                    deferred.resolve(response);
                })
                .error(function (response) {
                    deferred.reject(response);
                });

            return deferred.promise;
        },
        registerExternal: function (model) {
            var deferred = $q.defer();

            $http.post(GlobalInfo.apiUrl + '/Account/RegisterExternal', model)
                .success(function (response) {
                    deferred.resolve(response);
                })
                .error(function (response) {
                    deferred.reject(response);
                });

            return deferred.promise;
        },
        changePassowrd: function (model) {

            var deferred = $q.defer();

            $http.post(GlobalInfo.apiUrl + '/Account/ChangePassword', model)
                .success(function (response) {
                    deferred.resolve(response);
                })
                .error(function (response) {
                    deferred.reject(response);
                });

            return deferred.promise;
        },
        forgotPassword: function (model) {

            var deferred = $q.defer();

            $http.post(GlobalInfo.apiUrl + '/Account/ForgotPassword', model)
                .success(function (response) {
                    deferred.resolve(response);
                })
                .error(function (response) {
                    deferred.reject(response);
                });

            return deferred.promise;
        },
        resetPassword: function (model) {

            var deferred = $q.defer();
            $http.post(GlobalInfo.apiUrl + '/Account/ResetPassword', model)
                .success(function (response) {
                    deferred.resolve(response);
                })
                .error(function (response) {
                    deferred.reject(response);
                });

            return deferred.promise;
        },
		verificationToken: function (id,token) {

            var deferred = $q.defer();

            $http.get(GlobalInfo.apiUrl + '/Account/ConfirmEmail?idUser=' + id + '&code=' + token)
                    .success(function (response) {
                        deferred.resolve(response);
                    })
                    .error(function (response) {
                        deferred.reject(response);
                    });

                return deferred.promise;
            }
    };
});