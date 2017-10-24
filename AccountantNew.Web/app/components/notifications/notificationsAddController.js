
(function (app) {
    app.controller('notificationsAddController', ['$scope', 'apiService', '$state', 'commonService', 'notificationService', '$uibModalInstance', '$timeout', 'shareIDService','authData',
        function ($scope, apiService, $state, commonService, notificationService, $uibModalInstance, $timeout, shareIDService,authData) {

            $scope.flatFolders = [];
            $scope.newcategories = [];

            //cấu hình ckeditor
            $scope.ckeditorOptions = {
                language: 'vi',
                height: '200px',
            }

            $scope.notification = {
                CreatedDate: new Date(),
                Private: null,
                Status: true,
                ApplicationUserId: authData.authenticationData.id
            }

            $scope.GetAlias = function () {
                $scope.notification.Alias = commonService.getSeoTitle($scope.notification.Name);
            };

            $scope.close = function () {
                $uibModalInstance.close();
            }

            function loadNewCategory() {
                apiService.get('api/newcategory/getallnotificategory', null, function (result) {
                    result.data.splice(0,1);
                    $scope.newcategories = result.data;
                }, function (error) {
                    console.log('cannot get list parent');
                })
                $scope.notification.Image = "/Assets/admin/img/thong-bao.png";
            }

            loadNewCategory();

            $scope.AddNotification = function () {
                apiService.post('api/notification/create', $scope.notification, function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được thêm mới');
                    $uibModalInstance.close();
                    $state.go("home");
                    $timeout(function () {
                        $state.go("notifications");
                    }, 100);

                }, function (error) {
                    notificationService.displayError('Không thêm được bản ghi.');
                })
            }
        }])
})(angular.module('accountantnew.notifications'))