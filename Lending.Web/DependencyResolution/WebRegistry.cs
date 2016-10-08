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
            int eventStorePort = int.Parse(ConfigurationManager.AppSettings["EventStore:Port"]);

            For<EventStoreUnitOfWork>()
                .HybridHttpOrThreadLocalScoped()
                .Use<EventStoreUnitOfWork>()
                .Ctor<string>()
                .Is(eventStoreIpAddress)
                .Ctor<int>()
                .Is(eventStorePort);

            For<NHibernateUnitOfWork>()
                .HybridHttpOrThreadLocalScoped()
                .Use<NHibernateUnitOfWork>();

        }
    }
}
