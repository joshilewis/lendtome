using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Joshilewis.Infrastructure.UnitOfWork;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using StructureMap.Web;

namespace Lending.Web.DependencyResolution
{
    public class WebRegistry : Registry
    {
        public WebRegistry()
        {
            string eventStoreIpAddress = ConfigurationManager.AppSettings["EventStore:IPAddress"];

            For<IUnitOfWork>()
                .HybridHttpOrThreadLocalScoped()
                .Use<UnitOfWork>()
                .Ctor<string>()
                .Is(eventStoreIpAddress);

        }
    }
}
