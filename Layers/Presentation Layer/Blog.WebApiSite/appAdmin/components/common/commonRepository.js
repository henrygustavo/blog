angular.module("applicationAdminModule").factory('commonRepository',
    function($http, $q, GlobalInfo) {
        return {
            getStates: function() {
                var deferred = $q.defer();
                $http.get(GlobalInfo.apiUrl + '/commons/states')
                    .success(function(response) {
                        deferred.resolve(response);
                    })
                    .error(function(response) {
                        deferred.reject(response);
                    });

                return deferred.promise;
            },
            getSetting: function(idSetting) {
                var deferred = $q.defer();
                $http.get(GlobalInfo.apiUrl + '/commons/setting/' + idSetting)
                    .success(function(response) {
                        deferred.resolve(response);
                    })
                    .error(function(response) {
                        deferred.reject(response);
                    });

                return deferred.promise;
            },
            getFiles: function(idFolder) {
                var deferred = $q.defer();
                $http.get(GlobalInfo.apiUrl + '/files/idFolder/' + idFolder)
                    .success(function(response) {
                        deferred.resolve(response);
                    })
                    .error(function(response) {
                        deferred.reject(response);
                    });

                return deferred.promise;
            },
            getFile: function(idFile) {
                var deferred = $q.defer();
                $http.get(GlobalInfo.apiUrl + '/files/' + idFile)
                    .success(function(response) {
                        deferred.resolve(response);
                    })
                    .error(function(response) {
                        deferred.reject(response);
                    });

                return deferred.promise;
            },    
            getFilesFolder: function () {
                var deferred = $q.defer();
                $http.get(GlobalInfo.apiUrl + '/folders/content/files')
                    .success(function (response) {
                        deferred.resolve(response);
                    })
                    .error(function (response) {
                        deferred.reject(response);
                    });

                return deferred.promise;
            },
            getImagesFolder: function () {
                var deferred = $q.defer();
                $http.get(GlobalInfo.apiUrl + '/folders/content/images')
                    .success(function (response) {
                        deferred.resolve(response);
                    })
                    .error(function (response) {
                        deferred.reject(response);
                    });

                return deferred.promise;
            },
            getImageProfile: function() {
                var deferred = $q.defer();
                $http.get(GlobalInfo.apiUrl + '/files/images/profile')
                    .success(function(response) {
                        deferred.resolve(response);
                    })
                    .error(function(response) {
                        deferred.reject(response);
                    });

                return deferred.promise;
            }
        };
    });