(function (app) {
    app.controller('applicationUsersListController', ['$scope', 'apiService', 'notificationService', '$ngBootbox',
        function ($scope, apiService, notificationService, $ngBootbox) {
            $scope.keyword = '';
            $scope.loading = true;

            $scope.data = [];

            $scope.getUsers = getUsers;

            function getUsers(page) {
                page = page || 0;
                $scope.loading = true;
                var config = {
                    params: {
                        keyword: $scope.keyword,
                        page: page,
                        pageSize: 3
                    }
                }
                apiService.get('api/applicationUser/getlistpaging', config, function (result) {
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
            getUsers();

            $scope.deleteItem = function (id) {
                $ngBootbox.confirm('Bạn có chắc muốn xóa?.').then(function () {
                    var config = {
                        params: {
                            id: id
                        }
                    }
                    apiService.del('/api/applicationUser/delete', config, function () {
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
})(angular.module('accountantnew.application_users'));