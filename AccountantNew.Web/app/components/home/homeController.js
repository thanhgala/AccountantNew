(function (app) {
    app.controller('homeController', ['$scope','apiService',
        function ($scope,apiService) {
            //function testApi() {
            //    apiService.get('api/new/getall', null, function (response) {
            //        console.log(response.data.alo)
            //    },null)
            //}
            //testApi();
        }]);
})(angular.module('accountantnew'));
