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
using Lending.Execution.EventStore;
using NUnit.Framework;
using StructureMap;
using StructureMap.Graph;

namespace Tests
{
    public abstract class FixtureWithEventStore : Fixture
    {
        protected ClusterVNode Node;
        protected IEventStoreConnection Connection;
        protected IEventRepository EventRepository;
        protected InMemoryEventConsumer EventConsumer;
        protected DummyEventHandlerProvider EventHandlerProvider;
        protected IContainer Container;

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

            Container = new Container(x =>
            {
                x.Scan(y =>
                {
                    y.WithDefaultConventions();
                    y.LookForRegistries();
                    y.AssemblyContainingType<Query>();

                    y.AddAllTypesOf(typeof (IMessageHandler<,>));
                    y.AddAllTypesOf(typeof(IAuthenticatedMessageHandler<,>));
                    y.AddAllTypesOf(typeof(ICommandHandler<,>));
                    y.AddAllTypesOf(typeof(IAuthenticatedCommandHandler<,>));
                    y.AddAllTypesOf(typeof(IEventHandler<>));
                    y.AddAllTypesOf(typeof(AuthenticatedCommandHandler<,>));

                    y.ConnectImplementationsToTypesClosing(typeof(IMessageHandler<,>));
                    y.ConnectImplementationsToTypesClosing(typeof(IAuthenticatedMessageHandler<,>));
                    y.ConnectImplementationsToTypesClosing(typeof(ICommandHandler<,>));
                    y.ConnectImplementationsToTypesClosing(typeof(IAuthenticatedCommandHandler<,>));
                    y.ConnectImplementationsToTypesClosing(typeof(IEventHandler<>));
                    y.ConnectImplementationsToTypesClosing(typeof(AuthenticatedCommandHandler<,>));
                    ScannerAction(y);
                });

                x.For<IEventRepository>()
                    .Use(EventRepository);

                ConfigurationExpressionAction(x);
            });

            var blah = Container.WhatDoIHave();
            Console.WriteLine(blah);
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

        protected virtual Result HandleMessages(params Message[] messages)
        {
            Result result = null;

            foreach (var message in messages)
            {
                Type type = typeof (IMessageHandler<,>).MakeGenericType(message.GetType(), typeof (Result));
                MessageHandler handler = (MessageHandler)Container.GetInstance(type);
                result = (Result)handler.Handle(message);
                WriteRepository();
                if (!result.Success) break;
            }

            return result;

        }

        protected virtual void HandleEvents(params Event[] events)
        {
            foreach (var @event in events)
            {
                Type type = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
                IEventHandler handler = (IEventHandler)Container.GetInstance(type);
                handler.When(@event);
            }

        }

    }
}
