﻿(function (app) {
    app.factory('shareIDService', shareIDService);

    shareIDService.$inject = ['apiService'];

    function shareIDService(apiService) {
        var data = {
            idNew: '',
            pageCurrent: 0,
            isAdd: false,
            isEdit:false,
            maxPage: '',
            countInPage: '',
            keyWord:''
        };

        return {
            getID: function () {
                return data.idNew;
            },
            setID: function (id) {
                data.idNew = id;
            },
            getRoleID: function () {
                return data.roleID;
            },
            setRoleID: function (id) {
                data.roleID = id;
            },
            getCateID: function () {
                return data.cateID;
            },
            setCateID: function (id) {
                data.cateID = id;
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
            getIsEdit: function () {
                return data.isEdit;
            },
            setIsEdit: function (bool) {
                data.isEdit = bool;
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
            },
            getKeyWord: function () {
                return data.keyWord;
            },
            setKeyWord: function (keyword) {
                data.keyWord = keyword;
            }

    };
}
})(angular.module('accountantnew.common'));