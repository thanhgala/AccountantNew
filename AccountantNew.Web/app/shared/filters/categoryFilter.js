(function (app) {
    //app.filter('categoryFilter', categoryFilter);

    //categoryFilter.$inject = ['apiService'];

    //function categoryFilter(apiService) {
    //    return function (id) {
    //        var kq = "alo";

    //        //apiService.get('api/newcategory/getid/' + id, null, function (result) {
    //        //    kq = result.data.Name
    //        //    return kq;
    //        //}, function (error) {
    //        //});
    //        if (id == 123)
    //            return kq;
    //        else
    //            return 'Khóa'
    //    }
    //}
    app.filter('categoryFilter', ['apiService', '$timeout', function (apiService, $timeout) {
        return function (input) {
            var kq = "dad";
            apiService.get('api/newcategory/getid/' + input, null, function (result) {

                }, function (error) {

                }
            );
            $timeout(function () {
                return kq;
                    }, 100);
            //if (input == 123)
            //    return 'Kích hoạt';
            //else
            //    return 'Khóa'
        }
    }]);
})(angular.module('accountantnew.common'));