(function (app) {
    app.controller('filesPdfController', ['$scope', '$http', 'apiService', '$uibModal', '$ngBootbox', '$filter', 'commonService', 'shareIDService', 'notificationService', '$timeout', '$state', '$stateParams',
    function ($scope, $http, apiService, $uibModal, $ngBootbox, $filter, commonService, shareIDService, notificationService, $timeout, $state, $stateParams) {

        $scope.filePdf = [];

        $scope.flatFolders = [];

        $scope.newcategories = [];

        $scope.status = true;

        $scope.loading = true;

        $scope.timeStarted = new Date();

        $scope.$on("fileSelected", function (event, args) {
            $scope.filePdf.push(args.file);
            $scope.$apply();
        });

        $scope.cancel = function () {
            $scope.filePdf = [];
        }

        //$scope.viewer = pdf.Instance("viewer");

        //$scope.nextPage = function () {
        //    $scope.viewer.nextPage();
        //};

        //$scope.prevPage = function () {
        //    $scope.viewer.prevPage();
        //};

        //$scope.pageLoaded = function (curPage, totalPages) {
        //    $scope.currentPage = curPage;
        //    $scope.totalPages = totalPages;
        //};

        function getListFile() {
            apiService.get('api/file/getlistfile/' + $stateParams.action, null, function (result) {
                $scope.data = result.data;
                $scope.loading = false;
            }, function () {
                console.log('Cannot get list category');
            });
        }
        getListFile();

        $scope.addFile = function () {
            var folder = $stateParams.folder;
            var action = $stateParams.action;
            angular.element(document).find('#addFile').modal('hide');
            $scope.loading = true;
            $http({
                method: 'POST',
                url: "/api/file/import",
                headers: { 'Content-Type': undefined },
                transformRequest: function (data) {
                    var formData = new FormData();
                    formData.append("status", angular.toJson(data.status));
                    formData.append("timeStarted", angular.toJson(data.timeStarted));
                    formData.append("folders", angular.toJson(data.folders));
                    formData.append("categoryId", angular.toJson(data.categoryId));
                    for (var i = 0; i < data.files.length; i++) {
                        //add each file to the form data and iteratively name them
                        formData.append("file" + i, data.files[i]);
                    }
                    return formData;
                },
                data: { status: $scope.status, timeStarted: $scope.timeStarted.toDateString(), folders: folder, categoryId: action, files: $scope.filePdf }
                //data: { data }
            }).then(function (result, status, headers, config) {
                notificationService.displaySuccess(result.data);
                //var close = $('#addFile').modal('hide');
                //setTimeout(function () {
                //    $state.go("home");
                //    $timeout(function () {
                //        $state.go("files.action", { "folder": folder, "action": action });
                //    }, 20);
                //}, 300, close);
                angular.element("input[type='file']").val(null);
                $scope.filePdf = [];
                $scope.timeStarted = new Date();
                $scope.loading = false;
                getListFile();
            },
            function (data, status, headers, config) {
                notificationService.displayError(data.data);
                $scope.loading = false;
            });
        }

        function loadFileCategory() {
            apiService.get('api/newcategory/getallfilecategory', null, function (result) {
                $scope.newcategories = commonService.getTree(result.data, "ID", "ParentID");
                $scope.newcategories.forEach(function (item) {
                    commonService.recur(item, 0, $scope.flatFolders);
                });
            }, function (error) {
                console.log('cannot get list parent');
            })
        }

        $scope.getPdfDetail = function (item) {
            $scope.pdfDetail = item;
            $scope.pdfDetail.TimeStarted = new Date($scope.pdfDetail.TimeStarted);
            $scope.flatFolders = [];
            loadFileCategory();
        }

        $scope.updateFile = function () {
            apiService.put('api/file/update', $scope.pdfDetail, function (result) {
                notificationService.displaySuccess(result.data);
                angular.element(document).find('#updateFile').modal('hide');
                getListFile();
            }, function (err) {
                notificationService.displayError(err.data);
            })
        }

        $scope.openPdf = function (item) {
            $uibModal.open({
                animation: true,
                templateUrl: '/app/components/files/pdfViewDetailPoup.html',
                size: 'lg',
                controller: function ($scope) {
                    //$scope.data = item;
                    $scope.getPath = function () {
                        return item.Path + '#toolbar=0&navpanes=0&statusbar=0&zoom=85';
                    };
                }
            })
        }

        $scope.deletePdf = function (id) {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?.').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('/api/file/delete', config, function (result) {
                    notificationService.displaySuccess(result.data);
                    getListFile();
                }, function (err) {
                    notificationService.displayError(err.data);
                })
            });
        }
    }])
})(angular.module('accountantnew.files'))