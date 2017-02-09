angular.module("applicationAdminModule").factory('userService', function ($http, $q, $log, $rootScope, userRepository, helperService) {

    var service = {
        iniData: function () {
            var data =  {
                currententity: {},
                entities: [],
                selected: [],
                totalPages: 0,

                filterOptions: {
                    filterEmail: '',
                    filterName: '',
                    externalFilter: '',
                    useExternalFilter: true
                },
                sortOptions: {
                    fields: ["id"],
                    directions: ["desc"]
                },
                pagingOptions: {
                    pageSizes: [10, 20, 50, 100],
                    pageSize: "10",
                    currentPage: 1
                }
            }

            service.data = data;
        },

        find: function() {
            var params = {
                filterEmail: service.data.filterOptions.filterEmail,
                filterName: service.data.filterOptions.filterName,
                page: service.data.pagingOptions.currentPage,
                pageSize: service.data.pagingOptions.pageSize,
                sortBy: service.data.sortOptions.fields[0],
                sortDirection: service.data.sortOptions.directions[0]
            };

            userRepository.getAll(params).then(
               function (response) {
                   service.data.entities = response;
               },
               function (response) {

                   helperService.handlerError(response);
               }
            );
        }
    };
    return service;

});