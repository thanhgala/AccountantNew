(function (app) {
    app.controller('filesPdfController', ['$scope', '$http', 'apiService', '$uibModal', '$ngBootbox', '$filter', 'commonService', 'shareIDService', 'notificationService', '$timeout', '$state', '$stateParams', 'authData',
    function ($scope, $http, apiService, $uibModal, $ngBootbox, $filter, commonService, shareIDService, notificationService, $timeout, $state, $stateParams, authData) {

        $scope.filePdf = [];

        $scope.flatFolders = [];

        $scope.newcategories = [];

        $scope.status = false;

        $scope.loading = true;

        $scope.keyword = '';

        $scope.timeStarted = new Date();

        $scope.$on("fileSelected", function (event, args) {
            $scope.filePdf.push(args.file);
            $scope.$apply();
        });

        $scope.cancel = function () {
            $scope.filePdf = [];
        }

        $scope.getListFile = getListFile;
        $scope.getListFileApproval = getListFileApproval;

        function getListFile(page) {
            $scope.toggleName = 'DS file cần phê duyệt'
            $scope.isToggle = false;
            page = page || 0;
            $scope.loading = true;
            var config = {
                params: {
                    id: $stateParams.action,
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 20
                }
            }
            apiService.get('api/file/getlistfile', config, function (result) {
                if (result.data.TotalCount === 0) {
                    notificationService.displayWarning('Không có bản ghi nào được tìm thấy.');
                }
                $scope.data = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.countInPage = result.data.Count;
                $scope.folderName = $stateParams.folder.toUpperCase();
                $scope.totalApproval = result.data.TotalApproval;
                $scope.loading = false;
            }, function () {
                console.log('Cannot get list category');
            });
        }
        getListFile();

        function getListFileApproval(page) {
            $scope.toggleName = 'DS file'
            $scope.isToggle = true;
            page = page || 0;
            $scope.loading = true;
            var config = {
                params: {
                    id: $stateParams.action,
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 20
                }
            }
            apiService.get('api/file/getlistfileapproval', config, function (result) {
                if (result.data.TotalCount === 0) {
                    notificationService.displayWarning('Không có bản ghi nào được tìm thấy.');
                }
                $scope.data = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.countInPage = result.data.Count;
                $scope.folderName = $stateParams.folder.toUpperCase();
                $scope.loading = false;
            }, function () {
                console.log('Cannot get list category');
            });
        }

        function checkIsAdmin() {
            if (authData.authenticationData.isAdmin === "True") {
                $scope.isAdmin = true;
                $scope.status = true;
            }
        }
        checkIsAdmin();

        $scope.toggleFunc = function () {
            $scope.isToggle = !$scope.isToggle;
            if ($scope.isToggle) {
                getListFileApproval();
                $scope.toggleName = 'DS file';
            }
            else {
                getListFile();
                $scope.toggleName = 'DS file cần phê duyệt';
            }
        }

        //function getAllCate() {
        //    apiService.get('api/newcategory/getallparent', null, function (result) {
        //        $scope.dataCate = result.data;
        //    }, function () {
        //        console.log('Cannot get list category');
        //    });   
        //}
        //getAllCate();

        //$scope.filterData = function (id) {
        //    return $filter('filter')($scope.dataCate, { ID: id });
        //}

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
                    formData.append("createdBy", angular.toJson(authData.authenticationData.fullname))
                    for (var i = 0; i < data.files.length; i++) {
                        //add each file to the form data and iteratively name them
                        formData.append("file" + i, data.files[i]);
                    }
                    return formData;
                },
                data: { status: $scope.status, timeStarted: $scope.timeStarted.toDateString(), folders: folder, categoryId: action, files: $scope.filePdf, createdBy: authData.authenticationData.fullname }
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
            angular.element(document).find('#updateFile').modal('hide');
            $timeout(function () {
                apiService.put('api/file/update', $scope.pdfDetail, function (result) {
                    notificationService.displaySuccess(result.data);
                    getListFile();
                }, function (err) {
                    if (err.status === 400) {
                        notificationService.displayError('File này đã tồn tại trong thư mục bạn muốn di chuyển tới.');
                    } else {
                        notificationService.displayError(err.data);
                    }
                })
            }, 300);
        }

        $scope.openPdf = function (item) {
            apiService.get('api/file/detail', null, function (result) {
                $uibModal.open({
                    animation: true,
                    templateUrl: '/app/components/files/pdfViewDetailPoup.html',
                    size: 'lg',
                    controller: function ($scope) {
                        $scope.getPath = function () {
                            return item.Path + '#toolbar=0&navpanes=0&statusbar=0&zoom=85';
                        };
                    }
                })
            }, function (error) {
                console.log('cannot access');
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