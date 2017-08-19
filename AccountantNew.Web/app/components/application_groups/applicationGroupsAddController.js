//(function (app) {
//    app.controller('newsAddController', newsAddController);
(function (app) {
    app.controller('applicationGroupsAddController', ['$scope', 'apiService', '$state', 'commonService', 'notificationService', '$uibModalInstance', '$uibModal', '$timeout', 'shareIDService',
    function ($scope, apiService, $state, commonService, notificationService, $uibModalInstance, $uibModal, $timeout, shareIDService) {

        $scope.group = {
            ID: 0,
            Roles: []
        }

        $scope.isAdd = shareIDService.getIsAdd();

        function addSuccessed() {
            notificationService.displaySuccess($scope.group.Name + ' đã được thêm mới.');
            $uibModalInstance.close();
            $state.go("home");
            $timeout(function () {
                $state.go("application_groups");
            }, 0.01);
        }
        function addFailed(response) {
            notificationService.displayError(response.data.Message);
            notificationService.displayErrorValidation(response);
        }

        $scope.AddAppGroup = function () {
            if ($scope.isAdd) {
                apiService.post('api/applicationGroup/add', $scope.group, addSuccessed, addFailed);
            }
            else {
                apiService.put('api/applicationGroup/update', $scope.group, function () {
                    notificationService.displaySuccess($scope.group.Name + ' đã được chỉnh sữa.');
                    $uibModalInstance.close();
                    $uibModal.open({
                        animation: true,
                        templateUrl: '/app/components/application_groups/applicationGroupsEditPoup.html',
                        size: 'lg',
                        controller: 'applicationGroupsEditController'
                    });
                }, function (response) {
                    notificationService.displayError(response.data.Message);
                    notificationService.displayErrorValidation(response);
                });
            }
        }

        $scope.close = function () {
            $uibModalInstance.close();
        }

        function addRole() {
            if (!$scope.isAdd) {
                apiService.get('/api/applicationGroup/detail/' + shareIDService.getID(), null, function (result) {
                    $scope.group = result.data;
                    apiService.get('/api/applicationRole/getlistrolebycondition/' + shareIDService.getID(), function (response) {

                    }, function (err) {
                        $scope.roles = err.data;
                    })
                }, function (error) {
                    notificationService.displayError(error.data);
                });
            }
        }
        addRole();

        function loadRoles() {
            if ($scope.isAdd) {
                apiService.get('/api/applicationRole/getlistall', null, function (response) {
                    $scope.roles = response.data;
                }, function (err) {
                    notificationService.displayError('Không tải được danh sách quyền.')
                })
            }
        }
        loadRoles();
    }])
})(angular.module('accountantnew.application_groups'))