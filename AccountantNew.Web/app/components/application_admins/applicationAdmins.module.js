(function () {
    angular.module('accountantnew.application_admins', ['accountantnew.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider
        .state('application_admins', {
            url: "/application_admins",
            parent: 'base',
            templateUrl: "/app/components/application_admins/applicationAdminsListView.html",
            controller: "applicationAdminsListController"
        })
        .state('edit_application_admin', {
            //cấu hình state khi lấy tham số từ url
            url: "/edit_application_admin/:id/:page",
            parent: 'base',
            templateUrl: "/app/components/application_admins/applicationAdminsEditView.html",
            controller: "applicationAdminsEditController"
        })
    }
})();