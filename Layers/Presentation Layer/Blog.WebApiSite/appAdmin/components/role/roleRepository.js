angular.module("applicationAdminModule").factory('roleRepository', function ($http, $q, GlobalInfo) {
     return {
        getAll: function (params) {
            var deferred = $q.defer();
            $http.get(GlobalInfo.apiUrl + '/roles', {
                params: params
            })
                .success(function (response) {
                    deferred.resolve(response);
                })
                .error(function (response) {
                    deferred.reject(response);
                });

            return deferred.promise;
        },
        getAllList: function () {
            var deferred = $q.defer();
            $http.get(GlobalInfo.apiUrl + '/roles/state/active')
                .success(function (response) {
                    deferred.resolve(response);
                })
                .error(function (response) {
                    deferred.reject(response);
                });

            return deferred.promise;
        },
        getModel: function (id) {
            var deferred = $q.defer();

            $http.get(GlobalInfo.apiUrl + '/roles/' + id)
                .success(function (response) {
                    deferred.resolve(response);
                })
                .error(function (response) {
                    deferred.reject(response);
                });

            return deferred.promise;
        },
        insert: function (model) {
            var deferred = $q.defer();

            $http.post(GlobalInfo.apiUrl + '/roles', model)
                .success(function (response) {
                    deferred.resolve(response);
                })
                .error(function (response) {
                    deferred.reject(response);
                });

            return deferred.promise;
        },
        update: function (model) {
            var deferred = $q.defer();

            $http.put(GlobalInfo.apiUrl + '/roles/' + model.id, model)
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