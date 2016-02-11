using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Embedded;
using EventStore.Core;
using JWT;
using Lending.Cqrs;
using Lending.Cqrs.Exceptions;
using Lending.Cqrs.Query;
using Lending.Domain;
using Lending.Domain.OpenLibrary;
using Lending.Execution;
using Lending.Execution.DI;
using Lending.Execution.EventStore;
using Lending.Execution.UnitOfWork;
using Lending.ReadModels.Relational.SearchForLibrary;
using Microsoft.Owin.Testing;
using Nancy;
using NHibernate;
using NUnit.Framework;
using ServiceStack.ServiceModel.Serialization;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Web;
using HttpStatusCode = System.Net.HttpStatusCode;
using static Tests.FixtureExtensions;

namespace Tests
{
    public abstract class FixtureWithEventStore : Fixture
    {
        protected InMemoryEventConsumer EventConsumer;
        protected DummyEventHandlerProvider EventHandlerProvider;
        protected EventDispatcher EventDispatcher;

        public override void SetUp()
        {
            base.SetUp();

            GetContainer().GetInstance<ClusterVNode>().Start();
            GetContainer().GetInstance<IEventStoreConnection>().ConnectAsync().Wait();

        }

        public override void TearDown()
        {
            GetContainer().GetInstance<IEventStoreConnection>().Close();
            GetContainer().GetInstance<IEventStoreConnection>().Dispose();
            GetContainer().GetInstance<ClusterVNode>().Stop();
            base.TearDown();
        }

        protected IEventRepository EventRepository => GetContainer().GetInstance<IEventRepository>();

        protected void AndEventsSavedForAggregate<TAggregate>(Guid aggregateId, params Event[] expectedEvents) where TAggregate : Aggregate        {
            IEnumerable<Event> actualEvents = EventRepository.GetEventsForAggregate<TAggregate>(aggregateId);
            foreach (Event @event in actualEvents)
            {
                @event.ProcessId = Guid.Empty;
            }
            Assert.That(actualEvents, Is.EquivalentTo(expectedEvents));
        }


    }

    public class TestUnitOfWork : UnitOfWork
    {
        public TestUnitOfWork(ISessionFactory sessionFactory, IEventStoreConnection eventStoreConnection,
            IEventEmitter eventEmitter, EventDispatcher eventDispatcher)
            : base(sessionFactory, eventStoreConnection, eventEmitter, eventDispatcher)
        {
        }
    }
}
