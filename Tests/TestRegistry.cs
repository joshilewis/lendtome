using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Embedded;
using EventStore.Core;
using Lending.Execution;
using Lending.Execution.UnitOfWork;
using NHibernate;
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

            For<IUnitOfWork>()
                .HybridHttpOrThreadLocalScoped()
                .Use<TestUnitOfWork>()
                .Ctor<ISessionFactory>()
                .Is(c => c.GetInstance<ISessionFactory>())
                .Ctor<IEventEmitter>()
                .Is(c => c.GetInstance<IEventEmitter>())
                .Ctor<EventDispatcher>()
                .Is(c => c.GetInstance<EventDispatcher>())
                ;
        }
    }
}
