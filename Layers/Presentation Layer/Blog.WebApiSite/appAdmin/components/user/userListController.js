angular.module("applicationAdminModule").controller("userListController", function ($scope, userService,helperService) {
 
    userService.iniData();
    userService.find();
    $scope.data = userService.data;

	helperService.activateView('user');

    $scope.search = function(model) {

        var name = (model.searchUserName  != undefined) ? model.searchUserName : '';
        var email = (model.searchEmail != undefined) ? model.searchEmail : '';

        userService.data.filterOptions.filterName = name;
        userService.data.filterOptions.filterEmail = email;
        $scope.data.pagingOptions.currentPage = 1;
        userService.find();
        
    };
    $scope.$watch('data.sortOptions.fields', function (newVal, oldVal) {

        if (newVal.length > 0 && newVal !== oldVal) {
            $scope.data.pagingOptions.currentPage = 1;
            userService.find();
        }
    }, true);
    
    $scope.$watch('data.sortOptions.directions', function (newVal, oldVal) {

        if (newVal.length > 0 && newVal !== oldVal) {
            $scope.data.pagingOptions.currentPage = 1;
            userService.find();
        }
    }, true);

    $scope.$watch('data.pagingOptions', function (newVal, oldVal) {
        if (newVal !== oldVal) {

		if (newVal.pageSize != oldVal.pageSize) {
                $scope.data.pagingOptions.currentPage = 1;
            }
            userService.find();
        }
    }, true);

    var rowTemplate = '<div class="ngCellText" style="text-align:center"><a href="#/user/edit/{{row.entity.id}}" class="btn btn-xs btn-info" style="font-size:15px;margin-right:10px"><i class="glyphicon glyphicon-pencil"></i></a><a href="#/user/detail/{{row.entity.id}}" class="btn btn-xs btn-warning" style="font-size:15px"><i class="glyphicon glyphicon-eye-open"></i></a></div>';
    $scope.ngGridView = {
        data: 'data.entities.content',
        showFilter: false,
        multiSelect: false,
        enablePaging: true,
        showFooter: true,
        i18n: 'en',
        totalServerItems: 'data.entities.totalRecords',
        pagingOptions: $scope.data.pagingOptions,
        filterOptions: $scope.data.filterOptions,
        useExternalSorting: true,
        enableHighlighting: true,
        sortInfo: $scope.data.sortOptions,
        rowHeight: 50,
        columnDefs: [
                    { field: '', displayName: '', width: '70', sortable: false, cellTemplate: '<div class="ngCellText">{{row.rowIndex + 1}}</div>' },
                    { field: 'userName', displayName: 'User Name' },
                    { field: 'roleName', displayName: 'Role', width: '100' },
                    { field: 'email', displayName: 'Email' },
                    { field: 'disabled', displayName: 'Disabled', width: '100' },
                    { field: 'lastActivityDate', displayName: 'Ultima actualizacion', cellFilter: 'date:\'MM/dd/yyyy HH:MM:ss\'', width: '200'},
                    { field: 'edit', displayName: '', width: '120', sortable: false, cellTemplate: rowTemplate }
        ]
    };
});
