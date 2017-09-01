(function (app) {
    app.controller('applicationGroupsListController', ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter', '$uibModal', 'shareIDService',
    function ($scope, apiService, notificationService, $ngBootbox, $filter, $uibModal, shareIDService) {
        $scope.keyword = '';
        $scope.loading = true;

        $scope.data = [];

        $scope.isAll = false;

        function getGroups(page) {
            page = page || 0;
            $scope.loading = true;
            var config = {
                params: {
                    page: page,
                    pageSize: 20
                }
            }
            apiService.get('api/applicationGroup/getlistpaging', config, function (result) {
                if (result.data.TotalCount === 0) {
                    notificationService.displayWarning('Không có bản ghi nào được tìm thấy.');
                }
                $scope.data = result.data.Items;
                $scope.totalCount = result.data.TotalCount;
                $scope.loading = false;
            }, function (error) {
                $scope.loading = false;
                console.log('Cannot get list category');
            });
        }
        getGroups();

        $scope.deleteItem = function (id) {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?.').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('/api/applicationGroup/delete/', config, function (result) {
                    notificationService.displaySuccess(result.data.Name + ' xóa thành công');
                    getGroups();
                }, function (err) {
                    notificationService.displayError('Xóa không thành công.');
                })
            });
        }

        $scope.selectAll = function () {
            if ($scope.isAll === false) {
                angular.forEach($scope.data, function (item) {
                    item.checked = true;
                })
                $scope.isAll = true;
            }
            else {
                angular.forEach($scope.data, function (item) {
                    item.checked = false;
                })
                $scope.isAll = false;
            }
        }

        $scope.$watch("data", function (n, o) {
            var checked = $filter('filter')(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled')
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true)

        $scope.deleteMultiple = function () {
            var listID = [];
            angular.forEach($scope.selected, function (item) {
                listID.push(item.ID);
            });

            var config = {
                params: {
                    checkedList: JSON.stringify(listID)
                }
            }
            apiService.del('api/applicationGroup/deletemulti/', config, function (result) {
                notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi');
                getGroups();
            }, function (error) {
                notificationService.displayWarning('Xóa không thành công.')
            });
        }

        $scope.openAddPoup = function () {
            $uibModal.open({
                animation: true,
                backdrop: 'static',
                templateUrl: '/app/components/application_groups/applicationGroupsAddPoup.html',
                size: 'lg',
                controller: 'applicationGroupsAddController'
            })
            shareIDService.setIsAdd(true);
        }

        $scope.openUpdatePoup = function (id) {
            $uibModal.open({
                animation: true,
                backdrop: 'static',
                templateUrl: '/app/components/application_groups/applicationGroupsEditPoup.html',
                size: 'lg',
                controller: 'applicationGroupsEditController'
            })
            shareIDService.setID(id);
        }
    }])
})(angular.module('accountantnew.application_groups'))