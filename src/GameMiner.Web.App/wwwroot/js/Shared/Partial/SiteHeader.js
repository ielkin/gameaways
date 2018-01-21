if ($(".notifications-list-container").length > 0) {
    $("#UserMenu").css("left", $(".username").position().left - 35);
    $("#UserMenu").css("top", $(".username").position().top + 60);

    $(".username").click(function () {

        if ($("#UserMenu").css("display") == "none") {

            $("#UserMenu").slideDown(10);
            $(this).addClass("username-active");
        }
        else {
            $("#UserMenu").slideUp(10);
            $(this).removeClass("username-active");
        }
    });

    $(window).resize(function () {

        $("#UserMenu").css("left", $(".username").position().left);
        $("#UserMenu").css("top", $(".username").position().top + 50);

        if ($(document).width() <= 440) {
            $(".username").html("&nbsp;");

            $("#UserMenu li.top-border-left").css("width", $("#site-header .username").position().left);
            $("#UserMenu li.top-border-right").css("width", $(document).width() - $("#site-header .username").position().left - 54);
        }
        else {
            $("#UserMenu li.top-border-left").css("width", $("#site-header .username").position().left);
            $("#UserMenu li.top-border-right").css("width", $(document).width() - $("#site-header .username").position().left - 224);

            $(".username").html($(".username").attr("data-val-username"));
        }
    });
}