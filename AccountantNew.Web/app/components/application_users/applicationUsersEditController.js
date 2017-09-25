(function (app) {
    app.controller('applicationUsersEditController', ['apiService', '$scope', 'notificationService', '$location', '$stateParams', 'commonService',
        function (apiService, $scope, notificationService, $location, $stateParams, commonService) {
            $scope.account = {
                Groups: []
            };

            function loadDetail() {
                apiService.get('/api/applicationUser/detail/' + $stateParams.id, null, function (result) {
                    $scope.account = result.data;
                    $scope.account.BirthDay = new Date($scope.account.BirthDay);
                }, function (error) {
                    notificationService.displayError(error.data);
                });
            }

            loadDetail();

            $scope.updateAccount = function () {

            }

            $scope.GroupRole = function () {
                apiService.get('/api/applicationGroup/getlistall', null, function (response) {
                    $scope.groups = response.data;
                }, function (err) {
                    notificationService.displayError('Không tải được danh sách nhóm.');
                })
            }

            $scope.updateGroupToUser = function () {
                apiService.put('api/applicationUser/update', $scope.account, function () {
                    notificationService.displaySuccess($scope.account.FullName + ' đã được cập nhật thành công.');
                    angular.element(document).find('#updateGroup').modal('hide');
                    //$location.url('application_users');
                }, function (response) {
                    notificationService.displayError(response.data.Message);
                    notificationService.displayErrorValidation(response);
                })
            }
        }
    ])
})(angular.module('accountantnew.application_users'));