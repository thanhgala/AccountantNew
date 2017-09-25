(function () {
    angular.module('accountantnew.application_users', ['accountantnew.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider
        .state('application_users', {
            url: "/application_users",
            parent: 'base',
            templateUrl: "/app/components/application_users/applicationUsersListView.html",
            controller: "applicationUsersListController"
        })
        .state('edit_application_user', {
            //cấu hình state khi lấy tham số từ url
            url: "/edit_application_user/:id",
            parent: 'base',
            templateUrl: "/app/components/application_users/applicationUsersEditView.html",
            controller: "applicationUsersEditController"
        })
    }
})();