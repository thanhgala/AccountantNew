(function (app) {
    app.controller('newCategoryAddController', ['$scope', 'apiService', '$ngBootbox', '$state', '$timeout', '$uibModalInstance', 'commonService', 'notificationService', 'shareIDService',
        function ($scope, apiService, $ngBootbox, $state, $timeout, $uibModalInstance, commonService, notificationService, shareIDService) {

            $scope.newCategory = {
                CreatedDate: new Date(),
                Status: true
            };

            //$scope.parentCategories = [];

            $scope.close = function () {
                $uibModalInstance.close();
            }

            $scope.getAlias = function () {
                $scope.newCategory.Alias = commonService.getSeoTitle($scope.newCategory.Name);
            }
            
            function loadParentCategories() {
                $scope.isParent = shareIDService.getID();
                var listDisplay = [];
                if ($scope.isParent == null) {
                    //apiService.get('api/newcategory/getallparent', null, function (result) {
                    //    $scope.parentCategories = result.data;
                    //}, function (error) {
                    //    console.log('cannot get list parent');
                    //});

                    apiService.get('api/newcategory/getrootparent', null, function (result) {
                        angular.forEach(result.data, function (item) {
                            listDisplay.push(item.DisplayOrder);
                        });
                        $scope.newCategory.DisplayOrder = Math.max(...listDisplay) + 1;
                        listDisplay.splice(0, listDisplay.length);
                    }, function (error) {
                        console.log('cannot get list parent');
                    });
                } else {
                    apiService.get('api/newcategory/getid/' + shareIDService.getID(), null, function (result) {
                        $scope.Name = result.data.Name;
                        $scope.newCategory.ParentID = shareIDService.getID();
                    }, function (error) {
                        notificationService.displayError(error.data);
                    });

                    apiService.get('api/newcategory/getchildrootparent/' + shareIDService.getID(), null, function (result) {
                        if (result.data.length > 0) {
                            angular.forEach(result.data, function (item) {
                                listDisplay.push(item.DisplayOrder);
                            });
                            $scope.newCategory.DisplayOrder = Math.max(...listDisplay) + 1;
                            listDisplay.splice(0, listDisplay.length);
                        }
                        else {
                            $scope.newCategory.DisplayOrder = 1;
                        }

                    }, function (error) {
                        notificationService.displayError(error.data);
                    });

                }
            }
            loadParentCategories();

            $scope.AddNewCategory = function () {
                apiService.post('api/newcategory/create', $scope.newCategory,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được thêm mới');
                    $uibModalInstance.close();
                    $state.go("home");
                    $timeout(function () {
                        $state.go("new_categories");
                    }, 0.01);
                }, function (error) {
                    notificationService.displayError('Không thể thêm mới bản ghi.');
                });
            }
        }])
})(angular.module('accountantnew.new_categories'))