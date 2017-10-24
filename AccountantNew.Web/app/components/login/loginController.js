(function (app) {
    app.controller('loginController', ['$scope', 'notificationService', '$injector', 'loginService', '$window', 'authData', 'authenticationService',
    function ($scope, notificationService, $injector, loginService, $window, authData, authenticationService) {

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

        //if (authenticationService.getTokenInfo()) {
        //    var stateService = $injector.get('$state');
        //    stateService.go('home');
        //}

        $scope.loginSubmit = function () {
            $scope.loading = true;
            var userInfo;
            loginService.login($scope.loginData.userName, $scope.loginData.password).then(function (response) {
                if (response != null) {
                    var message = response.data;
                    if (message.error == 'invalid_grant') {
                        $scope.loading = false;
                        notificationService.displayError(message.error_description);
                    }
                    else if (message.error == 'invalid_group') {
                        $scope.loading = false;
                        notificationService.displayError(message.error_description);
                    }
                }
                else {
                    $scope.loading = false;
                    var stateService = $injector.get('$state');
                    stateService.go('home');
                }
            });
        }

    }
    ])
})(angular.module('accountantnew'));