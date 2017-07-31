(function (app) {
    app.controller('newCategoryListController', ['$scope', 'apiService', '$ngBootbox', '$filter', 'commonService', '$timeout', '$state', '$uibModal', 'shareIDService', 'notificationService',
        function ($scope, apiService, $ngBootbox, $filter, commonService, $timeout, $state, $uibModal, shareIDService, notificationService) {
            $scope.keyword = '';
            $scope.loading = true;

            $scope.data = [];

            $scope.isToggle = false;
            $scope.toggleName = 'Expend All'
            $scope.toggleFunc = function () {
                $scope.isToggle = !$scope.isToggle;
                if ($scope.isToggle) {
                    $scope.$broadcast('angular-ui-tree:collapse-all');
                    $scope.toggleName = 'Expend All'
                }
                else {
                    $scope.$broadcast('angular-ui-tree:expand-all');
                    $scope.toggleName = 'Collapse All'
                }
            }

            function getNewCategories() {
                apiService.get('api/newcategory/getall', null, function (result) {
                    $scope.data = result.data;
                    $scope.loading = false;
                }, function () {
                    console.log('Cannot get list category');
                });
            }
            getNewCategories();

            $scope.deleteNewCategory = function (id) {
                $ngBootbox.confirm('Bạn có chắc muốn xóa?.').then(function () {
                    var config = {
                        params: {
                            id: id
                        }
                    }
                    apiService.del('/api/newcategory/delete', config, function (result) {
                        notificationService.displaySuccess(result.data.Name + ' xóa thành công');
                        getNewCategories();
                    }, function (err) {
                        notificationService.displayError('Xóa không thành công.');
                    })
                });
            }

            $scope.openUpdatePoup = function (id) {
                $uibModal.open({
                    animation: true,
                    templateUrl: '/app/components/new_categorise/newCategoryEditPoup.html',
                    size: 'lg',
                    controller: 'newCategoryEditController'
                })
                shareIDService.setID(id);
            }

            $scope.openAddPoup = function (id) {
                id = id || null;
                $uibModal.open({
                    animation: true,
                    templateUrl: '/app/components/new_categorise/newCategoryAddPoup.html',
                    size: 'lg',
                    controller: 'newCategoryAddController'
                })
                shareIDService.setID(id);
            }          
        }])
})(angular.module('accountantnew.new_categories'))