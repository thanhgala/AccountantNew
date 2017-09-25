(function (app) {
    app.controller('homeController', ['$scope', 'apiService',
        function ($scope, apiService) {
            function testAuthen() {
                apiService.get('/api/home/testauthen', null, function (response) {
                    if (response != null) {
                        console.log('Authen is success');
                    }
                }, null)
            }
            testAuthen();
        }]);
})(angular.module('accountantnew'));
