using System;
using System.Collections.Generic;
using Joshilewis.Cqrs.Query;
using Joshilewis.Infrastructure;
using Joshilewis.Testing;
using Lending.Execution;
using Lending.Execution.DI;
using Lending.ReadModels.Relational.BookAdded;
using Lending.ReadModels.Relational.LibraryOpened;
using Lending.ReadModels.Relational.LinkAccepted;
using Lending.ReadModels.Relational.LinkRequested;
using Lending.ReadModels.Relational.ListLibrayLinks;
using Lending.ReadModels.Relational.ListRequestedLinks;
using Lending.ReadModels.Relational.SearchForBook;
using NUnit.Framework;
using static Joshilewis.Testing.Helpers.DIExtensions;
using static Joshilewis.Testing.Helpers.ApiExtensions;
using static Joshilewis.Testing.Helpers.EventStoreExtensions;
using static Joshilewis.Testing.Helpers.PersistenceExtensions;

namespace Tests
{
    [TestFixture]
    public abstract class Fixture
    {
        [SetUp]
        public virtual void SetUp()
        {
            SetUpDependcyProvision<LendingContainer>(new TestRegistry());
            SetUpOwinServer<Startup>();
            SetUpEventStore();
            SetUpPersistence();
        }

        [TearDown]
        public virtual void TearDown()
        {
            TearDownPersistence();
            TearDownEventStore();
            TearDownOwinServer();
        }

    }


}
