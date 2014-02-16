var attr = DS.attr;
App.UserOwnership = DS.Model.extend({
    //id: attr('string'),
    item: DS.hasMany('Item'),
});

