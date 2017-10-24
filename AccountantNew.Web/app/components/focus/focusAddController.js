
(function (app) {
    app.controller('focusAddController', ['$scope', 'apiService', '$state', 'commonService', 'notificationService', '$uibModalInstance', '$timeout', 'shareIDService',
        function ($scope, apiService, $state, commonService, notificationService, $uibModalInstance, $timeout, shareIDService) {

            $scope.focus = {
                Status: true,
                EndDate: new Date(),
            }

            $scope.close = function () {
                $uibModalInstance.close();
            }

            $scope.AddFocus = function () {
                apiService.post('api/focus/create', $scope.focus, function (result) {
                    notificationService.displaySuccess('Tiêu điểm đã được thêm mới');
                    $uibModalInstance.close();
                    $state.go("home");
                    $timeout(function () {
                        $state.go("focus");
                    }, 100);

                }, function (error) {
                    notificationService.displayError('Không thêm được bản ghi.');
                })
            }
        }])
})(angular.module('accountantnew.focus'))