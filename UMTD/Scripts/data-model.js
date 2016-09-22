//Data models
var uom = function (t) {
    this.id = t.Id;
    this.name = t.FullName;
}
var method = function (t) {
    this.id = t.Id;
    this.name = t.Name;
}
var material = function (t) {
    this.id = t.Id;
    this.name = t.Name;
}
var testTranslation = function (t) {
    var self = this;
    self.languageListVisible = ko.observable(false);
    self.editState = ko.observable(false);
    self.id = t.id;
    self.languageId = ko.observable(t.languageId);
    self.languageCode = ko.observable(t.languageCode);
    self.name = ko.observable(t.name);
    self.oldName = self.name;
    self.languageIcon = ko.pureComputed(function () {
        return "/Images/" + self.languageCode() + ".png";
    });
    //Edit
    self.edit = function (obj) {
        for (var i = 0; i < activeTest().translation().length; i++) {
            activeTest().translation()[i].editState(false);
        }
        self.editState(!self.editState());
        self.oldName = self.name();
    }
    self.cancelEdit = function () {
        self.editState(false);
        self.name(self.oldName);
    }
    //Language
    self.toggleLanguageList = function () {
        for (var i = 0; i < activeTest().translation().length; i++) {
            activeTest().translation()[i].languageListVisible(false);
        }
        self.languageListVisible(!self.languageListVisible());
    };
    self.setLanguage = function (data) {
        self.languageId(data.id);
        self.languageCode(data.code);
        self.updateTranslation();
        self.languageListVisible(false);
    };
    self.update = function () {
        $.get("/api/Test/TranslationUpdate", { userKey: $('#user-key').val(), translationId: self.id, languageId: self.languageId, translation: self.name });
        self.editState(false);
    }
}

var summaryTest = function (t) {
    var self = this;
    self.name = t.Name;
    self.id = t.Id;
    self.translationCount = t.TranslationCount;
    self.materialCount = t.MaterialCount;
    self.methodCount = t.MethodCount;
    self.uomCount = t.UomCount;
    self.showTest = function (obj) {
        loadCurrentTestDone = function (data) {
            activeTest(new test(data));
            $("#dialog-edit-test").dialog("open");
        };
        ajaxLoad("/api/Test/Get", { userKey: $('#user-key').val(), testId: obj.id }, loadCurrentTestDone);
    }
}
var test = function (t) {
    var self = this;
    self.id = t.Id;
    self.translation = ko.observableArray();
    var translationJSON = jQuery.parseJSON(t.Translation);
    translationJSON.map(function (e) {
        self.translation.push(new testTranslation(e));
    });
    self.uom = ko.observableArray(jQuery.parseJSON(t.Uom));
    self.method = ko.observableArray(jQuery.parseJSON(t.Method));
    self.material = ko.observableArray(jQuery.parseJSON(t.Material));

    //translation
    self.addTranslation = function () {
        $("#dialog-add-translation").dialog("open");
    }
    self.createTranslation = function () {
        var createTranslationDone = function (data) {
            activeTest().translation.push(new testTranslation({
                id: data,
                languageId: $('#language').val(),
                name: $('#newTranslation').val(),
                languageCode: languageList().find(function (obj) {
                    return obj.id == $('#language').val();
                }).code
            }));
            $("#dialog-add-translation").dialog("close");
        };
        ajaxLoad("/api/Test/TranslationInsert", { userKey: $('#user-key').val(), testId: activeTest().id, languageId: $('#language').val(), translation: $('#newTranslation').val() }, createTranslationDone);

    };
    self.removeTranslation = function (obj) {
        //Remove item
        self.translation.remove(obj);
        ajaxGet("api/Test/TranslationDelete", { userKey: $('#user-key').val(), translationId: obj.id });
    }
    //Uom
    self.removeUom = function (obj) {
        //Remove item
        self.uom.remove(obj);
        $.get("/api/Test/UomDelete", { testId: self.id, uomId: obj.id });
    }
    self.selectedUom = ko.observableArray();

    self.selectedUom.subscribe(function (value) {
        var name = findNameById(value, uomList);
        //TODO: Do not allow to add duplicates
        if (name != null) {
            self.uom.push({ id: self.selectedUom()[0], name: name });
            ajaxGet("/api/Test/UomInsert", { userKey: $('#user-key').val(), testId: self.id, uomId: self.selectedUom()[0] });
        }
    });
    //Material
    self.removeMaterial = function (obj) {
        //Remove item
        self.material.remove(obj);
        ajaxGet("/api/Test/MaterialDelete", { userKey: $('#user-key').val(), testId: self.id, materialId: obj.id });
    }
    self.selectedMaterial = ko.observableArray();

    self.selectedMaterial.subscribe(function (value) {
        var name = findNameById(value, materialList);
        //TODO: Do not allow to add duplicates
        if (name != null) {
            self.material.push({ id: self.selectedMaterial()[0], name: name });
            ajaxGet("api/Test/MaterialInsert", { userKey: $('#user-key').val(), testId: self.id, materialId: self.selectedMaterial()[0] });
        }
    });
    //Method
    self.removeMethod = function (obj) {
        //Remove item
        self.method.remove(obj);
        ajaxGet("/api/Test/MethodDelete", { userKey: $('#user-key').val(), testId: self.id, methodId: obj.id });
    }
    self.selectedMethod = ko.observableArray();

    self.selectedMethod.subscribe(function (value) {
        var name = findNameById(value, methodList);
        //TODO: Do not allow to add duplicates
        if (name != null) {
            self.method.push({ id: self.selectedMethod()[0], name: name });
            ajaxGet("/api/Test/MethodInsert", { userKey: $('#user-key').val(), testId: self.id, methodId: self.selectedMethod()[0] });
        }
    });

    self.confirm = function () {
        $("#dialog-confirm-test").dialog("open");
    }
}