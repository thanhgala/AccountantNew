(function (app) {
    app.controller('rootController', ['$scope',
        function ($scope) {
            $scope.slideBar = "/app/shared/view/slideBar.html"
        }])
})(angular.module('accountantnew'))