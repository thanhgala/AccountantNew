(function (app) {
    app.controller('postsListController', ['$scope', 'apiService', '$ngBootbox', '$filter', 'commonService', '$uibModal', 'shareIDService', 'notificationService', '$timeout', '$state',
         function ($scope, apiService, $ngBootbox, $filter, commonService, $uibModal, shareIDService, notificationService, $timeout, $state) {

             $scope.keyword = '';
             $scope.loading = true;

             var pageSize = 20;

             $scope.data = [];

             $scope.getPost = getPost;

             $scope.isAll = false;

             var pageCurrent;

             function getPost(page) {
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
                         keyword: $scope.keyword,
                         page: pageCurrent,
                         pageSize: pageSize
                     }
                 }
                 apiService.get('api/post/getall', config, function (result) {
                     if (result.data.TotalCount === 0) {
                         notificationService.displayWarning('Không có bản ghi nào được tìm thấy.');
                     }
                     $scope.loading = false;
                     $scope.data = result.data.Items;
                     $scope.page = result.data.Page;
                     $scope.pagesCount = result.data.TotalPages;
                     $scope.totalCount = result.data.TotalCount;
                     $scope.countInPage = result.data.Count;
                 }, function () {
                     $scope.loading = false;
                     console.log('Cannot get list category');
                 });
             }
             getPost();

             $scope.getPostDetail = function (item) {
                 document.getElementById("detail").innerHTML = item.Content;
                 //$scope.postDetail = item.Content;
             }

             $scope.deletePost = function (id) {
                 $ngBootbox.confirm('Bạn có chắc muốn xóa?.').then(function () {
                     var config = {
                         params: {
                             id: id
                         }
                     }
                     apiService.del('/api/post/delete', config, function (result) {
                         notificationService.displaySuccess('Bài đăng xóa thành công');
                         function checkPost(post) {
                             return post.ID === id;
                         }
                         var indexDelete = $scope.data.findIndex(checkPost)
                         $scope.data.splice(indexDelete, 1);
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
                 $ngBootbox.confirm('Bạn có chắc muốn xóa?.').then(function () {
                     var listID = [];
                     angular.forEach($scope.selected, function (item) {
                         listID.push(item.ID);
                     });

                     var config = {
                         params: {
                             checkedPosts: JSON.stringify(listID)
                         }
                     }
                     apiService.del('api/post/deletemulti', config, function (result) {
                         notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi');
                         angular.forEach(listID, function (item) {
                             function checkPosts(post) {
                                 return post.ID === item;
                             }
                             var indexDelete = $scope.data.findIndex(checkPosts)
                             $scope.data.splice(indexDelete, 1);
                         })
                     }, function (error) {
                         notificationService.displayWarning('Xóa không thành công.')
                     });
                 }
             )};
         }])
})(angular.module('accountantnew.posts'))