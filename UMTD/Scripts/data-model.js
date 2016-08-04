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
var synonym = function (t) {
    var self = this;
    self.id = t.id;
    self.languageId = t.languageId;
    self.name = t.name;
    self.languageIcon = ko.pureComputed(function () {
        return self.languageId == 1 ? "Images/ru.png" : "Images/en.png";
    });
}