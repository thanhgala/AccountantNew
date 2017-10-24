(function (app) {
    app.controller('applicationFileController', ['$scope', 'apiService', '$uibModalInstance', '$uibModal', '$filter', '$ngBootbox', '$timeout', 'shareIDService', 'notificationService',
        function ($scope, apiService, $uibModalInstance, $uibModal, $filter, $ngBootbox, $timeout, shareIDService, notificationService) {

            $scope.isAll = false;
            $scope.childcategories = [];
            //$scope.selected = [];
            var checked;

            function loadRootFileParent() {
                apiService.get('/api/newcategory/getrootfileparent/', null, function (result) {
                    $scope.categories = result.data;
                    $timeout(function () {
                        apiService.get('/api/newcategory/getchildrootparent/' + 4, null, function (result) {
                            $scope.childcategories = result.data;
                            loadCategoryGroup($scope.childcategories);
                        }, function (error) {
                            notificationService.displayError(error.data);
                        });
                    }, 0.1);
                }, function (error) {
                    notificationService.displayError(error.data);
                });
            }
            loadRootFileParent();

            function loadCategoryGroup(childcategories) {
                apiService.get('/api/applicationGroup/getcategorygroup/' + shareIDService.getID(), null, function (result) {
                    $scope.categorygroup = result.data;
                    angular.forEach(childcategories, function (item) {
                        angular.forEach($scope.categorygroup, function (jtem) {
                            if (item.ID === jtem) {
                                item.checked = true;
                                return;
                            }
                        })
                    })
                }, function (error) {
                    notificationService.displayError(error.data);
                });
            }

            $scope.ConfigFilePermission = function (id) {
                $scope.isFilePermission = true;
                apiService.get('/api/newcategory/getchildrootparent/' + id, null, function (result) {
                    $scope.childcategories = result.data;
                    loadCategoryGroup($scope.childcategories);
                }, function (error) {
                    notificationService.displayError(error.data);
                });
            }

            $scope.ConfigSupportPermission = function (id) {
                $scope.isFilePermission = false;
                apiService.get('/api/newcategory/getchildsupportparent/' + id, null, function (result) {
                    $scope.childcategories = result.data;
                    loadCategoryGroup($scope.childcategories);
                }, function (error) {
                    notificationService.displayError(error.data);
                });
            }

            $scope.selectAll = function (event) {
                var listID = [];
                if (event.target.checked) {
                    angular.forEach($scope.childcategories, function (item) {
                        item.checked = true;
                        listID.push(item.ID);
                    })
                    var config = {
                        params: {
                            checkedCateID: JSON.stringify(listID),
                            groupID: shareIDService.getID(),
                            add: true
                        }
                    }
                }
                else {
                    angular.forEach($scope.childcategories, function (item) {
                        item.checked = false;
                        listID.push(item.ID);
                    })
                    var config = {
                        params: {
                            checkedCateID: JSON.stringify(listID),
                            groupID: shareIDService.getID(),
                            add: false
                        }
                    }
                }
                $scope.isAll = event.target.checked;
                
                apiService.get('/api/applicationGroup/updatepermissionfile', config, function (result) {
                    if (!listID.length) {
                        notificationService.displaySuccess('Hủy quyền truy cập file thành công');
                    } else {
                        notificationService.displaySuccess('Cập nhật quyền truy cập file thành công');
                    }
                    
                }, function (error) {
                    notificationService.displayWarning('Cập nhật không thành công.')
                });

            }

            $scope.selectSingle = function (event, id) {
                //for (var index in $scope.childcategories)
                //    if (!$scope.childcategories[index].checked) {
                //        $scope.isAll = false;
                //        return;
                //    }
                //$scope.isAll = true;
                var config = {
                    params: {
                        checkedCateID: id,
                        groupID: shareIDService.getID()
                    }
                }
                if (event.target.checked) {
                    apiService.get('/api/applicationGroup/addpermissionfile', config, function (result) {
                        notificationService.displaySuccess('Thêm quyền truy cập file thành công');
                    }, function (error) {
                        notificationService.displayWarning('Cập nhật không thành công.')
                    });
                } else {
                    apiService.del('/api/applicationGroup/deletepermissionfile', config, function (result) {
                        notificationService.displaySuccess('Hủy quyền truy cập file thành công');
                    }, function (error) {
                        notificationService.displayWarning('Cập nhật không thành công.')
                    });
                }
            }

            $scope.$watch("childcategories", function (n, o) {
                checked = $filter('filter')(n, { checked: true });
                if (checked.length) {
                    $scope.selected = checked;
                    if (checked.length === $scope.childcategories.length) {
                        $scope.isAll = true;
                    } else {
                        $scope.isAll = false;
                    }
                    $('#btnSubmit').removeAttr('disabled');
                } else {
                    $('#btnSubmit').attr('disabled', 'disabled');
                    $scope.isAll = false;
                }
            }, true)

            //$scope.addFilePermission = function () {
            //    var listID = [];
            //    angular.forEach($scope.selected, function (item) {
            //        listID.push(item.ID);
            //    });

            //    console.log(JSON.stringify(listID) + ' ');

            //    var config = {
            //        params: {
            //            checkedCateID: JSON.stringify(listID),
            //            groupID:shareIDService.getID()
            //        }
            //    }

            //    apiService.get('/api/applicationGroup/addpermissionfile', config, function (result) {
            //        notificationService.displaySuccess('Cập nhật quyền truy cập file thành công');
            //    }, function (error) {
            //        notificationService.displayWarning('Cập nhật không thành công.')
            //    });
            //}

            $scope.close = function () {
                $uibModal.open({
                    animation: true,
                    keyboard: false,
                    backdrop: 'static',
                    templateUrl: '/app/components/application_groups/applicationGroupsEditPoup.html',
                    size: 'lg',
                    controller: 'applicationGroupsEditController'
                });
                $uibModalInstance.close();
            }
        }]);
})(angular.module('accountantnew.application_groups'))