using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Joshilewis.Infrastructure.DI;
using Lending.Web.App_Start;
using Lending.Web.DependencyResolution;
using log4net.Config;
using Lending.Execution.DI;
using StructureMap;

namespace Lending.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            XmlConfigurator.Configure();
            //LogManager.LogFactory = new ServiceStack.Logging.Log4Net.Log4NetFactory(true);

            IoC.Initialize<LendingContainer>(new WebRegistry());
            new StartupMigrator().InitialiseAndMigrate();

            //AppHost.Start(new StructureMapContainerAdapter(Container));
            
            //AreaRegistration.RegisterAllAreas();

            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //ViewEngines.Engines.Clear();
            //ViewEngines.Engines.Add(new CustomViewEngine());
        }
    }
}