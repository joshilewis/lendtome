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
using Lending.Core;
using Lending.Execution.EventStore;
using NUnit.Framework;

namespace Tests
{
    public class DatabaseAndEventStoreFixtureBase : DatabaseFixtureBase
    {
        protected static readonly ClusterVNode Node;
        protected IEventStoreConnection Connection;
        protected IEventEmitter Emitter;

        static DatabaseAndEventStoreFixtureBase()
        {
            var noIp = new IPEndPoint(IPAddress.None, 0);
            Node = EmbeddedVNodeBuilder
                .AsSingleNode()
                .WithInternalTcpOn(noIp)
                .WithInternalHttpOn(noIp)
                .RunInMemory()
                .Build();
        }

        public override void SetUp()
        {
            base.SetUp();
            Connection = EmbeddedEventStoreConnection.Create(Node);
            Connection.ConnectAsync().Wait();

            Emitter = new EventStoreEventEmitter(new ConcurrentQueue<Event>());
        }

        protected void WriteEvents()
        {
            foreach (Event @event in ((EventStoreEventEmitter) Emitter).Queue)
            {
                string streamName = String.Format("{0}-{1}", @event.GetType().Name, @event.Id);
                Connection.AppendToStreamAsync(streamName, ExpectedVersion.Any, @event.AsJson()).Wait();
            }
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
