using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Lending.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.IgnoreRoute("api/{*pathInfo}");
            routes.IgnoreRoute("_nancy/{*pathInfo}");
            routes.IgnoreRoute("authentication/{*pathInfo}");

            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" }); //Prevent exceptions for favicon

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.IgnoreRoute("{*url}");

            routes.MapRoute(
                name: "Default",
                url: "{*path}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}