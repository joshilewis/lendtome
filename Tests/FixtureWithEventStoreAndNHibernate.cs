using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using Lending.Cqrs;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Domain.AcceptConnection;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.RegisterUser;
using Lending.Domain.RemoveBookFromLibrary;
using Lending.Domain.RequestConnection;
using Lending.Execution.Auth;
using Lending.Execution.EventStore;
using Lending.Execution.Persistence;
using Lending.ReadModels.Relational.ConnectionAccepted;
using Lending.ReadModels.Relational.SearchForUser;
using NCrunch.Framework;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using ServiceStack.Authentication.NHibernate;
using Tests.ReadModels;
using Configuration = NHibernate.Cfg.Configuration;

namespace Tests
{
    [ExclusivelyUses("Database")]
    [NUnit.Framework.Category("Persistence")]
    [TestFixture]
    public abstract class FixtureWithEventStoreAndNHibernate : FixtureWithEventStore
    {
        protected static readonly Configuration Configuration;
        protected static readonly ISessionFactory SessionFactory;
        protected IRepository Repository;
        protected ISession Session;

        static FixtureWithEventStoreAndNHibernate()
        {
            //Set up database
            Configuration = Fluently.Configure()
                .Database(PostgreSQLConfiguration.PostgreSQL82
                    .ConnectionString(c => c.FromConnectionStringWithKey("lender_db"))
                    .DefaultSchema(ConfigurationManager.AppSettings["lender_db_schema"])
                    )
                .Mappings(m =>
                    m.FluentMappings
                        .AddFromAssemblyOf<RegisteredUserMap>()
                        .AddFromAssemblyOf<UserAuthPersistenceDto>()
                        .AddFromAssemblyOf<RegisteredUser>()
                        .AddFromAssemblyOf<UserConnection>()
                        .AddFromAssemblyOf<FixtureWithEventStoreAndNHibernate>()
                )
                .BuildConfiguration();

            SessionFactory = Configuration.BuildSessionFactory();
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            //Create DB
            new SchemaExport(Configuration)
                .Execute(true, true, false);

            Session = SessionFactory.OpenSession();
            Session.BeginTransaction();
            Repository = new NHibernateRepository(() => Session);

        }

        [TearDown]
        public override void TearDown()
        {
            CommitTransaction();

            //Tear down DB
            new SchemaExport(Configuration)
                .Execute(false, true, true);

            base.TearDown();
        }

        protected void CommitTransaction()
        {
            Session.Transaction.Commit();
            Session.Flush();
            Session.Transaction.Dispose();
            Session.Dispose();

        }

        protected void CommitTransactionAndOpenNew()
        {
            CommitTransaction();

            Session = SessionFactory.OpenSession();
            Session.BeginTransaction();

        }

        protected void SaveEntities(params object[] entitiesToSave)
        {
            foreach (var entity in entitiesToSave)
            {
                Repository.Save(entity);
            }
        }

        protected override Result HandleMessages(params Message[] messages)
        {
            Result result = base.HandleMessages(messages);
            CommitTransactionAndOpenNew();
            return result;
        }

        protected override Result DispatchMessage(Message message)
        {
            return HandleMessage((dynamic) message);
        }

        protected virtual void HandleEvents(params Event[] events)
        {
            foreach (var @event in events)
            {
                DispatchEvent(@event);
            }
            CommitTransactionAndOpenNew();

        }

        protected virtual void DispatchEvent(Event @event)
        {
            HandleEvent((dynamic)@event);
        }

        private Result HandleEvent(Event @event)
        {
            throw new NotImplementedException();
        }

        private Result HandleMessage(Message command)
        {
            throw new NotImplementedException();
        }

        protected void Given(params Message[] messages)
        {
            Result result = HandleMessages(messages);
            if(!result.Success) Assert.Fail();
        }

        protected void Given(params Event[] events)
        {
            HandleEvents(events);
        }

        private Result actualResult;
        protected void When(Message message)
        {
            actualResult = HandleMessages(message);
        }

        protected void Then(Result expectedResult)
        {
            actualResult.ShouldEqual(expectedResult);
        }

        protected void Then(Predicate<Result> resultEqualityPredicate)
        {
            resultEqualityPredicate(actualResult);
        }

        protected void AndEventsSavedForAggregate<TAggregate>(Guid aggregateId, params Event[] expectedEvents) where TAggregate : Aggregate
        {
            IEnumerable<Event> actualEvents = EventRepository.GetEventsForAggregate<TAggregate>(aggregateId);
            Assert.That(actualEvents, Is.EquivalentTo(expectedEvents));
        }

        private Result HandleMessage(RegisterUser message)
        {
            return new RegisterUserHandler(() => Repository, () => EventRepository).Handle(message);
        }

        private Result HandleMessage(RequestConnection message)
        {
            return new RequestConnectionHandler(() => Repository, () => EventRepository).Handle(message);
        }

        private Result HandleMessage(AcceptConnection message)
        {
            return new AcceptConnectionHandler(() => Repository, () => EventRepository).Handle(message);
        }

        private Result HandleMessage(AddBookToLibrary message)
        {
            return new AddBookToLibraryHandler(() => Repository, () => EventRepository).Handle(message);
        }

        private Result HandleMessage(RemoveBookFromLibrary message)
        {
            return new RemoveBookFromLibraryHandler(() => Repository, () => EventRepository).Handle(message);
        }

        private void HandleEvent(UserRegistered @event)
        {
        }

        private void HandleEvent(ConnectionAccepted @event)
        {
            new ConnectionAcceptedEventHandler(() => Session).When(@event);
        }

        private Result HandleMessage(SearchForBook message)
        {
            throw new NotImplementedException();
        }

        private Result HandleMessage(SearchForUser message)
        {
            return new SearchForUserHandler(() => Session).Handle(message);
        }

    }
}
