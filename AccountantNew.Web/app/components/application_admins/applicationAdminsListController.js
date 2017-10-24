(function (app) {
    app.controller('applicationAdminsListController', ['$scope', 'apiService', 'notificationService', '$ngBootbox', 'shareIDService',
        function ($scope, apiService, notificationService, $ngBootbox, shareIDService) {
            $scope.keyword = shareIDService.getKeyWord();
            $scope.loading = true;

            $scope.data = [];

            $scope.getAdmins = getAdmins;

            var pageCurrent;

            function getAdmins(page) {
                if ($scope.keyword === '') {
                    shareIDService.setKeyWord('')
                } else if ($scope.keyword != '') {
                    shareIDService.setKeyWord($scope.keyword)
                }
                if (!shareIDService.getIsEdit()) {
                    pageCurrent = page || 0;
                } else {
                    pageCurrent = shareIDService.getPageCurrent()
                    shareIDService.setIsEdit(false);
                    shareIDService.setPageCurrent(null);
                }
                $scope.loading = true;
                var config = {
                    params: {
                        keyword: shareIDService.getKeyWord(),
                        page: pageCurrent,
                        pageSize: 20
                    }
                }
                apiService.get('api/applicationAdmin/getlistpaging', config, function (result) {
                    if (result.data.TotalCount === 0) {
                        notificationService.displayWarning('Không có bản ghi nào được tìm thấy.');
                    }
                    $scope.loading = false;
                    $scope.data = result.data.Items;
                    $scope.page = result.data.Page;
                    $scope.pagesCount = result.data.TotalPages;
                    $scope.totalCount = result.data.TotalCount;
                    $scope.countInPage = result.data.Count;
                }, function (error) {
                    $scope.loading = false;
                    console.log('Cannot get list category');
                });
            }
            getAdmins();

            $scope.deleteItem = function (id) {
                $ngBootbox.confirm('Bạn có chắc muốn xóa?.').then(function () {
                    var config = {
                        params: {
                            id: id
                        }
                    }
                    apiService.del('/api/applicationAdmin/delete', config, function () {
                        notificationService.displaySuccess('Đã xóa thành công.');
                        var dataS = $scope.data;
                        function checkUser(user) {
                            return user.Id === id;
                        }
                        var indexDelete = $scope.data.findIndex(checkUser)
                        $scope.data.splice(indexDelete, 1);
                    },
                    function () {
                        notificationService.displayError('Xóa không thành công.');
                    });
                });
            }
        }
    ])
})(angular.module('accountantnew.application_admins'));