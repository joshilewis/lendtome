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
using Lending.Domain.RegisterUser;
using Lending.Domain.RequestConnection;
using Lending.Execution.Auth;
using Lending.Execution.EventStore;
using Lending.Execution.Persistence;
using Lending.ReadModels.Relational.ConnectionAccepted;
using NCrunch.Framework;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using ServiceStack.Authentication.NHibernate;
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

        protected override Result HandleCommands(params Command[] commands)
        {
            Result result = base.HandleCommands(commands);
            CommitTransactionAndOpenNew();
            return result;
        }

        protected override Result DispatchCommand(Command command)
        {
            return HandleCommand((dynamic) command);
        }

        private Result HandleCommand(Command command)
        {
            return null;
        }

        protected void Given(params Command[] commands)
        {
            Result result = HandleCommands(commands);
            if(!result.Success) Assert.Fail();
        }

        private Result actualResult;
        protected void When(Command command)
        {
            actualResult = HandleCommands(command);
        }

        protected void Then(Result expectedResult)
        {
            actualResult.ShouldEqual(expectedResult);
        }

        protected void And<TAggregate>(Guid aggregateId, IEnumerable<Event> expectedEvents) where TAggregate : Aggregate
        {
            IEnumerable<Event> actualEvents = EventRepository.GetEventsForAggregate<TAggregate>(aggregateId);
            Assert.That(actualEvents, Is.EquivalentTo(expectedEvents));
        }

        private Result HandleCommand(RegisterUser command)
        {
            return new RegisterUserHandler(() => Repository, () => EventRepository).Handle(command);
        }

        private Result HandleCommand(RequestConnection command)
        {
            return new RequestConnectionHandler(() => Repository, () => EventRepository).Handle(command);
        }

        private Result HandleCommand(AcceptConnection command)
        {
            return new AcceptConnectionHandler(() => Repository, () => EventRepository).Handle(command);
        }
    }
}
