using Nancy.Bootstrappers.StructureMap;
using Nancy.Diagnostics;
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

        protected override DiagnosticsConfiguration DiagnosticsConfiguration => new DiagnosticsConfiguration { Password = @"secret" };
    }
}
