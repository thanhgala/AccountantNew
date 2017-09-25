(function (app) {
    app.controller('loginController', ['$scope', 'notificationService', '$injector', 'loginService', '$window', 'authData',
    function ($scope, notificationService, $injector, loginService, $window, authData) {

        function autoLoad() {
            if (authData.authenticationData.isLoading) {
                $window.location.reload();
                authData.authenticationData.isLoading = false;
            }
        }

        autoLoad();

        $scope.loginData = {
            userName: "",
            password: ""
        };

        $scope.loginSubmit = function () {
            var userInfo;
            loginService.login($scope.loginData.userName, $scope.loginData.password).then(function (response) {
                if (response != null) {
                    var message = response.data;
                    if (message.error == 'invalid_grant') {
                        notificationService.displayError(message.error_description);
                    }
                    else if (message.error == 'invalid_group') {
                        notificationService.displayError(message.error_description);
                    }
                }
                else {
                    var stateService = $injector.get('$state');
                    stateService.go('home');
                    //var config = {
                    //    params: {
                    //        name: authData.authenticationData.userName
                    //    }
                    //}

                    //apiService.get('/api/applicationUser/info/', config, function (result) {
                    //    userInfo = {
                    //        fullName: result.data.fullName,
                    //        avarta: result.data.avarta,
                    //        id: result.data.id
                    //    };
                    //    authData.authenticationData.fullName = userInfo.fullName;
                    //    authData.authenticationData.avarta = userInfo.avarta;
                    //    authData.authenticationData.id = userInfo.id
                    //    authenticationService.setInfo(userInfo)
                    //}, function (error) {
                    //    notificationService.displayError(error.data);
                    //});
                }
            });
        }

    }
    ])
})(angular.module('accountantnew'));