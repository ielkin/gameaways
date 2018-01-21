$("#menu>li>a").click(function (event) {

    if ($(this).hasClass("menu-active") == false) {
        $("#menu>li>a.menu-active").parent().find("ul").hide();
        $("#menu>li>a.menu-active").removeClass("menu-active");

        $(this).parent().find("ul").show();
        $(this).addClass("menu-active");
    }
    else {
        $(this).parent().find("ul").hide();
        $(this).removeClass("menu-active");
    }
});

//$(".ga-user-icon").click(function (event) {

//    var userIcon = $(this);
//    var userName = userIcon.parent().find(".ga-username").text();

//    if (userName == $(".profile-preview .ga-username").text()) {
//        console.log(userName);
//        $(".profile-preview").css("left", userIcon.position().left - 255);
//        $(".profile-preview").css("top", userIcon.position().top - 165);
//        $(".profile-preview").show();
//    }
//    else {
//        $(".profile-preview").remove();
//        console.log("request sent");
//        // TODO: Check if widget should be rendered to the top or to bottom
//        $.get("/Account/Preview?username=" + userName, null, function (data) {
//            $(body).append(data);
//            $(".profile-preview").css("left", userIcon.position().left - 255);
//            $(".profile-preview").css("top", userIcon.position().top - 165);
//            $(".profile-preview").show();
//        });
//    }
//});

$.ajaxSetup({ cache: false });

$(document).click(function (e) {

    if (!$(e.target).is(".profile-preview,.profile-preview *,.ga-user-icon")) {
        $(".profile-preview").hide();
    }

    if (!$(e.target).is(".username")) {
        $("#UserMenu").slideUp(10);
        $(".username").removeClass("username-active");
    }

    if (!$(e.target).is(".notifications-icon, .notifications-list li .mark-as-read, .notifications-list li .dismiss-notification")) {
        $(".notifications-list-container").slideUp(10);
        $(".notifications-icon").removeClass("notifications-icon-active");
    }

    if (!$(e.target).is(".messages-icon, .messages-list .mark-as-read")) {
        $(".messages-list").slideUp(10);
        $(".messages-icon-active").removeClass("messages-icon-active");
    }

    if (!$(e.target).is(".ga-list-menu-container *,.reminder-container,.reminder-container *")) {
        $(".ga-list-menu-container .ga-list-menu-icon").not(".ga-list-menu-menu").hide();
        $(".ga-list-menu-container .ga-list-menu-menu").removeClass("active");
    }

    if (!$(e.target).is(".send-keys-popup *,.send-keys-button")) {
        $(".send-keys-popup").hide();
    }

    if (!$(e.target).is(".reminder-container,.reminder-container *,.remind-me,.ga-list-menu-remind")) {
        $(".reminder-container").hide();
    }

    if (!$(e.target).is(".report-problem, .report-problem-dialog, .report-problem-dialog *, .ui-autocomplete, .ui-autocomplete *")) {
        $(".report-problem-dialog").hide();
        $(".report-problem").css("margin-top", 0);
    }

    if (!$(e.target).is(".steam-trading-link-help, .steam-trading-link-help-content, .steam-trading-link-help-content *")) {
        $(".steam-trading-link-help-content").hide();
    }

    if (!$(e.target).is(".discount-code-tooltip, .show-code, .discount-code-tooltip *")) {
        $(".discount-code-tooltip").hide();
    }
});

function showNotification(text, type) {
    // type parameter will be used to show different types of notifications (info, error, success, etc.)

    var notification = $("<div class='notifcation-popup hidden' notification-type=" + type + ">"
        + "<p class='notification-popup-text float-right'>"
        + text
        + "</p>"
        + "</div>");

    $("body").append(notification);

    notification.css("left", $(window).width() - 325);
    notification.css("top", $(window).height() - 125);

    notification.show();

    setTimeout(function () {
        notification.fadeOut(1000);
    }, 10000);
}
function getCookie(cookieName) {
    var name = cookieName + "=";
    var ca = document.cookie.split(';');

    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];

        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }

        if (c.indexOf(name) != -1) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function isMobileBrowser() {
    var check = false;
    (function (a, b) { if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od|ad)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4))) check = true })(navigator.userAgent || navigator.vendor || window.opera);
    return check;
}

function loadingIndicator(element) {
    var overlay = '<div class="loading-overlay">' +
        '</div>';

    $(overlay).prependTo(element).css({ height: $(element).css("height"), width: $(element).outerWidth() });
};

$(".toggle-site-menu").click(function () {
    $(".drawer-container").toggle();
});

$(".drawer-container").click(function () {
    $(".drawer-container").hide();
})