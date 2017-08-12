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
                        //alert(objNode.id + '-' + objNode.text)
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
        
    }
}

category.init();