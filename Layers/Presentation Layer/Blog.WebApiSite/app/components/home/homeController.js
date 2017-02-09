angular.module("applicationModule").controller('homeController', function (searchText,searchTag, $scope, blogEntryRepository, helperService) {

    helperService.activateView('home');

    $scope.filter = {};
    $scope.content = [];
    $scope.totalRecords = "0";
    $scope.currentPage = "1";
    $scope.totalPages = "0";

    $scope.renderHtml = function (html_code) {
        return helperService.renderHtml(html_code);
    };

    var getAllBlogEntries = function (filter) {

        $scope.filter.page = (filter.page == undefined) ? "1" : filter.page;
        $scope.filter.pageSize = (filter.pageSize == undefined) ? "5" : filter.pageSize;
        $scope.filter.sortBy = (filter.sortBy == undefined) ? "CreationDate" : filter.sortBy;
        $scope.filter.sortDirection = (filter.sortDirection == undefined) ? "desc" : filter.sortDirection;
        $scope.filter.content = (filter.searchText == undefined) ? "" : filter.searchText;
        $scope.filter.tags = (filter.searchTag == undefined) ? "" : filter.searchTag;

        var params = {
            filterHeader:'',
            content :$scope.filter.content,
            tags :$scope.filter.tags,
            page: $scope.filter.page,
            pageSize: $scope.filter.pageSize,
            sortBy: $scope.filter.sortBy,
            sortDirection: $scope.filter.sortDirection
           

        };

        blogEntryRepository.getAll(params).then(
                function (response) {
                    $scope.content = response.content;
                    $scope.totalRecords = response.totalRecords;
                    $scope.currentPage = response.currentPage;
                    $scope.totalPages = response.totalPages;
                },
                function (response) {
                    helperService.handlerError(response);
                }
        );
    };

    $scope.newerPage = function (movPrevious) {

        $scope.filter.page = movPrevious;
        getAllBlogEntries($scope.filter);
    };

    $scope.olderPage = function (movNext) {
        $scope.filter.page = movNext;

        getAllBlogEntries($scope.filter);
    };

    getAllBlogEntries({page: "1", searchText: searchText,searchTag:searchTag});
});
