using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.StructureMap;
using Nancy.Diagnostics;
using StructureMap;
using Nancy.Authentication.Forms;
using Nancy.Authentication.Token;

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
            TokenAuthentication.Enable(pipelines, new TokenAuthenticationConfiguration(container.GetInstance<ITokenizer>()));
        }

        //protected override void ApplicationStartup(IContainer container, IPipelines pipelines)
        //{
        //    base.ApplicationStartup(container, pipelines);
        //    var formsAuthConfiguration =
        //                   new FormsAuthenticationConfiguration()
        //                   {
        //                       RedirectUrl = "~/signin",
        //                       //UserMapper = requestContainer.Resolve<IUserMapper>(),
        //                   };

        //    FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
        //}
    }
}
