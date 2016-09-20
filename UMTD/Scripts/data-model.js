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
   
    self.updateTranslation = function () {
        $.get("/api/Test/TranslationUpdate", { userKey: 'key', translationId: self.id, languageId: self.languageId, translation: self.name });
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
        ajaxLoad("/api/Test/Get", { userKey: 'key', testId: obj.id }, loadCurrentTestDone);
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
    self.removeTranslation = function (obj) {
        //Remove item
        self.translation.remove(obj);
        ajaxGet("api/Test/TranslationDelete", { translationId: obj.id });
    }
    //Uom
    self.removeUom = function (obj) {
        //Remove item
        self.uom.remove(obj);
        $.get("/api/Uom/Delete", { testId: self.id, uomId: obj.id });
    }
    self.selectedUom = ko.observableArray();

    self.selectedUom.subscribe(function (value) {
        var name = findNameById(value, uomList);
        if (name != null) {
            self.uom.push({ id: self.selectedUom()[0], name: name });
            ajaxGet("/api/Uom/Insert", { testId: self.id, uomId: self.selectedUom()[0] });
        }
    });
    //Material
    self.removeMaterial = function (obj) {
        //Remove item
        self.material.remove(obj);
        ajaxGet("/api/Material/Delete", { testId: self.id, materialId: obj.id });
    }
    self.selectedMaterial = ko.observableArray();

    self.selectedMaterial.subscribe(function (value) {
        var name = findNameById(value, materialList);
        if (name != null) {
            self.material.push({ id: self.selectedMaterial()[0], name: name });
            ajaxGet("api/Material/Insert", { testId: self.id, materialId: self.selectedMaterial()[0] });
        }
    });
    //Method
    self.removeMethod = function (obj) {
        //Remove item
        self.method.remove(obj);
        ajaxGet("/api/Method/Delete", { testId: self.id, methodId: obj.id });
    }
    self.selectedMethod = ko.observableArray();

    self.selectedMethod.subscribe(function (value) {
        var name = findNameById(value, methodList);
        if (name != null) {
            self.method.push({ id: self.selectedMethod()[0], name: name });
            ajaxGet("/api/Method/Insert", { testId: self.id, methodId: self.selectedMethod()[0] });
        }
    });

    self.confirm = function () {
        $("#dialog-confirm-test").dialog("open");
    }
}