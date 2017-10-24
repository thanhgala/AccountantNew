(function () {
    angular.module('accountantnew.notifications', ['accountantnew.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider
            .state('notifications', {
                url: "/notifications",
                parent: 'base',
                templateUrl: "/app/components/notifications/notificationsListView.html",
                controller: "notificationsListController"
            })
    }
})();