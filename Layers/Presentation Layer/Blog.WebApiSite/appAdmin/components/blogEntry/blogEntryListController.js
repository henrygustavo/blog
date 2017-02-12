angular.module("applicationAdminModule").controller("blogEntryListController", function ($scope, blogEntryService,helperService) {

	helperService.activateView('blogEntry');

	blogEntryService.iniData();
	blogEntryService.find();

	$scope.data = blogEntryService.data;

	$scope.search = function (model) {

	    var header = (model != undefined) ? model.searchHeader : '';

	    blogEntryService.data.filterOptions.filterHeader = header;
	    $scope.data.pagingOptions.currentPage = 1;
	    blogEntryService.find();
	};

	$scope.$watch('data.sortOptions.fields', function (newVal, oldVal) {

		if (newVal.length > 0 &&newVal !== oldVal) {
			$scope.data.pagingOptions.currentPage = 1;
			blogEntryService.find();
		}
	}, true);

	$scope.$watch('data.sortOptions.directions', function (newVal, oldVal) {

		if (newVal.length > 0 &&newVal !== oldVal) {
			$scope.data.pagingOptions.currentPage = 1;
			blogEntryService.find();
		}
	}, true);

	$scope.$watch('data.pagingOptions', function (newVal, oldVal) {
		if (newVal !== oldVal) {
		if (newVal.pageSize != oldVal.pageSize) {
                $scope.data.pagingOptions.currentPage = 1;
            }

			blogEntryService.find();
		}
	}, true);

	var rowTemplate = '<div class="ngCellText" style="text-align:center"><a href="#/blogEntry/edit/{{row.entity.id}}" class="btn btn-xs btn-info" style="font-size:15px;margin-right:10px"><i class="glyphicon glyphicon-pencil"></i></a><a href="#/blogEntry/detail/{{row.entity.id}}" class="btn btn-xs btn-warning" style="font-size:15px"><i class="glyphicon glyphicon-eye-open"></i></a></div>';

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

					{ field: 'header', displayName: 'Header'},
					{ field: 'author', displayName: 'Author', width: '100' },
                    { field: 'creationDate', displayName: 'CreationDate', width: '200', cellFilter: "date:'MM/dd/yyyy h:mma'" },
                    { field: 'tags', displayName: 'Tags', width: '200' },
					{ field: 'totalComments', displayName: 'Total Comments', width: '150' },
					{ field: 'edit', displayName: '', width: '120', sortable: false, cellTemplate: rowTemplate }
		]
	};
});