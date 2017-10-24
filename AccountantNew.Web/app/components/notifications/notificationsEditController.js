//(function (app) {
//    app.controller('newsAddController', newsAddController);
(function (app) {
    app.controller('notificationsEditController', ['$scope', 'apiService', '$state', 'commonService', 'notificationService', '$uibModalInstance', '$timeout', 'shareIDService',
        function ($scope, apiService, $state, commonService, notificationService, $uibModalInstance, $timeout, shareIDService) {

            $scope.newcategories = [];

            $scope.notification = {
            }

            $scope.GetAlias = function () {
                $scope.notification.Alias = commonService.getSeoTitle($scope.notification.Name);
            };

            $scope.close = function () {
                $uibModalInstance.close();
            }

            function loadNewCategory() {
                apiService.get('api/newcategory/getallnotificategory', null, function (result) {
                    $scope.newcategories = result.data;
                }, function (error) {
                    console.log('cannot get list parent');
                })
            }
            loadNewCategory();

            function loadNewDetail() {
                apiService.get('api/notification/getid/' + shareIDService.getID(), null, function (result) {
                    $scope.notification = result.data;
                }, function (error) {
                    notificationService.displayError(error.data);
                });
            }

            loadNewDetail();

            $scope.UpdateNotification = function () {
                apiService.put('api/notification/update', $scope.notification, function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được chỉnh sửa');
                    shareIDService.setIsEdit(true);
                    $uibModalInstance.close();
                    $state.go("home");
                    $timeout(function () {
                        $state.go("notifications");
                    }, 100);
                }, function (error) {
                    notificationService.displayError('Không sửa được bản ghi.');
                })
            }
        }])
})(angular.module('accountantnew.notifications'))