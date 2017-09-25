(function (app) {
    app.service('loginService', ['$http', '$q', 'authenticationService', 'authData', 'apiService', '$state',
        function ($http, $q, authenticationService, authData, apiService, $state) {
            var userInfo;
            var deferred;

            this.login = function (userName, password) {
                deferred = $q.defer();
                var data = "grant_type=password&username=" + userName + "&password=" + password;
                $http.post('/oauth/token', data, {
                    headers:
                       { 'Content-Type': 'application/x-www-form-urlencoded' }
                }).then(function (response) {
                    userInfo = {
                        accessToken: response.data.access_token,
                        userName: userName,
                        avarta: response.data.avarta
                    };
                    authenticationService.setTokenInfo(userInfo);
                    authData.authenticationData.IsAuthenticated = true;
                    authData.authenticationData.userName = userInfo.userName;
                    authData.authenticationData.accessToken = userInfo.accessToken;
                    authData.authenticationData.avarta = userInfo.avarta;
                    deferred.resolve(null);
                }, function (err, status) {
                    authData.authenticationData.IsAuthenticated = false;
                    authData.authenticationData.userName = null;
                    authData.authenticationData.avarta = null;
                    authData.authenticationData.accessToken = null;
                    deferred.resolve(err);
                });
                return deferred.promise;
            }

            this.logOut = function () {
                apiService.post('/api/account/logout', null, function (response) {
                    authenticationService.removeToken();
                    authData.authenticationData.IsAuthenticated = false;
                    authData.authenticationData.userName = null;
                    authData.authenticationData.isLoading = true;
                    authData.authenticationData.accessToken = null;
                    $state.go('login');
                },null)
            }
        }
    ])
})(angular.module('accountantnew.common'));