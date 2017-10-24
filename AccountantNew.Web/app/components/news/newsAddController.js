//(function (app) {
//    app.controller('newsAddController', newsAddController);
(function (app) {
    app.controller('newsAddController', ['$scope', 'apiService', '$state', 'commonService', 'notificationService', '$uibModalInstance', '$timeout', 'shareIDService', 'authData',
        function ($scope, apiService, $state, commonService, notificationService, $uibModalInstance, $timeout, shareIDService, authData) {

            $scope.flatFolders = [];
            $scope.newcategories = [];

            //cấu hình ckeditor
            $scope.ckeditorOptions = {
                language: 'vi',
                height: '200px',
            }

            $scope.new = {
                ApplicationUserId: authData.authenticationData.id
            }

            $scope.GetAlias = function () {
                $scope.new.Alias = commonService.getSeoTitle($scope.new.Name);
            };

            $scope.close = function () {
                $uibModalInstance.close();
            }

            function checkIsAdmin() {
                if (authData.authenticationData.isAdmin === "True") {
                    $scope.new.Status = true;
                    $scope.isAdmin = true;
                } 
                //angular.forEach(JSON.parse(authData.authenticationData.groups), function (item) {
                //    if (item.Name === "SupperAdmin") {
                //        $scope.new.Status = true;
                //        $scope.isAdmin = true;
                //    }
                //})    
            }
            checkIsAdmin();

            function loadNewCategory() {
                apiService.get('api/newcategory/getnewscategory', null, function (result) {
                    //$scope.newcategories = commonService.getTree(result.data, "ID","ParentID");
                    //$scope.newcategories.forEach(function (item) {
                    //    commonService.recur(item, 0, $scope.flatFolders);
                    //});
                    $scope.newcategories = result.data;
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

            $scope.AddNew = function () {
                apiService.post('api/new/create', $scope.new, function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được thêm mới');
                    $uibModalInstance.close();
                    $state.go("home");
                    $timeout(function () {
                        $state.go("news");
                    }, 100);

                }, function (error) {
                    notificationService.displayError('Không thêm được bản ghi.');
                })
            }
        }])
})(angular.module('accountantnew.news'))