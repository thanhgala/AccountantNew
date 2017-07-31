/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('accountantnew.new_categories', ['accountantnew.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider
            .state('new_categories', {
                url: "/new_categories",
                parent: 'base',
                templateUrl: "/app/components/new_categorise/newCategoryListView.html",
                controller: "newCategoryListController"
            })
    }
})();