using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Lending.Web.App_Start
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/content/css/app").Include("~/content/bootstrap.css"));

            bundles.Add(new ScriptBundle("~/js/jquery").Include("~/scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/js/app").Include(
                "~/scripts/angular.js",
                "~/scripts/angular-resource.js",
                "~/scripts/angular-route.js",
                "~/scripts/angular-ui-router.js",
                "~/scripts/AngularUI/ui-router.js",
                "~/scripts/ui-bootstrap-0.10.0.js",
                "~/scripts/ui-bootstrap-tpls-0.10.0.js",
                //"~/scripts/filters.js",
                "~/App/services.js",
                //"~/scripts/directives.js",
                "~/App/controllers.js",
                "~/App/app.js"
                ));
        }
    }
}