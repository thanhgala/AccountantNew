//(function (app) {
//    app.controller('newsAddController', newsAddController);
(function (app) {
    app.controller('applicationGroupsEditController', ['$scope', 'apiService', '$state', 'commonService', 'notificationService', '$uibModalInstance', '$uibModal', '$uibModalStack', '$timeout', 'shareIDService', '$filter',
    function ($scope, apiService, $state, commonService, notificationService, $uibModalInstance, $uibModal, $uibModalStack, $timeout, shareIDService, $filter) {
        $scope.isPost = false;
        $scope.group = {
            ID: 0,
            Roles: []
                //ApplicationRoleGroup: {
                //    //CanCreate: false,
                //    //CanRead: false,
                //    //CanUpdate: false,
                //    //CanDelete: false
                //} 
        }

        $scope.$watch("group.Roles", function (n, o) {
            var checked = $filter('filter')(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled')
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true)

        function findIndexRole(role) {
            return role.ID === shareIDService.getRoleID();
        }

        function findIndexRole2(role,item) {
            return role.ID === item.ID;
        }

        function loadDetail() {
            apiService.get('/api/applicationGroup/detail/' + shareIDService.getID(), null, function (result) {
                $scope.group = result.data;
                if (shareIDService.getRoleID()) {
                    //$scope.roleChoose = $scope.group.Roles.find(findRole)
                    $scope.stt = $scope.group.Roles.findIndex(findIndexRole);
                }
                if (shareIDService.getRoleID() === '446fd7d9-077d-4dd4-b819-ade6f052d036') {
                    $scope.isPost = true;
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
            $uibModalInstance.close();
            notificationService.displayError(response.data.Message);
            notificationService.displayErrorValidation(response);
        }

        $scope.UpdateAppGroup = function () {
            apiService.put('api/applicationGroup/update', $scope.group, addSuccessed, addFailed);
        }

        $scope.ConfigPermission = function (id) {
            $uibModalInstance.close();
            $uibModal.open({
                animation: true,
                templateUrl: '/app/components/application_groups/applicationPermissionPoup.html',
                size: 'sm',
                controller: 'applicationGroupsEditController'
            });
            shareIDService.setRoleID(id);
        }

        $scope.deleteMultipleRole = function () {
            var listID = [];
            angular.forEach($scope.selected, function (item) {
                function checkRole(role) {
                    return role.ID === item.ID;
                }
                var stt = $scope.group.Roles.findIndex(checkRole)
                $scope.group.Roles.splice(stt, 1);
            });
        }

        $scope.AddPermission = function () {
            apiService.put('api/applicationGroup/update', $scope.group, addSuccessed, addFailed);
        }

        $scope.close = function (boolen) {
            $uibModalInstance.close();
            if (!boolen) {
                $uibModal.open({
                    animation: true,
                    templateUrl: '/app/components/application_groups/applicationGroupsEditPoup.html',
                    size: 'lg',
                    controller: 'applicationGroupsEditController'
                });
            }
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

        $scope.AccessFile = function () {
            $uibModal.open({
                animation: true,
                templateUrl: '/app/components/application_groups/applicationFilePermission.html',
                size: 'lg',
                controller: 'applicationFileController'
            });
            $uibModalInstance.close();
        }
    }])
})(angular.module('accountantnew.application_groups'))