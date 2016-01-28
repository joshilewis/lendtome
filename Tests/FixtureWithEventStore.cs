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
using Lending.Execution;
using Lending.Execution.DI;
using Lending.Execution.EventStore;
using Lending.Execution.UnitOfWork;
using NHibernate;
using NUnit.Framework;
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

        protected virtual Action<IAssemblyScanner> ScannerAction
        {
            get { return x => { }; }
        }

        protected virtual Action<ConfigurationExpression> ConfigurationExpressionAction
        {
            get { return x => { }; }
        }

        public override void SetUp()
        {
            base.SetUp();

            Container = new LendingContainer();
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
                Type type = typeof (IMessageHandler<,>).MakeGenericType(message.GetType(), typeof (Result));
                MessageHandler handler = (MessageHandler)Container.GetInstance(type);
                result = (Result)handler.Handle(message);
                CommitTransactionAndOpenNew();
            }

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
            }

        }

        protected IEventRepository EventRepository => Container.GetInstance<IEventRepository>();
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
