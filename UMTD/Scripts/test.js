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
                data.map(function (e) { languageList.push({ id: e.Id, name: e.Name }); });
            };
            ajaxLoad("/api/Language/List", {}, languageLoadDone);
            break;
        case "uom":
            var uomLoadDone = function (data) {
                data.map(function (e) { uomList.push({ id: e.Id, name: e.FullName }); });
            };
            ajaxLoad("/api/Language/List", {}, uomLoadDone);
    }
}

var uomList = ko.observableArray();
var materialList = ko.observableArray();
var methodList = ko.observableArray();
var languageList = ko.observableArray();
var summaryTestList = ko.observableArray();
var currentTest = ko.observable();

//Object for saving data
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
            currentTest(test(data));
        };
        ajaxLoad("/api/Test/Get", { userKey: 'key', testId: obj.id}, loadCurrentTestDone);
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

    //Test translation
    self.addTestTranslation = function (obj) {
        activeTestId = self.id;
        $('#newTranslation').val('');
        $("#dialog-add-translation").dialog("open");
    }

    //Confirm test correct
    self.confirm = function (obj) {
        activeTest = self;
        $("#dialog-confirm-test").dialog("open");
    }
}
//Model itself
var ViewModel = function (uomList, materialList, methodList, languageList, summaryTestList, currentTest) {
    var self = this;
    self.summaryTestList = summaryTestList;
    self.currentTest = currentTest;
    self.uomList = uomList;
    self.materialList = materialList;
    self.methodList = methodList;
    self.languageList = languageList;
    //not used yet
    self.removeTest = function (test) {
        self.testList.remove(test);
        ajaxGet("/api/Test/Delete", { testId: test.id });
    };
    self.filter = function (data, event) {
        if (event.keyCode == 13) {
            loadTestList(self.currentPage);
            return false;
        }
        else
            return true;
    }
    self.createTranslation = function () {
        var j = 0;
        var translation = $('#newTranslation').val();

        var addTranslationDone = function (data) {
            if (findIndexById(activeTestId, self.testList) != null) {
                testList()[j].translation.push(new translation({ id: data, name: translation, languageId: $('#language').val() }));
            }
            else
                alert("Test with Id=" + activeTestId + " was not found")
        };
        ajaxLoad("api/Test/TranslationInsert", { testId: activeTestId, languageId: $('#language').val(), translation: translation }, addTranslationDone);

        $('#dialog-add-translation').dialog("close");
    };
    self.confirmTest = function () {
        ajaxGet("/api/Test/Confirm", { testId: activeTest.id });
        self.testList.remove(activeTest);
        $('#dialog-confirm-test').dialog("close");
    };
};

ko.applyBindings(new ViewModel(uomList, materialList, methodList, languageList, summaryTestList, currentTest));

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
$("#dialog-add-translation").dialog({
    modal: true,
    autoOpen: false,
    width: 400,
    position: { my: "center center", at: "center top" }
});
$("#dialog-confirm-test").dialog({
    modal: true,
    autoOpen: false,
    width: 400,
    position: { my: "center center", at: "center top" }
});
