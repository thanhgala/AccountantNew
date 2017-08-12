//(function (app) {
//    app.controller('newsAddController', newsAddController);
(function (app) {
    app.controller('applicationGroupsEditController', ['$scope', 'apiService', '$state', 'commonService', 'notificationService', '$uibModalInstance', '$uibModal', '$timeout', 'shareIDService',
    function ($scope, apiService, $state, commonService, notificationService, $uibModalInstance, $uibModal, $timeout, shareIDService) {

        $scope.group = {
            ID: 0,
            Roles: {
                ApplicationRoleGroup: {
                    //CanCreate: false,
                    //CanRead: false,
                    //CanUpdate: false,
                    //CanDelete: false
                }
            }
        }

        function findRole(role) {
            return role.ID === shareIDService.getRoleID();
        }

        function findIndexRole(role) {
            return role.ID === shareIDService.getRoleID();
        }

        function loadDetail() {
            apiService.get('/api/applicationGroup/detail/' + shareIDService.getID(), null, function (result) {
                $scope.group = result.data;
                if (shareIDService.getRoleID()) {
                    //$scope.roleChoose = $scope.group.Roles.find(findRole)
                    $scope.stt = $scope.group.Roles.findIndex(findIndexRole);
                }
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }

        loadDetail();

        function addSuccessed() {
            notificationService.displaySuccess($scope.group.Name + ' đã được chỉnh sửa.');
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

        $scope.UpdateAppGroup = function () {
            apiService.put('api/applicationGroup/update', $scope.group, addSuccessed, addFailed);
        }

        $scope.ConfigPermission = function (id) {
            $uibModal.open({
                animation: true,
                templateUrl: '/app/components/application_groups/applicationPermissionPoup.html',
                size: 'sm',
                controller: 'applicationGroupsEditController'
            });
            shareIDService.setRoleID(id);
        }

        $scope.AddPermission = function () {
            apiService.put('api/applicationGroup/update', $scope.group, addSuccessed, addFailed);
        }

        $scope.close = function () {
            $uibModalInstance.close();
        }

        $scope.AddRole = function () {
            $uibModal.open({
                animation: true,
                templateUrl: '/app/components/application_groups/applicationGroupsAddPoup.html',
                size: 'lg',
                controller: 'applicationGroupsAddController'
            });
            shareIDService.setIsAdd(false);
            $uibModalInstance.close();
        }
    }])
})(angular.module('accountantnew.application_groups'))