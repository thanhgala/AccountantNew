var searchAuto = {
    init: function () {
        searchAuto.registerEvents();
    },
    registerEvents: function () {
        $("#txtKeyword").autocomplete({
            minLength: 2,
            source: function (request, response) {
                $.ajax({
                    url: "/New/GetNewByName",
                    dataType: "json",
                    data: {
                        keyword: request.term
                    },
                    success: function (res) {
                        response(res.data);
                    }
                });
            },
            //focus: function (event, ui) {
            //    $("#txtKeyword").val(ui.item.Name);
            //    return false;
            //},
            select: function (event, ui) {
                $("#txtKeyword").val(ui.item.Name);
                return false;
            }
        }).autocomplete("instance")._renderItem = function (ul, item) {
            return $("<li>")
              .append("<img id='imageClass' src=" + item.Img + " alt= " + item.Name + '/>')
              .append("<a href='" + item.NewCategory + '-' + item.Alias + '-' + item.ID + '.html' + "'>" + item.Name + "</a>")
              .appendTo(ul);
        };
    }
}
searchAuto.init();