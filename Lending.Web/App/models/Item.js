var attr = DS.attr;
App.Item = DS.Model.extend({
    id: attr('string'),
    title: attr('string'),
    creator: attr('string'),
    edition: attr('string'),

    hasError: function () {
        var currentError = this.get("error");
        return !(currentError === '' || currentError == null);
    }.property('error'),

});
