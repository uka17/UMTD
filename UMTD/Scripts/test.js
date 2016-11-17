//Buffer for current choosen object
activeTest = ko.observable(new test({ Id: 0, Translation: "[]", Uom: "[]", Method: "[]", Material: "[]" }));

//Model itself
var ViewModel = function () {
    var self = this;
    
    self.summaryTestList = ko.observableArray();    
    self.uomList = ko.observableArray();;
    self.materialList = ko.observableArray();
    self.methodList = ko.observableArray();
    self.languageList = ko.observableArray();
    if ($('#user-email').val() != undefined)
        self.profile = ko.observable(new profile($('#user-email').val()));

    self.currentPage = ko.observable();
    self.pageCount = ko.observableArray();
    
    self.filter = function (data, event) {
        if (event.keyCode == 13) {
            location.hash = $('#filter').val() + '/' + self.currentPage();
            return false;
        }
        else
            return true;
    };
    self.loadReference = function (type) {
        switch (type) {
            case "material":
                var materialLoadDone = function (data) {
                    data.map(function (e) { self.materialList.push({ id: e.Id, name: e.Name }); });
                };
                ajaxLoad("/api/Material/List", { userKey: self.profile().api_key() }, materialLoadDone);
                break;
            case "method":
                var methodLoadDone = function (data) {
                    data.map(function (e) { self.methodList.push({ id: e.Id, name: e.Name }); });
                };
                ajaxLoad("/api/Method/List", { userKey: self.profile().api_key() }, methodLoadDone);
                break;
            case "language":
                var languageLoadDone = function (data) {
                    data.map(function (e) { self.languageList.push({ id: e.Id, name: e.Name, code: e.Code }); });
                };
                ajaxLoad("/api/Language/List", { userKey: self.profile().api_key() }, languageLoadDone);
                break;
            case "uom":
                var uomLoadDone = function (data) {
                    data.map(function (e) { self.uomList.push({ id: e.Id, name: e.FullName }); });
                };
                ajaxLoad("/api/Uom/List", { userKey: self.profile().api_key() }, uomLoadDone);
        }
    };
    self.gotoPage = function (pageNumber) {
        location.hash = $('#filter').val() + '/' + pageNumber;
    }

    self.loadTestList = function (pageNumber) {
        if ($('#user-email').val() != undefined)
            if (App.profile().api_key() != undefined) {
            self.currentPage(pageNumber);
            loadTestDone = function (data) {
                data.map(function (e) {
                    self.summaryTestList.push(new summaryTest(e));
                });
                self.loadPageCount();
            };
            self.summaryTestList.removeAll();
            ajaxLoad("/api/Test/Summary", { userKey: self.profile().api_key(), filter: $('#filter').val(), pageNumber: pageNumber }, loadTestDone);
        }
    };

    self.loadPageCount = function () {
        if ($('#user-email').val() != undefined)
            if (App.profile().api_key() != undefined) {
                loadPageCountDone = function (data) {
                    for (var i = 1; i < data + 1; i++) {
                        self.pageCount.push(i);
                    }
                };
                self.pageCount.removeAll();
                ajaxLoad("/api/Test/SummaryPageCount", { userKey: self.profile().api_key(), filter: $('#filter').val() }, loadPageCountDone);
            }
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

//Main application bindings
var App = new ViewModel();
ko.applyBindings(App);

//Client side navigation with Sammy
Sammy(function () {
    this.get('#:filter', function () {
        $('#filter').val(this.params.filter)
        App.loadTestList(App.currentPage());
    });
    this.get('#/:pageId', function () {
        App.loadTestList(this.params.pageId);
    });
    this.get('#:filter/:pageId', function () {
        $('#filter').val(this.params.filter)
        App.loadTestList(this.params.pageId);
    });
    this.get('/', function () {
        App.loadTestList(1);
    });
}).run();

//Test list
if ($('#user-email').val() != undefined) {
    if (App.profile().api_key() != undefined) {
        App.loadReference("material");
        App.loadReference("method");
        App.loadReference("uom");
        App.loadReference("language");
    }
    else
        $('#content').hide();
}
else
    $('#content').hide();
//Dialogs
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
