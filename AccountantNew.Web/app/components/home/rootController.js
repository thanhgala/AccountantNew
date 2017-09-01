(function (app) {
    app.controller('rootController', ['$scope', 'apiService',
        function ($scope, apiService) {
            $scope.slideBar = "/app/shared/view/slideBar.html";

            function getFileRootCategory() {
                apiService.get('api/newcategory/getfilecategory', null, function (result) {
                    $scope.rootCategory = result.data;
                }, function () {
                    console.log('Cannot get list category');
                });
            }
            getFileRootCategory();
        }])
})(angular.module('accountantnew'))