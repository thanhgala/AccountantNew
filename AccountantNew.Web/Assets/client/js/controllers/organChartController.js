getOrgChart.themes.myCustomTheme =
    {
        size: [270, 400],
        toolbarHeight: 46,
        textPoints: [
            { x: 130, y: 50, width: 250 },
            { x: 130, y: 120, width: 250 }
        ],
        textPointsNoImage: [
            { x: 130, y: 50, width: 250 },
            { x: 130, y: 120, width: 250 }
        ],
        expandCollapseBtnRadius: 20,
        defs: '<filter id="f1" x="0" y="0" width="200%" height="200%"><feOffset result="offOut" in="SourceAlpha" dx="5" dy="5" /><feGaussianBlur result="blurOut" in="offOut" stdDeviation="5" /><feBlend in="SourceGraphic" in2="blurOut" mode="normal" /></filter>',
        box: '<rect x="0" y="0" height="400" width="270" rx="10" ry="10" class="myCustomTheme-box" filter="url(#f1)"  />',
        text: '<text text-anchor="middle" width="[width]" class="get-text get-text-[index]" x="[x]" y="[y]">[text]</text>',
        image: '<clipPath id="getMonicaClip"><circle cx="135" cy="255" r="85" /></clipPath><image preserveAspectRatio="xMidYMid slice" clip-path="url(#getMonicaClip)" xlink:href="[href]" x="50" y="150" height="190" width="170"/>'
    };

var readUrl = "/Pages/Read";
var updateUrl = "/Pages/Update";
var updateImage = "/Pages/UpdateImage";

var peopleElement = document.getElementById("people");

var userID = "@HttpContext.Current.User.Identity.GetUserId()";

if (userID == '272557435') {
    var orgChart = new getOrgChart(peopleElement, {
        theme: "myCustomTheme",
        enableGridView: true,
        primaryFields: ["Name", "Position"],
        photoFields: ["Image"],
        clickNodeEvent: clickHandler,
        updatedEvent: updatedEvent
    });
} else {
    var orgChart = new getOrgChart(peopleElement, {
        theme: "myCustomTheme",
        enableGridView: true,
        enableEdit: false,
        primaryFields: ["Name", "Position"],
        photoFields: ["Image"],
        clickNodeEvent: clickHandler,
        updatedEvent: updatedEvent
    });
}


$.getJSON(readUrl, function (data) {
    orgChart.loadFromJSON(data);
    $("#dialog").dialog({
        modal: true,
        bgiframe: true,
        width: 500,
        height: 200,
        autoOpen: false,
        "closeX": false,
        closeOnEscape: false
    });
});

function updatedEvent(sender, args) {
    var model = [];
    for (var id in orgChart.nodes) {
        var node = orgChart.nodes[id];
        var data = $.extend(true, { ID: node.id, ParentID: node.pid }, node.data);
        model.push(data);
    }

    var model = JSON.stringify({ 'model': model });

    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        url: updateUrl,
        data: model
    });
}

function clickHandler(sender, args) {
    $("#imagefile").trigger('click');
    $('input[type=file]').change(function (event) {
        //var tmppath = URL.createObjectURL(event.target.files[0]);
        $("#dialog").dialog('option', 'buttons', {
            "Confirm": function () {
                var formData = new FormData();
                var imagefile = document.getElementById("imagefile").files[0];
                formData.append("imagefile", imagefile);
                formData.append("id", args.node.id);
                $.ajax({
                    type: 'POST',
                    url: updateImage,
                    dataType: 'json',
                    contentType: false,
                    processData: false, // Not to process data
                    data: formData,
                    success: function (result) {
                        window.location.reload(true);
                    },
                    error: function (err) {
                        alert(err.statusText);
                    }
                });
            },
            "Cancel": function () {
                window.location.reload(true);
            }
        });
        $("#dialog").dialog("open");
    });
}