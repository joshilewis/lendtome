var attr = DS.attr;
App.UserOwnership = DS.Model.extend({
    id: attr('string'),
    item: DS.attr('item'),

    hasError: function () {
        var currentError = this.get("error");
        return !(currentError === '' || currentError == null);
    }.property('error'),

});

