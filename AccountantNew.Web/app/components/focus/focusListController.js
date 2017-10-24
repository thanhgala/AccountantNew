(function (app) {
    app.controller('focusListController', ['$scope', 'apiService', '$ngBootbox', '$filter', 'commonService', '$uibModal', 'shareIDService', 'notificationService', '$timeout', '$state',
         function ($scope, apiService, $ngBootbox, $filter, commonService, $uibModal, shareIDService, notificationService, $timeout, $state) {

             $scope.keyword = '';
             $scope.loading = true;

             var pageSize = 20;

             $scope.data = [];

             $scope.getFocus = getFocus;

             $scope.isAll = false;

             var pageCurrent;

             function getFocus(page) {
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
                 apiService.get('api/focus/getall', config, function (result) {
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
             getFocus();

             $scope.deleteFocus = function (id) {
                 $ngBootbox.confirm('Bạn có chắc muốn xóa?.').then(function () {
                     var config = {
                         params: {
                             id: id
                         }
                     }
                     apiService.del('/api/focus/delete', config, function (result) {
                         notificationService.displaySuccess('Tiêu điểm xóa thành công');
                         function checkFocus(focus) {
                             return focus.ID === id;
                         }
                         var indexDelete = $scope.data.findIndex(checkFocus)
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
                             checkedFocus: JSON.stringify(listID)
                         }
                     }
                     apiService.del('api/focus/deletemulti', config, function (result) {
                         notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi');
                         angular.forEach(listID, function (item) {
                             function checkFocus(focus) {
                                 return focus.ID === item;
                             }
                             var indexDelete = $scope.data.findIndex(checkFocus)
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
                     templateUrl: '/app/components/focus/focusAddViewPoup.html',
                     controller: 'focusAddController'
                 })
             }

             $scope.openUpdatePoup = function (id) {
                 $uibModal.open({
                     animation: true,
                     templateUrl: '/app/components/focus/focusEditViewPoup.html',
                     controller: 'focusEditController'
                 })
                 shareIDService.setPageCurrent($scope.page);
                 shareIDService.setID(id);
             }
         }])
})(angular.module('accountantnew.focus'))