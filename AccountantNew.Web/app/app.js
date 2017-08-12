(function () {
    'use strict';

    angular.module('accountantnew',
        ['accountantnew.news',
         'accountantnew.new_categories',
         'accountantnew.application_groups',
         'accountantnew.common'])
        .config(config)
        .config(configAuthentication);

    config.$inject = ['$stateProvider', '$urlRouterProvider', '$qProvider', '$locationProvider'];

    configAuthentication.$inject = ['$httpProvider'];

    function config($stateProvider, $urlRouterProvider, $qProvider, $locationProvider) {
        //cách cấu hình routing cho accountantnew
        $stateProvider
            .state('base', {
                url: '',
                templateUrl: '/app/shared/view/baseView.html',
                abstract: true
            })
            .state('login', {
                url: '/login',
                templateUrl: "/app/components/login/loginView.html",
                controller: "loginController"
            })
            .state('home', {
                url: "/home",
                parent: 'base',
                templateUrl: "/app/components/home/homeView.html",
                controller: "homeController"
            })
            //.state('profile', {
            //    url: "/profile/:id",
            //    parent: 'base',
            //    templateUrl: "/app/components/profile/profileView.html",
            //    controller: "profileController"
            //})
            //.state('error', {
            //    url: "/error",
            //    parent: 'base',
            //    templateUrl: "/app/components/error500/err500View.html",
            //    controller: "err500Controller"
            //});
        $urlRouterProvider.otherwise('/home');
        $qProvider.errorOnUnhandledRejections(false);
        //$locationProvider.html5Mode(true);
    }
    function configAuthentication($httpProvider) {
        $httpProvider.interceptors.push(function ($q, $location) {
            return {
                request: function (config) {

                    return config;
                },
                requestError: function (rejection) {

                    return $q.reject(rejection);
                },
                response: function (response) {
                    if (response.status === 401) {
                        $location.path('/error');
                    }
                    //the same response/modified/or a new one need to be returned.
                    return response;
                },
                responseError: function (rejection) {

                    if (rejection.status === 401) {
                        $location.path('/error');
                    }
                    return $q.reject(rejection);
                }
            };
        });
    }
})();