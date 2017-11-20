using System.Configuration;
using Joshilewis.Infrastructure.Auth;
using Joshilewis.Infrastructure.DI;
using Microsoft.Owin.Extensions;
using Owin;
using Owin.StatelessAuth;

namespace Joshilewis.Infrastructure
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var pathsToIgnore = new[]
            {
                "/authentication/**",
                "/authentication",
            };

            app.Map("/api", site =>
            {
                site.RequiresStatelessAuth(//new SecureTokenValidator(ConfigurationManager.AppSettings["jwt_secret"]),
                                           //new FirebaseTokenValidator(),
                    IoC.Container.GetInstance<ITokenValidator>(),
                    new StatelessAuthOptions
                    {
                        IgnorePaths = pathsToIgnore,
                        PassThroughUnauthorizedRequests = true
                    });
                site.UseNancy();
                site.UseStageMarker(PipelineStage.MapHandler);
            });
        }
    }
}
