$(".send-keys-button").click(function (event) {

    $(".winner-id").val($(this).data("winnerid"));

    $(".send-keys-popup").css("left", $(this).position().left - 195);
    $(".send-keys-popup").css("top", $(this).position().top - 95);
    $(".send-keys-popup").show();
});

$(".send-redemption-code").click(function () {
    var winnerId = $(".winner-id").val();
    var url = "/Giveaways/SendGift"
        + "?winnerId=" + winnerId
        + "&redemptionCode=" + encodeURI($(".redemption-code").val());

    $.getJSON(url, null, function (response) {
        $(".send-keys-popup").hide();
        $(".redemption-code-cell" + winnerId).text($(".redemption-code").val())
        $(".gift-status" + winnerId).text(response.Description)
        $(".send-keys-button[data-winnerid='" + winnerId + "']").hide();

        $(".send-keys-button").remove();
        $(".mark-prize-sent").remove();
    });
});

$(".mark-prize-sent").click(function () {

    var element = $(this);
    var winnerId = element.attr("winner-id");
    var url = "/Giveaways/SendGift?winnerId=" + winnerId;

    $.getJSON(url, null, function (response) {
        $(".gift-status" + winnerId).text(response.Description);
        element.hide();
    });
});

$("p.buttons").on("click", ".enter-ga.enabled", function () {
    var button = $(this);

    //if (button.hasClass("enabled") == true) {
    //    button.toggleClass("enabled");

        $.get("/Giveaways/Enter/" + $("#Id").val(), null, function (data) {
            //button.toggleClass("enabled");

            //$(".my-entries-count").text(data.userEntriesCount);
            //$(".total-entries-count").text(data.entriesCount);
            //$(".giveaway-funding-progress-bar-active").width(data.fundingProgress + "%");
            //$(".giveaway-funding-header-value").text(data.fundingProgress);
            //$(".credits-container").text(data.creditBalance + " Credits");
            //$(".leave-ga").removeClass("hidden");

            //if (data.entriesEnabled == true) {
            //    button.text("+1 entry");
            //}
            //else {
            //    $(".enter-ga").addClass("hidden");
            //    $(".get-credits-button").removeClass("hidden");
            //}

            window.location.reload();
        });
    //}
});

$("p.buttons").on("click", ".leave-ga", function () {
    var button = $(this);
    $.get("/Giveaways/Leave/" + $("#Id").val(), null, function (data) {
        //$(".my-entries-count").text("No entries");
        //$(".total-entries-count").text(data.entriesCount);
        //$(".giveaway-funding-progress-bar-active").width(data.fundingProgress + "%");
        //$(".giveaway-funding-header-value").text(data.fundingProgress);
        //$(".credits-container").text(data.creditBalance + " Credits");
        //$(".leave-ga").addClass("hidden");

        //if (data.entriesEnabled == true) {
        //    $(".enter-ga").text("Enter");
        //    $(".enter-ga").removeClass("hidden");
        //    $(".get-credits-button").addClass("hidden");
        //}
        //else {
        //    $(".enter-ga").addClass("hidden");
        //    $(".get-credits-button").removeClass("hidden");
        //}

        window.location.reload();
    });
});

$(".accept-gift").on("click", function () {
    var acceptButton = $(this);

    console.log(acceptButton.attr("data-winnerid"));

    $.get("/Giveaways/AcceptGift?winnderId=" + acceptButton.attr("data-winnerid"), null, function (data) {
        acceptButton.text("Gift Accepted");
        acceptButton.removeClass("accept-gift");
        acceptButton.replaceWith("<span class='gift-worked'>Gift or code worked</span>");

        $(".report-invalid-gift").remove();
    });
});

$(".report-invalid-gift").on("click", function () {
    var reportButton = $(this);
    console.log($("#ID").val());
    $.get("/Giveaways/ReportInvalidGift?id=" + $("#ID").val(), null, function (data) {
        $(".ga-gift-code").addClass("red");
        $(".accept-gift").remove();
        reportButton.replaceWith("<span class='red'>Code is reported as invalid and currenly under ivestigation</span>");
    });
});

$("a.pick-new-winner").click(function () {
    var winnerRow = $(this).parent().parent();
    var winnerId = $(this).attr("data-winnerid");

    var button = $(this);

    if (button.hasClass("enabled") == true) {

        button.toggleClass("enabled");
        button.toggleClass("processing");

        $.get("/Giveaways/PickNewWinner?winnerId=" + winnerId, null, function (data) {
            winnerRow.find(".ga-username").text(data.Username);
            winnerRow.find(".ga-username").attr("href", "/User/" + data.Username);

            winnerRow.find("td").eq(2).html(data.GiftStatus);

            window.location.reload();
        });
    }
});

$(document).ready(function () {
    $(".entries-list").scroll(function (e) {
        if ($(".entries-list").scrollTop() === $(".entries-list").prop('scrollHeight') - $(".entries-list").height() && $(".giveaway-entry.loading").length === 0 && parseFloat($(".entries-list").attr("next-page")) < parseFloat($(".entries-list").attr("page-count"))) {

            $(".entries-list").scrollTop($(".giveaway-entry").last().offset().top - $(".entries-list").offset().top + $(".entries-list").scrollTop());
        }
    });
});

function enterWithToken(token) {
    $.get("/Giveaways/Enter?id=" + $("#Id").val() + "&token=" + token, null, function (data) {
        //button.toggleClass("enabled");

        //$(".my-entries-count").text(data.userEntriesCount);
        //$(".total-entries-count").text(data.entriesCount);
        //$(".giveaway-funding-progress-bar-active").width(data.fundingProgress + "%");
        //$(".giveaway-funding-header-value").text(data.fundingProgress);
        //$(".credits-container").text(data.creditBalance + " Credits");
        //$(".leave-ga").removeClass("hidden");

        //if (data.entriesEnabled == true) {
        //    button.text("+1 entry");
        //}
        //else {
        //    $(".enter-ga").addClass("hidden");
        //    $(".get-credits-button").removeClass("hidden");
        //}

        window.location.reload();
    });
}