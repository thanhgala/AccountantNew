(function () {
    angular.module('accountantnew.posts', ['accountantnew.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider
            .state('posts', {
                url: "/posts",
                parent: 'base',
                templateUrl: "/app/components/posts/postsListView.html",
                controller: "postsListController"
            })
    }
})();