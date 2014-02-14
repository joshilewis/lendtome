App.ItemController = Ember.ObjectController.extend({
    actions: {
        addItem: function() {
            var item = this.store.createRecord("item", {
                "id": "00000000000000000000000000000000", "title": "newtitle",
                "creator": "newcreator", "edition": "newedition"
            });
            item.save();
        },
    }
});