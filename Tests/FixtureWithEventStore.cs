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
using Lending.Domain.AcceptConnection;
using Lending.Domain.RegisterUser;
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
        protected ClusterVNode Node;
        protected IEventStoreConnection Connection;
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

            Container = new Container(x =>
            {
                x.For<IUnitOfWork>()
                    .HybridHttpOrThreadLocalScoped()
                    .Use<TestUnitOfWork>()
                    .Ctor<IEventStoreConnection>()
                    .Is(Connection)
                    .Ctor<ISessionFactory>()
                    .Is(c => c.GetInstance<ISessionFactory>())
                    .Ctor<IEventEmitter>()
                    .Is(c => c.GetInstance<IEventEmitter>())
                    .Ctor<EventDispatcher>()
                    .Is(c => c.GetInstance<EventDispatcher>())
                    ;

                x.Scan(y =>
                {
                    y.WithDefaultConventions();
                    y.LookForRegistries();
                    y.AssemblyContainingType<Query>();
                    y.AssemblyContainingType<DomainRegistry>();

                    ScannerAction(y);


                });


                ConfigurationExpressionAction(x);
            });

            var blah = Container.WhatDoIHave();
            //Console.WriteLine(blah);
        }

        public override void TearDown()
        {
            Connection.Close();
            Connection.Dispose();
            Node.Stop();
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
                if (!result.Success) return result;
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
