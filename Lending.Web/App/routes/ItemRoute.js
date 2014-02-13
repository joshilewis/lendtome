App.OwnershipListRoute = Ember.Route.extend({
    model: function () {
        return this.store.find('item', 1);
    },
});

//App.Store.registerAdapter('App.UserItemList', DS.RESTAdapter.extend({
//    url: "/user/items/"
//}));