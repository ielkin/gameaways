$(".account-menu-container").tabs().addClass("ui-tabs-vertical ui-helper-clearfix");
$(".account-menu-container li").removeClass("ui-corner-top").addClass("ui-corner-left");

$(document).ready(function () {
    if (getUrlParameter('tab') != null) {
        $("#" + getUrlParameter('tab')).trigger("click");
    }
    else {
        $(".manage-account-list a").first().trigger("click");
    }
});

function getUrlParameter(variable) {
    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] == variable) { return pair[1]; }
    }
    return (false);
}