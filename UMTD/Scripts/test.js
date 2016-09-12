var activeTestId = 0;

function loadTestList(filter, pageNumber) {
    testList.removeAll();
    $.ajax({
        statusCode: {
            404: function () {
                alert("page not found");
            }
        },
        contentType: "application/json",
        dataType: "json",
        data: { filter: '', pageNumber: pageNumber },
        url: "/api/Test/List",
    })
      .done(function (data) {
          data.map(function (e) {
              testList.push(new test(e));
          });
      });
}

function ajaxLoad(type) {
    var url = null;
    switch (type) {
        case "material":
            url = "/api/Material/List";
            arr = materialList;
            break;
        case "method":
            url = "/api/Method/List";
            arr = methodList;
            break;
        case "language":
            url = "/api/Language/List";
            arr = languageList;
            break;
        case "uom":
            url = "/api/Uom/List";
            arr = uomList;
    }
    loadingEvent(type, 1);
    if (url != null) {
        this.request = $.ajax({
            statusCode: {
                404: function () {
                    alert("Page not found");
                }
            },
            async: false,
            contentType: "application/json",
            dataType: "json",
            url: url,
        })
          .done(function (data) {
              data.map(function (e) {
                  switch (type) {
                      case "uom":
                          arr.push({
                              id: e.Id,
                              name: e.FullName
                          });
                          break;
                      default:
                          arr.push({
                              id: e.Id,
                              name: e.Name
                          });
                  }
              });
              loadingEvent(type, 0);
          });
    }
}

var testList = ko.observableArray();
var uomList = ko.observableArray();
var materialList = ko.observableArray();
var methodList = ko.observableArray();
var languageList = ko.observableArray();

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
    self.removeSynonym = function (obj) {
        //Remove item
        self.synonym.remove(obj);
        $.get("api/Test/TranslationDelete", { translationId: obj.id });
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
            $.get("/api/Uom/Insert", { testId: self.id, uomId: self.selectedUom()[0] });
        }
    });
    //Material
    self.removeMaterial = function (obj) {
        //Remove item
        self.material.remove(obj);
        $.get("/api/Material/Delete", { testId: self.id, materialId: obj.id });
    }
    self.selectedMaterial = ko.observableArray();

    self.selectedMaterial.subscribe(function (value) {
        var name = findNameById(value, materialList);
        if (name != null) {
            self.material.push({ id: self.selectedMaterial()[0], name: name });
            $.get("api/Material/Insert", { testId: self.id, materialId: self.selectedMaterial()[0] });
        }
    });
    //Method
    self.removeMethod = function (obj) {
        //Remove item
        self.method.remove(obj);
        $.get("/api/Method/Delete", { testId: self.id, methodId: obj.id });
    }
    self.selectedMethod = ko.observableArray();

    self.selectedMethod.subscribe(function (value) {
        var name = findNameById(value, methodList);
        if (name != null) {
            self.method.push({ id: self.selectedMethod()[0], name: name });
            $.get("/api/Method/Insert", { testId: self.id, methodId: self.selectedMethod()[0] });
        }
    });

    //Test translation
    self.addTestTranslation = function (obj) {
        activeTestId = self.id;
        $('#newTranslation').val('');
        $("#dialog-add-translation").dialog("open");
    }
}
//Model itself
var ViewModel = function (testList, uomList, materialList, methodList, languageList) {
    var self = this;
    self.testList = testList;
    self.uomList = uomList;
    self.materialList = materialList;
    self.methodList = methodList;
    self.languageList = languageList;
    self.removeTest = function (test) {
        self.testList.remove(test);
        $.get("/api/Test/Delete", { testId: test.id });
    };
    self.gotoPage = function (pageNumber) {
        loadTestList('', pageNumber);
        self.currentPage(pageNumber);
    }
    self.pageCount = [1, 2, 3, 4, 5];
    self.currentPage = ko.observable(1);
};

ko.applyBindings(new ViewModel(testList, uomList, materialList, methodList, languageList));

//Test list
loadTestList('', 1);

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

$('#createTestTranslation').click(function () {
    var j = 0;
    var translation = $('#newTranslation').val();

    $.get("api/Test/TranslationInsert", { testId: activeTestId, languageId: $('#language').val(), translation: translation })
    .done(function (data) {
        for (var i = 0; i < arr().length - 1; i++) {
            if (arr()[i].id == activeTestId)
                j = i;
        }
        if (j == 0) {
            testList()[j].synonym.push(new synonym({ id: data, name: translation, languageId: $('#language').val() }));
        }
        else
            alert("Test with Id=" + activeTestId + " was not found")
    });

    $('#dialog-add-translation').dialog("close");
});
//uom and methods will be triggered recursively
ajaxLoad("material");
ajaxLoad("method");
ajaxLoad("uom");
ajaxLoad("language");


