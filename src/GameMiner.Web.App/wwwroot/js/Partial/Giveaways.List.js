$(".ga-list-menu-menu").click(function () {

    if ($(this).hasClass("active") == true) {
        $(this).parent().find("span").not($(this)).hide();
        $(".ga-list-menu-menu.active").removeClass("active");
    }
    else {
        $(".ga-list-menu-menu.active").parent().find("span").not($(".ga-list-menu-menu.active")).hide();
        $(".ga-list-menu-menu.active").removeClass("active");
        $(this).parent().find("span").show();
        $(this).addClass("active");
    }
});

$(".ga-list-menu-remind").on('click', function (event) {

    $("#set-reminder").attr("giveaway-id", $(this).attr("data-item-id"));
    $(".reminder-container").css("left", $(this).offset().left - $(".reminder-container").width() / 2  - 14);
    $(".reminder-container").css("top", $(this).offset().top - $(".reminder-container").height() - 35);
    $(".reminder-container").toggle();
});

$(document).on("click", ".hide-game", function () {
    var element = $(this);

    var url = "/Giveaways/HideGiveaway"
        + "?itemId=" + element.attr("data-item-id")
        + "&itemType=" + element.attr("data-item-type");

    $.getJSON(url, null, function (data) {
        element.removeClass("hide-game");
        element.addClass("unhide-game");

        var giveawayDiv = element.parentsUntil(".giveaway").parent();

        var gameTitle = giveawayDiv.find(".ga-info .game-title").text();

        var hiddenGiveawayDivs = $(".giveaway .game-title").filter(function (index) { return $(this).text() == gameTitle; }).parentsUntil(".giveaway").parent();

        hiddenGiveawayDivs.before("<div class='hidden-ga-placeholder'>"
            + "Giveaways with " + gameTitle + " were hidden <a class='unhide-ga-link'>Unhide</a>"
            + "</div>");
    });
});

$(document).on("click", ".unhide-ga-link", function () {
    $(this).parentsUntil(".giveaway").parent().find(".unhide-game").trigger("click");
    $(".hidden-ga-placeholder").remove();
});

$(document).on("click", ".unhide-game", function () {
    var element = $(".unhide-game");

    var url = "/Giveaways/UnhideGiveaway"
        + "?itemId=" + $(this).attr("data-item-id")
        + "&itemType=" + $(this).attr("data-item-type");

    $.getJSON(url, null, function (data) {
        element.addClass("hide-game");
        element.removeClass("unhide-game");
    });
});

$(document).on("click", ".leave-ga", function (event) {
    var giveawayId = $(event.target).attr("data-val");
    $.get("/Giveaways/Leave/" + giveawayId, null, function (data) {
        $("#" + $(event.target).attr("data-val")).removeClass("has-entry");
        $(event.target).remove();
        $("#" + giveawayId).find(".entries-count").text(data.EntriesCount);
        $(".ga-points>span").text(data.Balance);
    });
})