using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Hangfire;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Hangfire.MemoryStorage;

[assembly: OwinStartup(typeof(Lending.Web.Startup))]

namespace Lending.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseMemoryStorage();

            app.UseHangfireServer();
            app.UseHangfireDashboard();
        }
    }
}
