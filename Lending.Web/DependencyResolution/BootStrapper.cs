using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Bootstrappers.StructureMap;
using StructureMap;

namespace Lending.Web.DependencyResolution
{
    public class BootStrapper : StructureMapNancyBootstrapper
    {
        protected override IContainer GetApplicationContainer()
        {
            return MvcApplication.Container;
        }
    }
}
