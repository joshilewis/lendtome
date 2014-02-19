'use strict';

//var lendtomeApp = angular.module('lendtome', [
//    'ngRoute',
//    'lendtomeControllers',
//    'lendtomeServices'
//]);

// Declares how the application should be bootstrapped. See: http://docs.angularjs.org/guide/module
angular.module('lendtome', [
    'ngRoute',
    //'ui.compat',
    'ui.router',
    //'app.filters',
    'lendtomeServices',
    //'app.directives',
    'lendtomeControllers'
])

    // Gets executed during the provider registrations and configuration phase. Only providers and constants can be
    // injected here. This is to prevent accidental instantiation of services before they have been fully configured.
    .config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {

        // UI States, URL Routing & Mapping. For more info see: https://github.com/angular-ui/ui-router
        // ------------------------------------------------------------------------------------------------------------

        $routeProvider
            .when('/myitems', {
                templateUrl: 'app/myitems.html',
                controller: 'userItemsController'
            })
            .when('/signin', {
                templateUrl: 'app/signin.html'
                //controller: 'userItemsController'
            })
        ;

        //$stateProvider
        //    .state('/myitems', {
        //        url: '/myitems',
        //        templateUrl: 'app/myitems.html',
        //        controller: 'userItemsController'

        //    })
        //    .state('/signin', {
        //        url: '/signin',
        //        templateUrl: 'app/signin.html'

        //    })
        //    .state('otherwise', {
        //        url: '*path',
        //        templateUrl: '/views/404',
        //        controller: 'Error404Ctrl'
        //    });

        $locationProvider.html5Mode(true);

    }])

    // Gets executed after the injector is created and are used to kickstart the application. Only instances and constants
    // can be injected here. This is to prevent further system configuration during application run time.
    .run(['$templateCache', '$rootScope', '$state', '$stateParams', function ($templateCache, $rootScope, $state, $stateParams) {

        // <ui-view> contains a pre-rendered template for the current view
        // caching it will prevent a round-trip to a server at the first page load
        var view = angular.element('#ui-view');
        $templateCache.put(view.data('tmpl-url'), view.html());

        // Allows to retrieve UI Router state information from inside templates
        $rootScope.$state = $state;
        $rootScope.$stateParams = $stateParams;

        $rootScope.$on('$stateChangeSuccess', function (event, toState) {

            // Sets the layout name, which can be used to display different layouts (header, footer etc.)
            // based on which page the user is located
            $rootScope.layout = toState.layout;
        });
    }]);