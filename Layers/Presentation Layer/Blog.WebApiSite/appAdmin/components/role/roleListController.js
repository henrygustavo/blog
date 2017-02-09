angular.module("applicationAdminModule").controller("roleListController", function ($scope, roleService,helperService) {

	helperService.activateView('role');

    roleService.iniData();
    roleService.find();

    $scope.data = roleService.data;

    $scope.search = function (model) {

        var name = (model != undefined) ? model.searchName : '';

        roleService.data.filterOptions.filterName = name;

		$scope.data.pagingOptions.currentPage = 1;

        roleService.find();

    };
    $scope.$watch('data.sortOptions.fields', function (newVal, oldVal) {

        if (newVal.length > 0 && newVal !== oldVal) {
            $scope.data.pagingOptions.currentPage = 1;
            roleService.find();
        }
    }, true);

    $scope.$watch('data.sortOptions.directions', function (newVal, oldVal) {

        if (newVal.length > 0 && newVal !== oldVal) {
            $scope.data.pagingOptions.currentPage = 1;
            roleService.find();
        }
    }, true);

    $scope.$watch('data.pagingOptions', function (newVal, oldVal) {
        if (newVal !== oldVal) {

			if (newVal.pageSize != oldVal.pageSize) {
                $scope.data.pagingOptions.currentPage = 1;
            }

            roleService.find();
        }
    }, true);

    var rowTemplate = '<div class="ngCellText" style="text-align:center"><a href="#/role/edit/{{row.entity.id}}" class="btn btn-xs btn-info" style="font-size:15px;margin-right:10px"><i class="glyphicon glyphicon-pencil"></i></a><a href="#/role/detail/{{row.entity.id}}" class="btn btn-xs btn-warning" style="font-size:15px"><i class="glyphicon glyphicon-eye-open"></i></a></div>';

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
        enableHighlighting: true,
        useExternalSorting: true,
        sortInfo: $scope.data.sortOptions,
        rowHeight: 50,
        columnDefs: [
                    { field: '', displayName: '', width: '70', sortable: false, cellTemplate: '<div class="ngCellText">{{row.rowIndex + 1}}</div>' },
                    { field: 'name', displayName: 'Name' },
                    { field: 'edit', displayName: '', width: '120', sortable: false, cellTemplate: rowTemplate }
        ]
    };
});
