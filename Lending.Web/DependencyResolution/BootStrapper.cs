using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Lending.Execution.Auth;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.StructureMap;
using Nancy.Diagnostics;
using StructureMap;
using Nancy.Authentication.Forms;
using Nancy.Authentication.Token;
using Nancy.Conventions;
using Nancy.Owin;
using Nancy.Responses.Negotiation;

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
            var user = owinEnvironment["server.User"] as ClaimsPrincipal;
            if (user != null && user.Identity.IsAuthenticated)
            {
                context.CurrentUser =
                    new CustomUserIdentity(Guid.Parse(user.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value),
                        user.Identity.Name, user.Claims.Select(x => x.Value));
            }

        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);

            nancyConventions.StaticContentsConventions
                .Add(StaticContentConventionBuilder.AddDirectory("App", @"App"));
            nancyConventions.StaticContentsConventions
                .Add(StaticContentConventionBuilder.AddDirectory("fonts", @"fonts"));
            nancyConventions.StaticContentsConventions
                .Add(StaticContentConventionBuilder.AddDirectory("Scripts", @"Scripts"));
            nancyConventions.StaticContentsConventions
                .Add(StaticContentConventionBuilder.AddDirectory("Content", @"Content"));
        }

        protected override NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                return NancyInternalConfiguration.WithOverrides((c) =>
                {
                    c.ResponseProcessors.Clear();
                    c.ResponseProcessors.Add(typeof(JsonProcessor));
                });
            }
        }
    }
}
