using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Joshilewis.Cqrs.Exceptions;
using Joshilewis.Infrastructure.Auth;
using Joshilewis.Infrastructure.DI;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.StructureMap;
using Nancy.Diagnostics;
using Nancy.Extensions;
using Nancy.Owin;
using Nancy.Responses.Negotiation;
using StructureMap;

namespace Joshilewis.Infrastructure
{
    public class BootStrapper : StructureMapNancyBootstrapper
    {
        protected override IContainer GetApplicationContainer()
        {
            return IoC.Container;
        }

        protected override DiagnosticsConfiguration DiagnosticsConfiguration
            => new DiagnosticsConfiguration {Password = @"secret"};

        protected override void ApplicationStartup(IContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            pipelines.OnError.AddItemToEndOfPipeline((context, exception) =>
            {
                HttpStatusCode code = HttpStatusCode.BadRequest;
                if (exceptionStatusCodeMap.ContainsKey(exception.GetType())) code = exceptionStatusCodeMap[exception.GetType()];

                string reasonPhrase = exception.Message;
                if (exception is AggregateNotFoundException) reasonPhrase = "Not Found";

                return new Response()
                {
                    ReasonPhrase = reasonPhrase,
                    StatusCode = code
                };
            });

        }
        private readonly Dictionary<Type, HttpStatusCode> exceptionStatusCodeMap = new Dictionary<Type, HttpStatusCode>()
        {
            {typeof(AggregateNotFoundException), HttpStatusCode.NotFound },
            {typeof(NotAuthorizedException), HttpStatusCode.Forbidden },
            {typeof(Exception), HttpStatusCode.BadRequest },

        };

        protected override void RequestStartup(IContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);
            var owinEnvironment = context.GetOwinEnvironment();
            if (!owinEnvironment.ContainsKey("server.User")) return;

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
