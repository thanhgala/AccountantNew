(function (app) {
    app.controller('newsListController', ['$scope', 'apiService', '$ngBootbox', '$filter', 'commonService', '$uibModal', 'shareIDService', 'notificationService', '$timeout', '$state', 'authData',
    function ($scope, apiService, $ngBootbox, $filter, commonService, $uibModal, shareIDService, notificationService, $timeout, $state, authData) {
            $scope.keyword = '';
            $scope.loading = true;

            var pageSize = 4;

            $scope.data = [];

            $scope.getNews = getNews;

            $scope.getNewsApproval = getNewsApproval;

            $scope.isAll = false;

            $scope.isToggle = false;

            $scope.pageCurrent;

            function getNews(page) {
                $scope.toggleName = 'DS bài viết cần phê duyệt'
                $scope.isToggle = false;
                if (!shareIDService.getIsEdit()) {
                    $scope.pageCurrent = page || 0;
                }else{
                    $scope.pageCurrent = shareIDService.getPageCurrent()
                    shareIDService.setIsEdit(false);
                    shareIDService.setPageCurrent(null);
                }
                $scope.loading = true;
                var config = {
                    params: {
                        keyword:$scope.keyword,
                        page: $scope.pageCurrent,
                        pageSize: pageSize
                    }
                }
                apiService.get('api/new/getall', config, function (result) {
                    if (result.data.TotalCount === 0) {
                        notificationService.displayWarning('Không có bản ghi nào được tìm thấy.');
                    }
                    $scope.loading = false;
                    $scope.data = result.data.Items;
                    $scope.page = result.data.Page;
                    $scope.pagesCount = result.data.TotalPages;
                    $scope.totalCount = result.data.TotalCount;
                    $scope.countInPage = result.data.Count;
                    $scope.totalApproval = result.data.TotalApproval;

                    shareIDService.setMaxPage($scope.pagesCount - 1);
                    shareIDService.setCountInPage($scope.totalCount % pageSize);
                }, function () {
                    $scope.loading = false;
                    console.log('Cannot get list category');
                });
            }

            getNews();

            function getNewsApproval(page) {
                $scope.toggleName = 'DS bài viết'
                $scope.isToggle = true;
                if (!shareIDService.getIsEdit()) {
                    $scope.pageCurrent = page || 0;
                } else {
                    $scope.pageCurrent = shareIDService.getPageCurrent()
                    shareIDService.setIsEdit(false);
                    shareIDService.setPageCurrent(null);
                }
                $scope.loading = true;
                var config = {
                    params: {
                        keyword: $scope.keyword,
                        page: $scope.pageCurrent,
                        pageSize: pageSize
                    }
                }
                apiService.get('api/new/getallapproval', config, function (result) {
                    if (result.data.TotalCount === 0) {
                        notificationService.displayWarning('Không có bản ghi nào được tìm thấy.');
                    }
                    $scope.loading = false;
                    $scope.data = result.data.Items;
                    $scope.page = result.data.Page;
                    $scope.pagesCount = result.data.TotalPages;
                    $scope.totalCount = result.data.TotalCount;
                    $scope.countInPage = result.data.Count;

                    shareIDService.setMaxPage($scope.pagesCount - 1);
                    shareIDService.setCountInPage($scope.totalCount % pageSize);
                }, function () {
                    $scope.loading = false;
                    console.log('Cannot get list category');
                });
            }

            function checkIsAdmin() {
                if (authData.authenticationData.isAdmin === "True") {
                    $scope.isAdmin = true;
                }
            }
            checkIsAdmin();

            $scope.toggleFunc = function () {
                $scope.isToggle = !$scope.isToggle;
                if ($scope.isToggle) {
                    getNewsApproval();
                    $scope.toggleName = 'DS bài viết';
                }
                else {
                    getNews();
                    $scope.toggleName = 'DS bài viết phê duyệt';
                }
            }

            $scope.deleteNews = function (id) {
                $ngBootbox.confirm('Bạn có chắc muốn xóa?.').then(function () {
                    var config = {
                        params: {
                            id: id
                        }
                    }
                    apiService.del('/api/new/delete', config, function (result) {
                        notificationService.displaySuccess(result.data.Name + ' xóa thành công');
                        if ($scope.countInPage === 1 && $scope.totalCount > 1) {
                            getNews($scope.pageCurrent - 1);
                        }
                        else {
                            getNews($scope.pageCurrent);
                        }
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

            $scope.deleteMultiple =  function() {
                var listID = [];
                angular.forEach($scope.selected, function (item) {
                    listID.push(item.ID);
                });

                var config = {
                    params: {
                        checkedProduct: JSON.stringify(listID)
                    }
                }
                apiService.del('api/new/deletemulti', config, function (result) {
                    notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi');
                    getNews();
                }, function (error) {
                    notificationService.displayWarning('Xóa không thành công.')
                });
            }

            $scope.openUpdatePoup = function (id) {
                $uibModal.open({
                    animation: true,
                    templateUrl: '/app/components/news/newsEditViewPoup.html',
                    size: 'lg',
                    controller: 'newsEditController'
                })
                shareIDService.setPageCurrent($scope.pageCurrent);
                shareIDService.setID(id);
            }

            $scope.openAddPoup = function () {
                $uibModal.open({
                    animation: true,
                    backdrop:false,
                    templateUrl: '/app/components/news/newsAddViewPoup.html',
                    size: 'lg',
                    controller: 'newsAddController'
                })
                //shareIDService.setIsAdd(true);
            }
        }])
})(angular.module('accountantnew.news'))