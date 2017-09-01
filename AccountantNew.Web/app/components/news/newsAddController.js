//(function (app) {
//    app.controller('newsAddController', newsAddController);
(function (app) {
    app.controller('newsAddController', ['$scope', 'apiService', '$state', 'commonService', 'notificationService', '$uibModalInstance', '$timeout', 'shareIDService',
        function ($scope, apiService, $state, commonService, notificationService, $uibModalInstance, $timeout, shareIDService) {

            $scope.flatFolders = [];
            $scope.newcategories = [];

            //cấu hình ckeditor
            $scope.ckeditorOptions = {
                language: 'vi',
                height: '200px',
            }

            $scope.new = {
                CreatedDate: new Date(),
                Private: false,
                Status:true
            }

            $scope.GetAlias = function() {
                $scope.new.Alias = commonService.getSeoTitle($scope.new.Name);
            };

            $scope.close = function () {
                $uibModalInstance.close();
            }

            function loadNewCategory() {
                apiService.get('api/newcategory/getnewscategory', null, function (result) {
                    $scope.newcategories = commonService.getTree(result.data, "ID","ParentID");
                    $scope.newcategories.forEach(function (item) {
                        commonService.recur(item, 0, $scope.flatFolders);
                    });
                }, function (error) {
                    console.log('cannot get list parent');
                })
            }
           
            loadNewCategory();

            $scope.ChooseImage = function () {
                var finder = new CKFinder();
                finder.readOnly = false;
                finder.selectActionFunction = function (fileUrl) {
                    $scope.$apply(function () {
                        $scope.new.Image = fileUrl;
                    })
                }
                finder.popup();
            }

            $scope.AddNew = function() {
                apiService.post('api/new/create', $scope.new, function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được thêm mới');
                    $uibModalInstance.close();
                    $state.go("home");
                    shareIDService.setPageCurrent(0)
                    $timeout(function () {
                        $state.go("news");
                    }, 0.01);

                }, function (error) {
                    notificationService.displayError('Không thêm được bản ghi.');
                })
            }
        }])
})(angular.module('accountantnew.news'))