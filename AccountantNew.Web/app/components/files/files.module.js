/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('accountantnew.files', ['accountantnew.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider
             .state('files', {
                 url: "/files/:folder-:id",
                 parent: 'base',
                 templateUrl: "/app/components/files/filesListView.html",
                 controller: "filesListController"
             })
            .state('files.action', {
                url: "/files/:folder/:action",
                parent: 'base',
                templateUrl: "/app/components/files/filesPdfView.html",
                controller: "filesPdfController"
            })
    }
})();