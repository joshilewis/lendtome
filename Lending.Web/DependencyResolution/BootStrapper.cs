using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Lending.Execution.Nancy;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.StructureMap;
using Nancy.Diagnostics;
using StructureMap;
using Nancy.Authentication.Forms;
using Nancy.Authentication.Token;
using Nancy.Owin;

namespace Lending.Web.DependencyResolution
{
    public class BootStrapper : StructureMapNancyBootstrapper
    {
        protected override IContainer GetApplicationContainer()
        {
            return MvcApplication.Container;
        }

        protected override DiagnosticsConfiguration DiagnosticsConfiguration => new DiagnosticsConfiguration { Password = @"secret" };

        protected override void ApplicationStartup(IContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
        }

        protected override void RequestStartup(IContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);

            var owinEnvironment = context.GetOwinEnvironment();

            var principal = owinEnvironment?["server.User"] as ClaimsPrincipal;

            if (principal == null) return;

            var userName = principal.Identity.Name;
            var claims = principal.Claims.Where(
                o => o.Type == ClaimTypes.Role)
                .Select(o => o.Value);
            context.CurrentUser = new AuthenticatedUser(userName,claims);
        }
    }
}
