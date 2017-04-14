angular.module("applicationModule").controller('blogEntryController',
    function(headerUrl, $scope, blogEntryRepository, tagRepository, helperService, $state) {
      
        $scope.model = {};
        $scope.post = {};

        $scope.renderHtml = function(html_code) {
            return helperService.renderHtml(html_code);
        };

        $scope.searchTags = function(searchTag) {

            $state.go('searchTags', { searchTag: searchTag });
        };

        $scope.addComment = function(model) {
            model.idBlogEntry = $scope.post.id;
            blogEntryRepository.addComment(model).then(
                function(response) {

                    if (response.success) {
                        $scope.post.comments.push(response.data);
                        helperService.showAlert(response.message, "success");
                    }

                    $scope.model = {};
                    $scope.form.$setPristine();
                },
                function(response) {
                    helperService.handlerError(response);
                }
            );
        };

        var getModel = function(headerUrl) {
            blogEntryRepository.getblogEntryByHeaderUrl(headerUrl).then(
                function(response) {
                    $scope.post = response;
                },
                function(response) {
                    helperService.handlerError(response);
                }
            );
        };

        var initialLoad = function() {

            getModel(headerUrl);
        };

        initialLoad();
        

    });

