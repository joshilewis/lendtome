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
    public class DatabaseAndEventStoreFixtureBase : DatabaseFixtureBase
    {
        protected static readonly ClusterVNode Node;
        protected IEventStoreConnection Connection;
        protected IRepository Repository;
        private ConcurrentQueue<Aggregate> aggregateQueue;

        static DatabaseAndEventStoreFixtureBase()
        {
            var noIp = new IPEndPoint(IPAddress.None, 0);
            Node = EmbeddedVNodeBuilder
                .AsSingleNode()
                .WithInternalTcpOn(noIp)
                .WithInternalHttpOn(noIp)
                .RunInMemory()
                .Build();
            Node.Start();
        }

        public override void SetUp()
        {
            base.SetUp();
            Connection = EmbeddedEventStoreConnection.Create(Node);
            Connection.ConnectAsync().Wait();

            aggregateQueue = new ConcurrentQueue<Aggregate>();

            Repository = new EventStoreRepository(aggregateQueue);
        }

        protected void WriteAggregates()
        {
            foreach (var aggregate in aggregateQueue)
            {
                foreach (var @event in aggregate.GetUncommittedEvents())
                {
                    string stream = $"{aggregate.GetType()}-{aggregate.Id}";
                    AppendEvent(new StreamEventTuple(stream, @event));
                }
            }
        }

        protected void WriteEvents(params StreamEventTuple[] eventsToWrite)
        {
            AppendEvents(eventsToWrite);
        }

        private void AppendEvents(IEnumerable<StreamEventTuple> eventsToWrite)
        {
            foreach (StreamEventTuple tuple in eventsToWrite)
            {
                AppendEvent(tuple);
            }
        }

        private void AppendEvent(StreamEventTuple tuple)
        {
            Connection.AppendToStreamAsync(tuple.Stream, ExpectedVersion.Any, tuple.Event.AsJson()).Wait();
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
