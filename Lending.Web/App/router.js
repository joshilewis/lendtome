App.Router.map(function () {
    this.route("index", { path: "/" });
    this.route("about");
    this.route("userOwnershipList", { path: "/myitems" });
    this.route("item", { path: "/item" });
});