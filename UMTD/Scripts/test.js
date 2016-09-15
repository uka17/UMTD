var activeTestId = 0;
var activeTest = null;

function loadTestList(pageNumber) {    
    loadTestDone = function (data) {
        loadPageCount($('#filter').val());
        testList.removeAll();
        data.map(function (e) {
            testList.push(new test(e));
        });
    };
    ajaxLoad("/api/Test/List", { filter: $('#filter').val(), pageNumber: pageNumber }, loadTestDone);
}

function loadPageCount(filter) {   
    loadPageCountDone = function (data) {
        if (pageList().length != data) {
            pageList.removeAll();
            for (var i = 1; i < data + 1; i++) {
                pageList.push(i);
            }
        }
    };
    ajaxLoad("/api/Test/PageCount", { filter: filter }, loadPageCountDone);
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

var testList = ko.observableArray();
var uomList = ko.observableArray();
var materialList = ko.observableArray();
var methodList = ko.observableArray();
var languageList = ko.observableArray();
var pageList = ko.observableArray();

//Object for saving data
var test = function (t) {
    var self = this;
    self.id = t.Id;
    self.synonym = ko.observableArray();
    var synonymJSON = jQuery.parseJSON(t.Synonym);
    synonymJSON.map(function (e) {
        self.synonym.push(new synonym(e));
    });
    self.uom = ko.observableArray(jQuery.parseJSON(t.Uom));
    self.method = ko.observableArray(jQuery.parseJSON(t.Method));
    self.material = ko.observableArray(jQuery.parseJSON(t.Material));

    //Synonym
    self.removeTranslation = function (obj) {
        //Remove item
        self.synonym.remove(obj);
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
var ViewModel = function (testList, uomList, materialList, methodList, languageList, pageList) {
    var self = this;
    self.testList = testList;
    self.uomList = uomList;
    self.materialList = materialList;
    self.methodList = methodList;
    self.languageList = languageList;
    self.removeTest = function (test) {
        self.testList.remove(test);
        ajaxGet("/api/Test/Delete", { testId: test.id });
    };
    self.gotoPage = function (pageNumber) {
        loadTestList(pageNumber);
        self.currentPage(pageNumber);
    }
    self.pageList = pageList;
    self.currentPage = ko.observable(1);
    self.filter = function (data, event) {
        if (event.keyCode == 13) {
            loadTestList(self.currentPage);
            return false;
        }
        else
            return true;
    }
    self.createTranslation = function() {
        var j = 0;
        var translation = $('#newTranslation').val();

        var addTranslationDone = function (data) {
            if (findIndexById(activeTestId, self.testList) != null) {
                testList()[j].synonym.push(new synonym({ id: data, name: translation, languageId: $('#language').val() }));
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

ko.applyBindings(new ViewModel(testList, uomList, materialList, methodList, languageList, pageList));

//Test list
loadTestList(1);

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


loadReference("material");
loadReference("method");
loadReference("uom");
loadReference("language");


