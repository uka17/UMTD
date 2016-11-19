var alertStandart = function (jqXHR, status, errorThrown) {
    alert(errorThrown + ": " + jqXHR.responseText);
}

function ajaxLoad(url, data, doneFunction, errorFunction) {
    $.ajax({
        statusCode: {
            404: function () {
                alert("Resource was not found");
            },
            200: doneFunction
        },
        contentType: "application/json",
        dataType: "json",
        data: data,
        url: url,
    })
  .error(errorFunction == null ? alertStandart : errorFunction);
}
function ajaxLoadSync(url, data) {
    return $.ajax({
        type: "GET",
        url: url,
        data: data,
        async: false
    }).responseText;
}
function ajaxGet(url, data) {
    $.ajax({
        statusCode: {
            404: function () {
                alert("Page not found");
            }
        },
        data: data,
        url: url,
    })
    .error(function (jqXHR, status, errorThrown) {
        alert(errorThrown + ": " + jqXHR.responseText);
    });
}
function findNameById(id, arr) {
    for (var i = 0; i < arr().length - 1; i++) {
        if (arr()[i].id == id)
            return arr()[i].name;
    }
    return null;
}
function findIndexById(id, arr) {
    for (var i = 0; i < arr().length - 1; i++) {
        if (arr()[i].id == id)
            return i;
    }
    return null;
}
function findObjectById(id, arr) {
    for (var i = 0; i < arr().length - 1; i++) {
        if (arr()[i].id == id)
            return arr()[i];
    }
    return null;
}
function loadingEvent(entity, state) {
    $("#dialog-message").attr("title", entity);
    if (state)
        $("#dialog-message").dialog("open");
    else
        $("#dialog-message").dialog("close");
}
