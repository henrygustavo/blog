angular.module("applicationAdminModule").factory('personalInformationRepository', function ($http, $q, GlobalInfo) {
    return {
        getAll: function (params) {
            var deferred = $q.defer();
            $http.get(GlobalInfo.apiUrl + '/personalInformations', { params: params })
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

            $http.get(GlobalInfo.apiUrl + '/personalInformations/' + id)
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

            $http.post(GlobalInfo.apiUrl + '/personalInformations', model)
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

            $http.put(GlobalInfo.apiUrl + '/personalInformations/' + model.id, model)
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