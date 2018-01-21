function searchGames() {
    $.getJSON("/Giveaways/SearchGames?term=" + $("input#game-search").val(), function (games) {
        $("#games-list").html("");
        for (var i = 0; i < games.length; i++) {
            $("#games-list").append("<a game-id='" + games[i].id + "'>" + games[i].label + "</a>");
        }
        $("#games-list").removeClass('hidden');
    });
}

$(".list-of-cities").on("click", "a", function (e) {
    $("#venue-city-id").val($(e.target).attr("city-id"));
    $("input#venue-city").val($(e.target).text());
    $(".dropdown-content").hide();
});

$("#game-search").keyup(function () {
    if ($("#game-search").val().length > 2) {
        searchGames();
    } else {
        $("#games-list").addClass('hidden');
    }
});

$(".create-giveaway.enabled").click(function () {
    var button = $(this);

    if (button.hasClass("enabled") == true) {
        button.removeClass("enabled");
        $("#GiveawayForm").submit();
    }
});

$("#games-list").on("click", "a", function (e) {
    $("#game-search").val($(e.target).text());
    $("#GameId").val($(e.target).attr("game-id"));
    $("#games-list").addClass("hidden");
});