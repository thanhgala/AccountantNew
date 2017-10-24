(function () {
    angular.module('accountantnew.focus', ['accountantnew.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider
            .state('focus', {
                url: "/focus",
                parent: 'base',
                templateUrl: "/app/components/focus/focusListView.html",
                controller: "focusListController"
            })
    }
})();