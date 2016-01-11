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
using Lending.Execution.UnitOfWork;
using Lending.ReadModels.Relational.BookAdded;
using Lending.ReadModels.Relational.ConnectionAccepted;
using Lending.ReadModels.Relational.SearchForBook;
using Lending.ReadModels.Relational.SearchForUser;
using Lending.ReadModels.Relational.UserRegistered;
using NCrunch.Framework;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using ServiceStack.Authentication.NHibernate;
using StructureMap;
using StructureMap.Graph;
using Configuration = NHibernate.Cfg.Configuration;

namespace Tests
{
    [ExclusivelyUses("Database")]
    [NUnit.Framework.Category("Persistence")]
    [TestFixture]
    public abstract class FixtureWithEventStoreAndNHibernate : FixtureWithEventStore
    {
        protected Configuration Configuration => Container.GetInstance<Configuration>();
        protected ISessionFactory SessionFactory => Container.GetInstance<ISessionFactory>();
        protected IRepository Repository => Container.GetInstance<IRepository>();
        protected ISession Session => Container.GetInstance<ISession>();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            //Create DB
            new SchemaExport(Configuration)
                .Execute(true, true, false);

            Container.GetInstance<IUnitOfWork>().Begin();

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
            Container.GetInstance<IUnitOfWork>().Commit();
        }

        protected override void CommitTransactionAndOpenNew()
        {
            CommitTransaction();

            Container.GetInstance<IUnitOfWork>().Begin();
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
            return result;
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

        protected override Action<IAssemblyScanner> ScannerAction
        {
            get
            {
                return x =>
                {
                    x.AssemblyContainingType<AcceptConnection>();
                    x.AssemblyContainingType<RegisteredUser>();
                };
            }
        }
    }
}
