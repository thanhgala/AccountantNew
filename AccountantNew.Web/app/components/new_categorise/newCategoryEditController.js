(function (app) {
    app.controller('newCategoryEditController', ['$scope', 'apiService', '$ngBootbox', '$uibModalInstance', 'commonService', 'notificationService', 'shareIDService','$state','$timeout',
    function ($scope, apiService, $ngBootbox, $uibModalInstance, commonService, notificationService, shareIDService,$state,$timeout) {

            $scope.newCategory = {
                
            };

            $scope.parentCategories = [];

            $scope.close = function () {
                $uibModalInstance.close();
            }

            $scope.getAlias = function () {
                $scope.newCategory.Alias = commonService.getSeoTitle($scope.newCategory.Name);
            }

            function loadCategoryDetail() {
                apiService.get('api/newcategory/getid/' + shareIDService.getID(), null, function (result) {
                    $scope.newCategory = result.data;
                }, function (error) {
                    notificationService.displayError(error.data);
                });
            }

            loadCategoryDetail();

            function loadParentCategories() {
                apiService.get('api/newcategory/getallparent', null, function (result) {
                    $scope.parentCategories = result.data;
                }, function (error) {
                    console.log('cannot get list parent');
                })
            }
            loadParentCategories();

            $scope.UpdateNewCategory = function () {
                apiService.put('api/newcategory/update', $scope.newCategory,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được chỉnh sữa');
                    $uibModalInstance.close();
                    $state.go("home");
                    $timeout(function () {
                        $state.go("new_categories");
                    }, 0.01);
                }, function (error) {
                    notificationService.displayError('Không thể sửa bản ghi.');
                });
            }
        }])
})(angular.module('accountantnew.new_categories'))