$('#postComment').click(function () {
    if ($("#Text").val() != '') {
        $.ajax({
            type: "POST",
            url: '/Comments/AddComment',
            data: $("#AddCommentForm").serialize(),
            dataType: "html",
            success: function (data) {
                var result = $.parseJSON(data);

                var commentDiv = "<div class='comment'>"
                    + "<p style='background-image: url(\"" + result.ProfilePictureUrl + "\");'>"
                    + "<a href='/User/" + result.Username + "'>" + result.Username + "</a>"
                    + " - " + result.CommentDate
                    + "</br>"
                    + "<span>" + result.Text + "</span>"
                    + "</p>"
                    + "</div>";

                if ($("#comments p.comment").length == 0) {
                    $("#comments #AddCommentForm").before(commentDiv);
                    $(".no-new-comments-note").remove();
                }
                else {
                    $("#comments p").first().after(commentDiv);
                }

                $("#Text").val('');
            }
        });
    }
});