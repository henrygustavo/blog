angular.module("applicationModule").factory('tagRepository', function ($http, $q, GlobalInfo) {
    return {
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
        }
    };
});