/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('accountantnew.application_groups', ['accountantnew.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider
            .state('application_groups', {
                url: "/application_groups",
                parent: 'base',
                templateUrl: "/app/components/application_groups/applicationGroupsListView.html",
                controller: "applicationGroupsListController"
            })
    }
})();