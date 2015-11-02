using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Embedded;
using EventStore.Core;
using Lending.Cqrs;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Domain;
using Lending.Domain.RegisterUser;
using Lending.Execution;
using Lending.Execution.EventStore;
using NUnit.Framework;

namespace Tests
{
    public abstract class FixtureWithEventStore : Fixture
    {
        protected ClusterVNode Node;
        protected IEventStoreConnection Connection;
        protected IEventRepository EventRepository;
        protected InMemoryEventConsumer EventConsumer;
        protected DummyEventHandlerProvider EventHandlerProvider;

        public override void SetUp()
        {
            base.SetUp();
            EventHandlerProvider = new DummyEventHandlerProvider();
            EventConsumer = new InMemoryEventConsumer(EventHandlerProvider);
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
            EventRepository = new EventStoreEventRepository(new InMemoryEventEmitter(), Connection);
        }

        protected void RegisterEventHandler<TEvent>(IEventHandler eventHandler) where TEvent : Event
        {
            EventHandlerProvider.RegisterHandler<TEvent>(eventHandler);
        }

        protected void WriteRepository()
        {
            ((EventStoreEventRepository)EventRepository).Commit(Guid.NewGuid());
        }

        protected void SaveAggregates(params Aggregate[] aggregatesToSave)
        {
            foreach (var aggregate in aggregatesToSave)
            {
                EventRepository.Save(aggregate);
            }
            ((EventStoreEventRepository)EventRepository).Commit(Guid.NewGuid());
        }

        public override void TearDown()
        {
            Connection.Close();
            Connection.Dispose();
            Node.Stop();
            base.TearDown();
        }

        protected virtual Result HandleCommands(params Command[] commands)
        {
            Result result = null;

            foreach (var command in commands)
            {
                result = DispatchCommand(command);
                WriteRepository();
                if (!result.Success) break;
            }

            return result;

        }

        protected abstract Result DispatchCommand(Command command);

    }
}
