function loadTestList() {
    loadTestDone = function (data) {
        summaryTestList.removeAll();
        data.map(function (e) {
            summaryTestList.push(new summaryTest(e));
        });
    };
    ajaxLoad("/api/Test/Summary", { userKey: 'key', filter: $('#filter').val()}, loadTestDone);
}
function loadReference(type) {
    switch (type) {
        case "material":
            var materialLoadDone = function (data) {
                data.map(function (e) { materialList.push({ id: e.Id, name: e.Name }); });
            };
            ajaxLoad("/api/Material/List", {}, materialLoadDone);
            break;
        case "method":
            var methodLoadDone = function (data) {
                data.map(function (e) { methodList.push({ id: e.Id, name: e.Name }); });
            };
            ajaxLoad("/api/Method/List", {}, methodLoadDone);
            break;
        case "language":
            var languageLoadDone = function (data) {
                data.map(function (e) { languageList.push({ id: e.Id, name: e.Name, code: e.Code }); });
            };
            ajaxLoad("/api/Language/List", {}, languageLoadDone);
            break;
        case "uom":
            var uomLoadDone = function (data) {
                data.map(function (e) { uomList.push({ id: e.Id, name: e.FullName }); });
            };
            ajaxLoad("/api/Uom/List", {}, uomLoadDone);
    }
}

var uomList = ko.observableArray();
var materialList = ko.observableArray();
var methodList = ko.observableArray();
var languageList = ko.observableArray();
var summaryTestList = ko.observableArray();
var activeTest = ko.observable(new test({ Id: 0, Translation: "[]", Uom: "[]", Method: "[]", Material: "[]" }));

//Model itself
var ViewModel = function (uomList, materialList, methodList, languageList, summaryTestList, activeTest) {
    var self = this;
    self.summaryTestList = summaryTestList;
    self.activeTest = activeTest;
    self.uomList = uomList;
    self.materialList = materialList;
    self.methodList = methodList;
    self.languageList = languageList;

    self.filter = function (data, event) {
        if (event.keyCode == 13) {
            loadTestList();
            return false;
        }
        else
            return true;
    }

    //Add translation
    self.addTranslation = function () {
        $("#dialog-add-translation").dialog("open");
    }
    self.createTranslation = function () {
        var createTranslationDone = function (data) {
            activeTest().translation.push(new testTranslation({
                id: data,
                languageId: $('#language').val(),
                name: $('#newTranslation').val(),
                languageCode: self.languageList().find(function (obj) {
                    return obj.id == $('#language').val();
                }).code
            }));
            $("#dialog-add-translation").dialog("close");
        };
        ajaxLoad("/api/Test/TranslationInsert", { testId: activeTest().id, languageId: $('#language').val(), translation: $('#newTranslation').val() }, createTranslationDone);

    };
    self.confirmTest = function () {
        ajaxGet("/api/Test/Confirm", { testId: activeTest().id });
        self.summaryTestList.remove(function (item) { return item.id == activeTest().id; });
        $('#dialog-confirm-test').dialog("close");
        $('#dialog-edit-test').dialog("close");
    };
    self.hideTest = function () {
        $('#dialog-edit-test').dialog("close");
    };
};

ko.applyBindings(new ViewModel(uomList, materialList, methodList, languageList, summaryTestList, activeTest));

//Test list
loadTestList();

loadReference("material");
loadReference("method");
loadReference("uom");
loadReference("language");

$("#dialog-message").dialog({
    modal: true,
    autoOpen: false,
    width: 280
});
$("#dialog-edit-test").dialog({
    modal: true,
    autoOpen: false,
    width: 700
});

$("#dialog-add-translation").dialog({
    modal: true,
    autoOpen: false,
    width: 400,
    position: { my: "center center", at: "center top" }
});
$("#dialog-confirm-test").dialog({
    modal: true,
    autoOpen: false,
    width: 420,
    position: { my: "center center", at: "center center" }
});
