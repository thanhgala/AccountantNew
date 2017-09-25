var category = {
    init: function () {
        category.loadData();
        category.registerEvent();

    },
    registerEvent: function () {
        
    },
    loadData: function () {
        $.ajax({
            url: '/File/GetJsonCategory',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                $('#treeview')
                    .on('changed.jstree', function (e, data) {
                        var objNode = data.instance.get_node(data.selected);
                        //alert(objNode.id + '-' + objNode.text);

                        //var c = null;
                        //c = data.changed.selected;
                        //alert(c + ' ');

                        category.getListFile(objNode.id);
                    })
                    .jstree({
                        //'plugins': ["contextmenu", "changed", "checkbox"],
                        'plugins': ["contextmenu"],
                    'core': {
                        'data': response.data,
                        'themes': {
                            'name': 'proton',
                            'responsive': true
                        }
                    },
                });
            }
        }); 
    },

    getListFile:function(id){
        $.ajax({
            url: '/File/GetListFile',
            type: 'POST',
            data: {
                idPermission: id
            },
            success: function (response) {
                if (response) {
                    $('#listFile').html(response);
                } else {
                    alert("Bạn không có quyền truy cập vào đây");
                    $('#listFile').html("<p>Bạn không có quyền truy cập vào đây</p>");
                }
            }
        })
    }
}

category.init();