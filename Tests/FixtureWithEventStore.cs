using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Embedded;
using EventStore.Core;
using Lending.Domain;
using Lending.Execution.EventStore;
using NUnit.Framework;

namespace Tests
{
    public class FixtureWithEventStore : Fixture
    {
        protected ClusterVNode Node;
        protected IEventStoreConnection Connection;
        protected IRepository Repository;

        public override void SetUp()
        {
            base.SetUp();
            var noIp = new IPEndPoint(IPAddress.None, 0);
            Node = EmbeddedVNodeBuilder
                .AsSingleNode()
                .WithInternalTcpOn(noIp)
                .WithInternalHttpOn(noIp)
                .RunInMemory()
                .Build();
            Node.Start();

            Connection = EmbeddedEventStoreConnection.Create(Node);
            Connection.ConnectAsync().Wait();

            Repository = new EventStoreRepository(new DummyEventEmitter(), Connection);
        }

        protected void WriteRepository()
        {
            ((EventStoreRepository)Repository).Commit(Guid.NewGuid());
        }

        protected void SaveAggregates(params Aggregate[] aggregatesToSave)
        {
            foreach (var aggregate in aggregatesToSave)
            {
                Repository.Save(aggregate);
            }
            ((EventStoreRepository)Repository).Commit(Guid.NewGuid());
        }

        public override void TearDown()
        {
            Connection.Close();
            Connection.Dispose();
            Node.Stop();
            base.TearDown();
        }
    }

}
