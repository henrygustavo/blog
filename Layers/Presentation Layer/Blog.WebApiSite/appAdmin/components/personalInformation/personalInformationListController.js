angular.module("applicationAdminModule").controller("personalInformationListController", function ($scope, personalInformationService,helperService) {

	helperService.activateView('personalInformation');

	personalInformationService.iniData();
	personalInformationService.find();

	$scope.data = personalInformationService.data;

	$scope.search = function (model) {

		$scope.data.pagingOptions.currentPage = 1;
	};

	$scope.$watch('data.sortOptions.fields', function (newVal, oldVal) {

		if (newVal.length > 0 &&newVal !== oldVal) {
			$scope.data.pagingOptions.currentPage = 1;
			personalInformationService.find();
		}
	}, true);

	$scope.$watch('data.sortOptions.directions', function (newVal, oldVal) {

		if (newVal.length > 0 &&newVal !== oldVal) {
			$scope.data.pagingOptions.currentPage = 1;
			personalInformationService.find();
		}
	}, true);

	$scope.$watch('data.pagingOptions', function (newVal, oldVal) {
		if (newVal !== oldVal) {
		if (newVal.pageSize != oldVal.pageSize) {
                $scope.data.pagingOptions.currentPage = 1;
            }

			personalInformationService.find();
		}
	}, true);

	$scope.hasItem = function () {

	    return $scope.data.entities.totalRecords;
	};

	var rowTemplate = '<div class="ngCellText" style="text-align:center"><a href="#/personalInformation/edit/{{row.entity.id}}" class="btn btn-xs btn-info" style="font-size:15px;margin-right:10px"><i class="glyphicon glyphicon-pencil"></i></a><a href="#/personalInformation/detail/{{row.entity.id}}" class="btn btn-xs btn-warning" style="font-size:15px"><i class="glyphicon glyphicon-eye-open"></i></a></div>';

	$scope.ngGridView = {
		data: 'data.entities.content',
		showFilter: false,
		multiSelect: false,
		enablePaging: true,
		showFooter: true,
		totalServerItems: 'data.entities.totalRecords',
		pagingOptions: $scope.data.pagingOptions,
		filterOptions: $scope.data.filterOptions,
		useExternalSorting: true,
		i18n: 'en',
		enableHighlighting:true,
		rowHeight: 50,
		sortInfo: $scope.data.sortOptions,
		columnDefs: [
					    { field: '', displayName: '', width: '70', sortable: false, cellTemplate: '<div class="ngCellText">{{row.rowIndex + 1}}</div>' },
                        { field: 'siteName', displayName: 'Site Name', width: '600' },
                        { field: 'firstName', displayName: 'First Name', width: '600' },
                        { field: 'edit', displayName: '', width: '120', sortable: false, cellTemplate: rowTemplate }
		]
	};
});