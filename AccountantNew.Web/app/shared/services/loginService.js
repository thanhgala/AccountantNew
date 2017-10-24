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
                        avarta: response.data.avarta,
                        groups: response.data.groups,
                        isAdmin: response.data.isAdmin,
                        id: response.data.id,
                        fullname: response.data.fullname,
                        namepca: response.data.namepca,
                        department:response.data.department
                    };
                    authenticationService.setTokenInfo(userInfo);
                    authData.authenticationData.IsAuthenticated = true;
                    authData.authenticationData.userName = userInfo.userName;
                    authData.authenticationData.accessToken = userInfo.accessToken;
                    authData.authenticationData.groups = userInfo.groups;
                    authData.authenticationData.isAdmin = userInfo.isAdmin;
                    authData.authenticationData.avarta = userInfo.avarta;
                    authData.authenticationData.id = userInfo.id;
                    authData.authenticationData.fullname = userInfo.fullname;
                    authData.authenticationData.namepca = userInfo.namepca;
                    authData.authenticationData.department = userInfo.department;
                    deferred.resolve(null);
                }, function (err, status) {
                    authData.authenticationData.IsAuthenticated = false;
                    authData.authenticationData.userName = null;
                    authData.authenticationData.avarta = null;
                    authData.authenticationData.groups = null;
                    authData.authenticationData.isAdmin = null;
                    authData.authenticationData.accessToken = null;
                    authData.authenticationData.id = null;
                    authData.authenticationData.fullname = null;
                    authData.authenticationData.namepca = null;
                    authData.authenticationData.department = null;
                    deferred.resolve(err);
                });
                return deferred.promise;
            }

            this.logOut = function () {
                //apiService.post('/api/account/logout', null, function (response) {
                //    authenticationService.removeToken();
                //    authData.authenticationData.IsAuthenticated = false;
                //    authData.authenticationData.userName = null;
                //    authData.authenticationData.isLoading = true;
                //    authData.authenticationData.accessToken = null;
                //    $state.go('login');
                //},null)

                authenticationService.removeToken();
                authData.authenticationData.IsAuthenticated = false;
                authData.authenticationData.userName = null;
                authData.authenticationData.isLoading = true;
                authData.authenticationData.accessToken = null;
                authData.authenticationData.groups = null;
                authData.authenticationData.isAdmin = null;
                authData.authenticationData.id = null;
                authData.authenticationData.fullname = null;
                authData.authenticationData.namepca = null;
                authData.authenticationData.department = null;
                $state.go('login');
            }
        }
    ])
})(angular.module('accountantnew.common'));