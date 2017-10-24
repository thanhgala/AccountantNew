//(function (app) {
//    app.controller('newsAddController', newsAddController);
(function (app) {
    app.controller('focusEditController', ['$scope', 'apiService', '$state', 'commonService', 'notificationService', '$uibModalInstance', '$timeout', 'shareIDService',
        function ($scope, apiService, $state, commonService, notificationService, $uibModalInstance, $timeout, shareIDService) {

            $scope.focus = {
                EndDate: new Date()
            }

            $scope.focusCategory = [
                { Type: 1, Name: 'Tin vui' },
                { Type: 2, Name: 'Tin buồn' },
                { Type: 3, Name: 'Nhân viên mới' }
            ]

            $scope.close = function () {
                $uibModalInstance.close();
            }

            function loadFocusDetail() {
                apiService.get('api/focus/getfocus/' + shareIDService.getID(), null, function (result) {
                    $scope.focus = result.data;
                    if (result.data.EndDate) {
                        $scope.focus.EndDate = new Date(result.data.EndDate);
                    }
                    else {
                        $scope.focus.EndDate = new Date(new Date());
                    }
                    //angular.element(document).find('#typeSelect').val($scope.focus.Type);
                }, function (error) {
                    notificationService.displayError(error.data);
                });
            }

            loadFocusDetail();

            $scope.UpdateFocus = function () {
                apiService.put('api/focus/update', $scope.focus, function (result) {
                    notificationService.displaySuccess('Tiêu điểm đã được chỉnh sửa');
                    shareIDService.setIsEdit(true);
                    $uibModalInstance.close();
                    $state.go("home");
                    $timeout(function () {
                        $state.go("focus");
                    }, 100);
                }, function (error) {
                    notificationService.displayError('Không sửa được bản ghi.');
                })
            }
        }])
})(angular.module('accountantnew.focus'))