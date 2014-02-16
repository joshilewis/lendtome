window.App = Em.Application.create();
App.ApplicationAdapter = DS.RESTAdapter.extend({
    namespace: 'api',
    antiForgeryTokenSelector: "#antiForgeryToken",
});

