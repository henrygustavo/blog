var localHostUrl = window.location.origin || window.location.protocol + '//' + window.location.host;

var applicationModule = angular.module("applicationAdminModule", ['ui.router', 'ngMessages', 'satellizer', 'permission', 'permission.ui', 'blockUI', 'ui.bootstrap', 'ui.mask', 'fcsa-number', 'ngGrid', 'textAngular', 'bootstrapLightbox', 'ngTagsInput', 'angucomplete-alt']);

applicationModule.config(function ($urlRouterProvider, $stateProvider, $locationProvider, $authProvider, GlobalInfo, blockUIConfig) {
	   
	$urlRouterProvider.otherwise('/');

	$stateProvider
	.state('home', {
			url: '/',
			templateUrl: '/appAdmin/components/home/homeView.html',
			controller: 'homeController',
			authenticate: true,
			data: {	permissions: {only: ['AUTHORIZED'],redirectTo: 'login'	}
		}
	})
	.state('login', {
			url: '/login',
			templateUrl: '/appAdmin/components/account/accountLoginView.html',
			controller: 'accountLoginController',
			data: {	permissions: {only: ['ANONYMOUS'] } }
	 })
	 .state('changepw', {
			url: '/changepassword',
			templateUrl: '/appAdmin/components/account/accountChangePasswordView.html',
			controller: 'accountChangePasswordController',
			authenticate: true,
			data: {	permissions: {only: ['AUTHORIZED'],redirectTo: 'login'}	}
			  })
	 .state('accessDenied', {
			url: '/accessDenied',
			templateUrl: '/appAdmin/components/accessDenied/accessDeniedView.html',
			controller: 'accessDeniedController',
			authenticate: true,
			data: {	permissions: {only: ['AUTHORIZED'],redirectTo: 'login'	}
		  }	  
	  })
	 .state('forgotpw', {
			url: '/forgotpassword',
			templateUrl: '/appAdmin/components/account/accountForgotPasswordView.html',
			controller: 'accountForgotPasswordController',
			data: {	permissions: {only: ['ANONYMOUS']	}
		  }
	  })
	.state('resetpw', {
	    url: '/resetpassword/{token:any}',
		templateUrl: '/appAdmin/components/account/accountResetPasswordView.html',
		controller: 'accountResetPasswordController',
		data: { permissions: { only: ['ANONYMOUS'] } },
		resolve: {
					token: function ($stateParams) {
							return $stateParams.token;
				}
		}
	})
	.state('verificationToken', {
	    url: '/verificationToken/:id/{token:any}',
		templateUrl: '/appAdmin/components/account/accountVerificationTokenView.html',
		controller: 'accountVerificationTokenController',
		data: { permissions: { only: ['ANONYMOUS'] } },
		resolve: {
					id: function ($stateParams) {
						return $stateParams.id;
					},
					token: function ($stateParams) {
							return encodeURIComponent($stateParams.token);
					}
				}
	})
	 .state('userList', {
			url: '/user',
			templateUrl: '/appAdmin/components/user/userListView.html',
			controller: 'userListController',
			authenticate: true,
			data: {	permissions: {only: ['ADMIN'],redirectTo: 'accessDenied'}	}
	 })
	 .state('userEdit', {
			url: '/user/edit/:id',
			templateUrl: '/appAdmin/components/user/userEditView.html',
			controller: 'userEditController',
			authenticate: true,
			resolve: {
						id: function ($stateParams) {
							return $stateParams.id;}
					},
			data: {	permissions: {only: ['ADMIN'],redirectTo: 'accessDenied'	}
		 }        
	 })
	.state('userDetail', {
			url: '/user/detail/:id',
			templateUrl: '/appAdmin/components/user/userDetailView.html',
			controller: 'userDetailController',
			authenticate: true,
			resolve: {
						id: function ($stateParams) {
							return $stateParams.id;}
					},
			data: {	permissions: {only: ['ADMIN'],redirectTo: 'accessDenied'}	}
	})
	 .state('roleList', {
			url: '/role',
			templateUrl: '/appAdmin/components/role/roleListView.html',
			controller: 'roleListController',
			authenticate: true,
			data: {	permissions: {only: ['ADMIN'],redirectTo: 'accessDenied'	}
		 }
	 })
	 .state('roleEdit', {
			url: '/role/edit/:id',
			templateUrl: '/appAdmin/components/role/roleEditView.html',
			controller: 'roleEditController',
			authenticate: true,
			resolve: {
						id: function ($stateParams) {
							return $stateParams.id;}
					},
			data: {	permissions: {only: ['ADMIN'],redirectTo: 'accessDenied'	}
		 }
	 })
	 .state('roleDetail', {
			url: '/role/detail/:id',
			templateUrl: '/appAdmin/components/role/roleDetailView.html',
			controller: 'roleDetailController',
			authenticate: true,
			resolve: {
						id: function ($stateParams) {
							return $stateParams.id;}
					},
			data: {	permissions: {only: ['ADMIN'],redirectTo: 'accessDenied'	}
		 }
	 })
	.state('personalInformationList', {
			url: '/personalInformation',
			templateUrl: '/appAdmin/components/personalInformation/personalInformationListView.html',      
			controller: 'personalInformationListController',
			authenticate: true,
			data: {	permissions: {only: ['AUTHORIZED'],redirectTo: 'accessDenied'}	}
	 })
	 .state('personalInformationEdit', {
			url: '/personalInformation/edit/:id',
			templateUrl: '/appAdmin/components/personalInformation/personalInformationEditView.html',
			controller: 'personalInformationEditController',
			authenticate: true,
			resolve: {
						id: function ($stateParams) {
							return $stateParams.id;}
			},
			data: {	permissions: {only: ['AUTHORIZED'],redirectTo: 'accessDenied'	}
		 }
	 })
	 .state('personalInformationDetail', {
			url: '/personalInformation/detail/:id',
			templateUrl: '/appAdmin/components/personalInformation/personalInformationDetailView.html',	 
			controller: 'personalInformationDetailController',
			authenticate: true,
			resolve: {
						id: function ($stateParams) {
							return $stateParams.id;}
			},
			data: {	permissions: {only: ['AUTHORIZED'],redirectTo: 'accessDenied'}	}
	 })
	.state('blogEntryList', {
			url: '/blogEntry',
			templateUrl: '/appAdmin/components/blogEntry/blogEntryListView.html',      
			controller: 'blogEntryListController',
			authenticate: true,
			data: {	permissions: {only: ['AUTHORIZED'],redirectTo: 'accessDenied'}	}
	 })
	 .state('blogEntryEdit', {
			url: '/blogEntry/edit/:id',
			templateUrl: '/appAdmin/components/blogEntry/blogEntryEditView.html',
			controller: 'blogEntryEditController',
			authenticate: true,
			resolve: {
						id: function ($stateParams) {
							return $stateParams.id;}
			},
			data: {	permissions: {only: ['AUTHORIZED'],redirectTo: 'accessDenied'	}
		 }
	 })
	 .state('blogEntryDetail', {
			url: '/blogEntry/detail/:id',
			templateUrl: '/appAdmin/components/blogEntry/blogEntryDetailView.html',	 
			controller: 'blogEntryDetailController',
			authenticate: true,
			resolve: {
						id: function ($stateParams) {
							return $stateParams.id;}
			},
			data: {	permissions: {only: ['AUTHORIZED'],redirectTo: 'accessDenied'}	}
	 })
	.state('tagList', {
			url: '/tag',
			templateUrl: '/appAdmin/components/tag/tagListView.html',      
			controller: 'tagListController',
			authenticate: true,
			data: {	permissions: {only: ['AUTHORIZED'],redirectTo: 'accessDenied'}	}
	 })
	 .state('tagEdit', {
			url: '/tag/edit/:id',
			templateUrl: '/appAdmin/components/tag/tagEditView.html',
			controller: 'tagEditController',
			authenticate: true,
			resolve: {
						id: function ($stateParams) {
							return $stateParams.id;}
			},
			data: {	permissions: {only: ['AUTHORIZED'],redirectTo: 'accessDenied'	}
		 }
	 })
	 .state('tagDetail', {
			url: '/tag/detail/:id',
			templateUrl: '/appAdmin/components/tag/tagDetailView.html',	 
			controller: 'tagDetailController',
			authenticate: true,
			resolve: {
						id: function ($stateParams) {
							return $stateParams.id;}
			},
			data: {	permissions: {only: ['AUTHORIZED'],redirectTo: 'accessDenied'}	}
	 })
	;

	var urlRegisterLogin = GlobalInfo.apiUrl + '/Account/RegisterLoginExternal';
	
	$authProvider.loginUrl = GlobalInfo.apiUrl + '/oauth/token';
	$authProvider.tokenName = 'access_token';
	
	blockUIConfig.message = 'Please wait!';

})
.constant('GlobalInfo',
{
	apiUrl: '/api',
	localHostUrl: localHostUrl+'/admin.html#/',
	resetUrl: localHostUrl + '/admin.html#/resetpassword',
	confirmUrl: localHostUrl + '/admin.html#/verificationToken'
})
.run(function (PermRoleStore, authManager, $rootScope, $state) {

	// Define anonymous role
    PermRoleStore.defineRole('ADMIN', function (stateParams) {
		return authManager.isInRole("admin");
	});
	
    PermRoleStore.defineRole('ANONYMOUS', function (stateParams) {
		return authManager.isAnonymous();
	});
	
    PermRoleStore.defineRole('AUTHORIZED', function (stateParams) {
		return !authManager.isAnonymous();
	});

	 // Redirect to login if route requires auth and you're not logged in
    $rootScope.$on('$stateChangeStart', function (event, toState, toParams) {
        
        if (toState.authenticate && authManager.isAnonymous()) {
                $rootScope.returnToState = toState.url;
                $rootScope.returnToStateParams = toParams.id;
                $state.go('login', {});
            } 
    });
})
;