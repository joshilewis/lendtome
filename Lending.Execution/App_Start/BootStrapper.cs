using System;
using System.Linq;
using System.Security.Claims;
using Lending.Cqrs.Exceptions;
using Lending.Execution.Auth;
using Lending.Execution.DI;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.StructureMap;
using Nancy.Diagnostics;
using Nancy.Owin;
using Nancy.Responses.Negotiation;
using StructureMap;

namespace Lending.Execution.App_Start
{
    public class BootStrapper : StructureMapNancyBootstrapper
    {
        protected override IContainer GetApplicationContainer()
        {
            return IoC.Container;
        }

        protected override DiagnosticsConfiguration DiagnosticsConfiguration => new DiagnosticsConfiguration { Password = @"secret" };

        protected override void ApplicationStartup(IContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            pipelines.OnError.AddItemToEndOfPipeline((context, exception) =>
            {
                if (exception is AggregateNotFoundException) return new NotFoundResponse()
                {
                    ReasonPhrase = exception.Message,
                };

                if (exception is NotAuthorizedException) return new Response()
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    ReasonPhrase = exception.Message,
                };

                return new Response()
                {
                    ReasonPhrase = exception.Message,
                    StatusCode = HttpStatusCode.BadRequest,
                };
            });
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
