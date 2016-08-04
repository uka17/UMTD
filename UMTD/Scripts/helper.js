function findNameById(id, arr) {
    for (var i = 0; i < arr().length - 1; i++) {
        if (arr()[i].id == id)
            return arr()[i].name;
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