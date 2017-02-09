'use strict';

angular.module("applicationAdminModule").factory('authManager', function ($auth) {

    var authManager = {

        isInRole: function (role) {

            if ($auth.isAuthenticated()) {

                var payload = $auth.getPayload();
           
                if(payload.role != undefined) {

                    return (payload.role.indexOf(role) > -1);

                } else {
                    return false;
                }

            } else {
                return false;
            }
        },
        isAnonymous: function () {
            return !$auth.isAuthenticated();
        }
    };

    return authManager;
});
