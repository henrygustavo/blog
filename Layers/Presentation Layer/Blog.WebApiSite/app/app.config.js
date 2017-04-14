var localHostUrl = window.location.origin || window.location.protocol + "//" + window.location.host;

var applicationModule = angular.module("applicationModule",
    [
        "ui.router",
        "ngResource",
        "ngRoute",
        "ngAnimate",
        "ui.bootstrap",
        "blockUI",
        "ngMessages",
        "ngSanitize",
        "ngEnlighter",
        "angular-input-text"
    ]);

applicationModule.config(function($urlRouterProvider,
        $stateProvider,
        $locationProvider,
        GlobalInfo,
        blockUIConfig,
        inputTextConfigProvider) {

        blockUIConfig.templateUrl = "/app/shared/html/block-ui-overlay.html";
        inputTextConfigProvider.setValidationMessagePath("/app/shared/html/validation-messages.html");

        $urlRouterProvider.otherwise("/");

        $stateProvider
            .state("home",
                {
                    url: "/",
                    templateUrl: "/app/components/home/homeView.html",
                    controller: "homeController",
                    resolve: {
                        searchText: function($stateParams) {
                            return $stateParams.searchText;
                        },
                        searchTag: function($stateParams) {
                            return $stateParams.searchTag;
                        }
                    }
                })
            .state("contact",
                {
                    url: "/contact",
                    templateUrl: "/app/components/contact/contactView.html",
                    controller: "contactController"
                })
            .state("about",
                {
                    url: "/about",
                    templateUrl: "/app/components/about/aboutView.html",
                    controller: "aboutController"
                })
            .state("search",
                {
                    url: "/search/:searchText",
                    templateUrl: "/app/components/home/homeView.html",
                    controller: "homeController",
                    resolve: {
                        searchText: function($stateParams) {
                            return $stateParams.searchText;
                        },
                        searchTag: function($stateParams) {
                            return $stateParams.searchTag;
                        }
                    }
                })
            .state("searchTags",
                {
                    url: "/searchTags/:searchTag",
                    templateUrl: "/app/components/home/homeView.html",
                    controller: "homeController",
                    resolve: {
                        searchText: function($stateParams) {
                            return $stateParams.searchText;
                        },
                        searchTag: function($stateParams) {
                            return $stateParams.searchTag;
                        }
                    }
                })
            .state("blogEntry",
                {
                    url: "/blogEntry/:headerUrl",
                    templateUrl: "/app/components/blogEntry/blogEntryView.html",
                    controller: "blogEntryController",
                    resolve: {
                        headerUrl: function($stateParams) {
                            return $stateParams.headerUrl;
                        }
                    }
                });
    })
    .constant("GlobalInfo",
        {
            apiUrl: localHostUrl + "/api",
            localHostUrl: localHostUrl
        });