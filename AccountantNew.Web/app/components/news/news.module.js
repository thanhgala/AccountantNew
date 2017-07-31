/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('accountantnew.news', ['accountantnew.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider
            .state('news', {
                url: "/news",
                parent: 'base',
                templateUrl: "/app/components/news/newsListView.html",
                controller: "newsListController"
            })
    }
})();