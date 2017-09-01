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
                        category.getListFile(objNode.id);
                    })
                    .jstree({
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
                id: id
            },
            success: function (response) {
                $('#listFile').html(response);
            }
        })
    }
}

category.init();