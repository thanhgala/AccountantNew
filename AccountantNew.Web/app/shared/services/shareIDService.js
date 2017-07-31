(function (app) {
    app.factory('shareIDService', shareIDService);

    shareIDService.$inject = ['apiService'];

    function shareIDService(apiService) {
        var data = {
            idNew: '',
            pageCurrent: 0,
            isAdd: false,
            maxPage: '',
            countInPage:''
        };

        return {
            getID: function () {
                return data.idNew;
            },
            setID: function (id) {
                data.idNew = id;
            },
            getPageCurrent: function () {
                return data.pageCurrent;
            },
            setPageCurrent: function (page) {
                data.pageCurrent = page;
            },
            getIsAdd: function () {
                return data.isAdd;
            },
            setIsAdd: function(bool){
                data.isAdd = bool;
            },
            getMaxPage: function () {
                return data.maxPage;
            },
            setMaxPage: function (max) {
                data.maxPage = max;
            },
            getCountInPage: function () {
                return data.countInPage;
            },
            setCountInPage: function (count) {
                data.countInPage = count;
            }
        };
    }
})(angular.module('accountantnew.common'));