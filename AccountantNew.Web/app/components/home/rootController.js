(function (app) {
    app.controller('rootController', ['$scope', 'apiService', 'authData', 'loginService',
        function ($scope, apiService, authData, loginService) {
            $scope.slideBar = "/app/shared/view/slideBar.html";

            function getFileRootCategory() {
                apiService.get('api/newcategory/getfilecategory', null, function (result) {
                    $scope.rootCategory = result.data;
                }, function () {
                    console.log('Cannot get list category');
                });
            }
            getFileRootCategory();

            $scope.authentication = authData.authenticationData;

            $scope.logOut = function () {
                loginService.logOut();
            }
        }])
})(angular.module('accountantnew'))