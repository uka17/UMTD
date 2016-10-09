$(function () {
    $("#dialog-login").dialog({
        modal: true,
        autoOpen: false,
        width: 420,
        position: { my: "center center", at: "center center" }
    });

    $("#profile").dialog({
        modal: true,
        autoOpen: false,
        width: 420,
        position: { my: "center top", at: "center top" }
    });

    $('#show-login-dialog').click(function () {
        $("#dialog-login").dialog("open");
        //$(".ui-dialog-titlebar").hide();
        return false;
    });
    $('#reference-link').click(function (event) {
        $('#reference-menu').toggle();
        event.stopPropagation();
    });
    $('#profile-link').click(function (event) {
        $('#profile').dialog('open');
    });

    $(window).click(function () {
        $('#reference-menu').hide();
    });

    $('#do-login').click(function () {
        $.ajax({
            url: "/api/User/Login",
            data: { email: $('#email').val(), password: $('#password').val(), remember: $('#remember').prop('checked') },
            statusCode: {
                404: function () {
                    $('.form-error').html("Page not found");
                },
                403: function (data) {
                    $('.form-error').html('⚠ ' + data.responseJSON);
                    $('.form-error').fadeOut();
                    $('.form-error').fadeIn();
                }
            },
            contentType: "application/json",
            dataType: "json"
        })
        .done(function (data) {
            location.reload();
        })
        .error(function (jqXHR, status, errorThrown) {
            if (jqXHR.status != 403) {
                alert(errorThrown + ": " + jqXHR.responseText);
            }
        });
        return false;
    });
});




