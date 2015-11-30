using Lending.Execution.Nancy;
using Nancy.Bootstrappers.StructureMap;
using Nancy.SimpleAuthentication;
using SimpleAuthentication.Core;
using SimpleAuthentication.Core.Providers;
using StructureMap;

namespace Shell
{
    public class BootStrapper : StructureMapNancyBootstrapper
    {
        protected override IContainer GetApplicationContainer()
        {
            return Program.Container;
        }

    }
}
