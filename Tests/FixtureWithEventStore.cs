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

            Container.GetInstance<ClusterVNode>().Start();
            Container.GetInstance<IEventStoreConnection>().ConnectAsync().Wait();

        }

        public override void TearDown()
        {
            Container.GetInstance<IEventStoreConnection>().Close();
            Container.GetInstance<IEventStoreConnection>().Dispose();
            Container.GetInstance<ClusterVNode>().Stop();
            base.TearDown();
        }

        protected virtual Result HandleMessages(params Message[] messages)
        {
            Result result = null;

            foreach (var message in messages)
            {
                result = HandleMessage(message);
            }

            return result;

        }

        private Result HandleMessage(Message message)
        {
            Type type = typeof(IMessageHandler<,>).MakeGenericType(message.GetType(), typeof(Result));
            MessageHandler handler = (MessageHandler)Container.GetInstance(type);
            Result result = (Result)handler.Handle(message);
            CommitTransactionAndOpenNew();
            return result;
        }

        protected abstract void CommitTransactionAndOpenNew();

        protected virtual void HandleEvents(params Event[] events)
        {
            foreach (var @event in events)
            {
                Type type = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
                IEventHandler handler = (IEventHandler)Container.GetInstance(type);
                handler.When(@event);
                CommitTransactionAndOpenNew();
            }

        }

        protected IEventRepository EventRepository => Container.GetInstance<IEventRepository>();

        protected void Given(params Message[] messages)
        {
            Result result = HandleMessages(messages);
        }

        protected void Given(params Event[] events)
        {
            HandleEvents(events);
        }

        private Result actualResult;
        private Exception actualException;
        protected void When(Message message)
        {
            try
            {
                actualResult = HandleMessages(message);
            }
            catch (Exception exception)
            {
                actualException = exception;
            }
        }

        protected void Then(Result expectedResult)
        {
            actualResult.ShouldEqual(expectedResult);
        }

        protected void Then<TResult>(Result expectedResult) where TResult : Result
        {
            actualResult = JsonDataContractDeserializer.Instance.DeserializeFromString<TResult>(responseString);
            Then(expectedResult);
        }

        protected void Then(Predicate<Result> resultEqualityPredicate)
        {
            resultEqualityPredicate(actualResult);
        }

        protected void AndEventsSavedForAggregate<TAggregate>(Guid aggregateId, params Event[] expectedEvents) where TAggregate : Aggregate
        {
            IEnumerable<Event> actualEvents = EventRepository.GetEventsForAggregate<TAggregate>(aggregateId);
            foreach (Event @event in actualEvents)
            {
                @event.ProcessId = Guid.Empty;
            }
            Assert.That(actualEvents, Is.EquivalentTo(expectedEvents));
        }

        private string responseString;
        protected void WhenGetEndpoint(string uri)
        {
            try
            {
                responseString = HitEndPoint(uri);
            }
            catch (Exception exception)
            {
                actualException = exception;
            }
        }

        protected string HitEndPoint(string uri)
        {
            string path = $"https://localhost/api/{uri}/";
            var response = Client.GetAsync(path).Result;
            return response.Content.ReadAsStringAsync().Result;
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
