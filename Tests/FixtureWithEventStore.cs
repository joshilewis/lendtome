using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Embedded;
using EventStore.Core;
using Lending.Cqrs;
using Lending.Cqrs.Command;
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
using NHibernate;
using NUnit.Framework;
using ServiceStack.ServiceModel.Serialization;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Web;

namespace Tests
{
    public abstract class FixtureWithEventStore : Fixture
    {
        protected InMemoryEventConsumer EventConsumer;
        protected DummyEventHandlerProvider EventHandlerProvider;
        protected IContainer Container;
        protected EventDispatcher EventDispatcher;
        protected HttpClient Client;

        private TestServer server;

        protected virtual Action<ConfigurationExpression> ConfigurationExpressionAction
        {
            get { return x => { }; }
        }

        public override void SetUp()
        {
            base.SetUp();

            Container = IoC.Initialize(new TestRegistry());
            Container.GetInstance<ClusterVNode>().Start();
            Container.GetInstance<IEventStoreConnection>().ConnectAsync().Wait();

            server = TestServer.Create<Startup>();
            Client = server.HttpClient;
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJFeHBpcmVzIjoiXC9EYXRlKDE0NTQ2ODgxMTUyNjMpXC8iLCJDbGFpbXMiOlt7Iklzc3VlciI6IkxPQ0FMIEFVVEhPUklUWSIsIk9yaWdpbmFsSXNzdWVyIjoiTE9DQUwgQVVUSE9SSVRZIiwiUHJvcGVydGllcyI6e30sIlN1YmplY3QiOm51bGwsIlR5cGUiOiJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIiwiVmFsdWUiOiJKb3NodWEgTGV3aXMiLCJWYWx1ZVR5cGUiOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYSNzdHJpbmcifSx7Iklzc3VlciI6IkxPQ0FMIEFVVEhPUklUWSIsIk9yaWdpbmFsSXNzdWVyIjoiTE9DQUwgQVVUSE9SSVRZIiwiUHJvcGVydGllcyI6e30sIlN1YmplY3QiOm51bGwsIlR5cGUiOiJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciIsIlZhbHVlIjoiMGMzMTFmOGUtOWIwYS00YTIyLWFlNmUtMGM1N2YwNzczNThjIiwiVmFsdWVUeXBlIjoiaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEjc3RyaW5nIn1dfQ.GPW_aZii5oJflCa-AQKneXZHMFyuFy4F5e6Fi-1U4aU");
        }

        public override void TearDown()
        {
            Container.GetInstance<IEventStoreConnection>().Close();
            Container.GetInstance<IEventStoreConnection>().Dispose();
            Container.GetInstance<ClusterVNode>().Stop();
            server.Dispose();
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

        protected void Given(string url, Message message)
        {
            var response = Client.PostAsJsonAsync($"https://localhost/api/{url}", message).Result;
            Result result =
                JsonDataContractDeserializer.Instance.DeserializeFromString<Result>(
                    response.Content.ReadAsStringAsync().Result);
            CommitTransactionAndOpenNew();

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

        protected void Then(Exception expectedException)
        {
            actualException.ShouldEqual(expectedException);
        }

        protected Exception FailBecauseUnauthorized(Guid userId, Guid aggregateId, Type aggregateType)
        {
            return new NotAuthorizedException(userId, aggregateId, aggregateType);
        }

        protected void Then(Predicate<Result> resultEqualityPredicate)
        {
            resultEqualityPredicate(actualResult);
        }

        protected void Then<TResult>(Predicate<Result> resultEqualityPredicate) where TResult : Result
        {
            actualResult = JsonDataContractDeserializer.Instance.DeserializeFromString<TResult>(responseString);
            Then(resultEqualityPredicate);
        }

        protected void AndEventsSavedForAggregate<TAggregate>(Guid aggregateId, params Event[] expectedEvents) where TAggregate : Aggregate
        {
            IEnumerable<Event> actualEvents = EventRepository.GetEventsForAggregate<TAggregate>(aggregateId);
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
