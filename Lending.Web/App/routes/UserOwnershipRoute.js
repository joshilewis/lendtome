App.UserOwnershipRoute = Ember.Route.extend({
    model: function () {
        return this.store.find('userOwnership');
    },
});

//App.Store.registerAdapter('App.UserItemList', DS.RESTAdapter.extend({
//    url: "/user/items/"
//}));