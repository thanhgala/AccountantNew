(function (app) {
    app.controller('notificationsListController', ['$scope', 'apiService', '$ngBootbox', '$filter', 'commonService', '$uibModal', 'shareIDService', 'notificationService', '$timeout', '$state',
         function ($scope, apiService, $ngBootbox, $filter, commonService, $uibModal, shareIDService, notificationService, $timeout, $state) {

             $scope.keyword = '';
             $scope.loading = true;

             var pageSize = 4;

             $scope.data = [];

             $scope.getNotifications = getNotifications;

             $scope.isAll = false;

             var pageCurrent;

             function getNotifications(page) {
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
                 apiService.get('api/notification/getallnotification', config, function (result) {
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
             getNotifications();

             $scope.deleteNotifications = function (id) {
                 $ngBootbox.confirm('Bạn có chắc muốn xóa?.').then(function () {
                     var config = {
                         params: {
                             id: id
                         }
                     }
                     apiService.del('/api/notification/delete', config, function (result) {
                         notificationService.displaySuccess(result.data.Name + ' xóa thành công');
                         function checkNotification(notification) {
                             return notification.ID === id;
                         }
                         var indexDelete = $scope.data.findIndex(checkNotification)
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
                             checkedProduct: JSON.stringify(listID)
                         }
                     }
                     apiService.del('api/notification/deletemulti', config, function (result) {
                         notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi');
                         angular.forEach(listID, function (item) {
                             function checkNotification(notification) {
                                 return notification.ID === item;
                             }
                             var indexDelete = $scope.data.findIndex(checkNotification)
                             $scope.data.splice(indexDelete, 1);
                         })
                     }, function (error) {
                         notificationService.displayWarning('Xóa không thành công.')
                     });
                 }
             )};

             $scope.openAddPoup = function () {
                 $uibModal.open({
                     animation: true,
                     backdrop: false,
                     templateUrl: '/app/components/notifications/notificationsAddViewPoup.html',
                     size: 'lg',
                     controller: 'notificationsAddController'
                 })
             }

             $scope.openUpdatePoup = function (id) {
                 $uibModal.open({
                     animation: true,
                     templateUrl: '/app/components/notifications/notificationsEditViewPoup.html',
                     size: 'lg',
                     controller: 'notificationsEditController'
                 })
                 shareIDService.setPageCurrent($scope.page);
                 shareIDService.setID(id);
             }
         }])
})(angular.module('accountantnew.notifications'))