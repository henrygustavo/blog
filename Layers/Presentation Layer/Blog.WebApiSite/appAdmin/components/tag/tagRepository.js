angular.module("applicationAdminModule").factory('tagRepository', function ($http, $q, GlobalInfo) {
    return {
        getAll: function (params) {
            var deferred = $q.defer();
            $http.get(GlobalInfo.apiUrl + '/tags', { params: params })
                .success(function (response) {
                    deferred.resolve(response);
                })
                .error(function (response) {
                    deferred.reject(response);
                });

            return deferred.promise;
        },
        getTags: function () {
            var deferred = $q.defer();
            $http.get(GlobalInfo.apiUrl + '/tags/state/active')
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

            $http.get(GlobalInfo.apiUrl + '/tags/' + id)
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

            $http.post(GlobalInfo.apiUrl + '/tags', model)
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

            $http.put(GlobalInfo.apiUrl + '/tags/' + model.id, model)
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