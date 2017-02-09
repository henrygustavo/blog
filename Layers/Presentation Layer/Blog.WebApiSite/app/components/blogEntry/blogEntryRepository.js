angular.module("applicationModule").factory('blogEntryRepository', function ($http, $q, GlobalInfo) {
    return {
        getAll: function (params) {
            var deferred = $q.defer();
            $http.get(GlobalInfo.apiUrl + '/blogEntries/', {params: params})
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

            $http.get(GlobalInfo.apiUrl + '/blogEntries/' + id)
                    .success(function (response) {
                        deferred.resolve(response);
                    })
                    .error(function (response) {
                        deferred.reject(response);
                    });

            return deferred.promise;
        },
        getblogEntryByHeaderUrl: function (headerUrl) {
            var deferred = $q.defer();

            $http.get(GlobalInfo.apiUrl + '/blogEntries/headerUrl/' + headerUrl)
                    .success(function (response) {
                        deferred.resolve(response);
                    })
                    .error(function (response) {
                        deferred.reject(response);
                    });

            return deferred.promise;
        },
        addComment: function (model) {
            var deferred = $q.defer();

            $http.post(GlobalInfo.apiUrl + '/blogEntries/comment', model)
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