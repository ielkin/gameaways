$('#postPassword').click(function () {
    $.ajax({
        type: "POST",
        url: '/Account/ChangePassword',
        data: $("#ChangePasswordForm").serialize(),
        dataType: "html",
        success: function (data) {
            var result = $.parseJSON(data);
            var message = $(".message");
            message.removeClass("message-error message-success");
            message.addClass("message-" + result.Status);
            message.fadeTo(0, 1);
            message.text(result.Message);
            message.fadeTo(3000, 0);
        }
    });
});