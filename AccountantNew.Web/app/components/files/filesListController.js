(function (app) {
    app.controller('filesListController', ['$scope', 'apiService', '$ngBootbox', '$filter', 'commonService', '$uibModal', 'shareIDService', 'notificationService', '$timeout', '$state','$stateParams',
    function ($scope, apiService, $ngBootbox, $filter, commonService, $uibModal, shareIDService, notificationService, $timeout, $state,$stateParams) {

        function getFileChildCategory() {
            apiService.get('api/newcategory/getchildrootparent/' + $stateParams.id, null, function (result) {
                $scope.data = result.data;
            }, function () {
                console.log('Cannot get list category');
            });
        }
        getFileChildCategory();


        $scope.OpenFolder = function (name, id) {

            function findCate(item) {
                return item.ID === id;
            }
            var myData = $scope.data;
            $scope.itemData = $scope.data.find(findCate);
            if ($scope.itemData.Child) {
                $state.go("files", { "folder": name, "id": id });
            }
            else {
                $state.go("files.action", { "folder": name, "action": id });
            }
        }
    }])
})(angular.module('accountantnew.files'))