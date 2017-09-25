/// <reference path="/Assets/admin/libs/angular/angular.js" />
(function (app) {
    app.factory('apiService', apiService);

    apiService.$inject = ['$http', 'notificationService', 'authenticationService', '$location'];

    function apiService($http, notificationService, authenticationService, $location) {
        return {
            get: get,
            post: post,
            put: put,
            del: del
        }

        function post(url, data, success, failure) {
            authenticationService.setHeader();
            $http.post(url, data).then(function (result) {
                success(result);
            }, function (error) {
                //Không có quyền authen
                if (error.status === 403) {
                    notificationService.displayError('Authenticate is denied.');
                    $location.path('/error');
                }
                else if (failure != null) {
                    failure(error)
                }
            });
        }

        function get(url, params, success, failure) {
            authenticationService.setHeader();
            $http.get(url, params).then(function (result) {
                success(result);
            }, function (error) {
                if (error.status === 403) {
                    notificationService.displayError('Authenticate is denied.');
                    $location.path('/error');
                }
                else if (failure != null) {
                    failure(error)
                }
            });
        }

        function put(url, data, success, failure) {
            authenticationService.setHeader();
            $http.put(url, data).then(function (result) {
                success(result)
            }, function (error) {
                if (error.status === 403) {
                    notificationService.displayError('Authenticate is denied');
                    $location.path('/error');
                }
                else if (failure != null) {
                    failure(error);
                }
            });
        }

        function del(url, data, success, failure) {
            authenticationService.setHeader();
            $http.delete(url, data).then(function (result) {
                success(result);
            }, function (error) {
                if (error.status === 403) {
                    notificationService.displayError('Authenticate is denied');
                    $location.path('/error');
                }
                else if (failure != null) {
                    failure(error)
                }
            });
        }
    }
})(angular.module('accountantnew.common'));