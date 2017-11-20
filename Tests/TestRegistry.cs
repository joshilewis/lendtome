using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Embedded;
using EventStore.Core;
using Joshilewis.Cqrs;
using Joshilewis.Infrastructure.Auth;
using Joshilewis.Infrastructure.EventRouting;
using Joshilewis.Infrastructure.UnitOfWork;
using Lending.Execution;
using NHibernate;
using Owin.StatelessAuth;
using StructureMap;
using StructureMap.Web;

namespace Tests
{
    public class TestRegistry : Registry
    {
        public TestRegistry()
        {
            var noIp = new IPEndPoint(IPAddress.None, 0);
            ClusterVNode node = EmbeddedVNodeBuilder
                .AsSingleNode()
                .WithInternalTcpOn(noIp)
                .WithInternalHttpOn(noIp)
                .RunInMemory()
                .Build();
            var connection = EmbeddedEventStoreConnection.Create(node);

            For<ClusterVNode>()
                .Singleton()
                .Use(node);

            For<IEventStoreConnection>()
                .Singleton()
                .Use(connection);

            For<EventStoreUnitOfWork>()
                .HybridHttpOrThreadLocalScoped()
                .Use<TestUnitOfWork>()
                .Ctor<IEventEmitter>()
                .Is(c => c.GetInstance<IEventEmitter>())
                ;

            //For<IEventRepository>()
            //    .Use(c => c.GetInstance<TestUnitOfWork>().EventRepository)
            //    ;
            For<NHibernateUnitOfWork>()
                .HybridHttpOrThreadLocalScoped()
                .Use<NHibernateUnitOfWork>();

            For<ITokenValidator>()
                .Use(new SecureTokenValidator(ConfigurationManager.AppSettings["jwt_secret"]));

        }
    }
}
