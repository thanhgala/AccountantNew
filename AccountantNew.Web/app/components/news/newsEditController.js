//(function (app) {
//    app.controller('newsAddController', newsAddController);
(function (app) {
    app.controller('newsEditController', ['$scope', 'apiService', '$state', 'commonService', 'notificationService', '$uibModalInstance', '$timeout', 'shareIDService',
        function ($scope, apiService, $state, commonService, notificationService, $uibModalInstance, $timeout, shareIDService) {

            $scope.flatFolders = [];
            $scope.newcategories = [];

            //cấu hình ckeditor
            $scope.ckeditorOptions = {
                language: 'vi',
                height: '200px',
            }

            $scope.new = {
            }

            $scope.GetAlias = function () {
                $scope.new.Alias = commonService.getSeoTitle($scope.new.Name);
            };

            $scope.close = function () {
                $uibModalInstance.close();
            }

            function loadNewCategory() {
                apiService.get('api/newcategory/getnewscategory', null, function (result) {
                    //$scope.newcategories = commonService.getTree(result.data, "ID", "ParentID");
                    //$scope.newcategories.forEach(function (item) {
                    //    commonService.recur(item, 0, $scope.flatFolders);
                    //});
                    $scope.newcategories = result.data;
                }, function (error) {
                    console.log('cannot get list parent');
                })
            }
            loadNewCategory();

            function loadNewDetail() {
                apiService.get('api/new/getid/' + shareIDService.getID(), null, function (result) {
                    $scope.new = result.data;
                }, function (error) {
                    notificationService.displayError(error.data);
                });
            }
            loadNewDetail();

            $scope.ChooseImage = function () {
                var finder = new CKFinder();
                finder.readOnly = true;
                finder.selectActionFunction = function (fileUrl) {
                    $scope.$apply(function () {
                        $scope.new.Image = fileUrl;
                    })
                }
                finder.popup();
            }

            $scope.UpdateNew = function () {
                apiService.put('api/new/update', $scope.new, function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được chỉnh sửa');
                    $uibModalInstance.close();
                    $state.go("home");
                    $timeout(function () {
                        $state.go("news");
                    }, 0.01);
                }, function (error) {
                    notificationService.displayError('Không sửa được bản ghi.');
                })
            }
        }])
})(angular.module('accountantnew.news'))