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
        static Fixture()
        {
            TestValueEqualityHelpers.SetValueEqualityActions(new Dictionary<Type, Action<object, object>>()
            {
                {typeof(Result<RequestedLink[]>), (actual, expected) => ((Result<RequestedLink[]>)actual).ShouldEqual((Result<RequestedLink[]>)expected) },
                {typeof(Result<LibraryLink[]>), (actual, expected) => ((Result<LibraryLink[]>)actual).ShouldEqual((Result<LibraryLink[]>)expected) },
                {typeof(Result<OpenedLibrary[]>), (actual, expected) => ((Result<OpenedLibrary[]>)actual).ShouldEqual((Result<OpenedLibrary[]>)expected) },
                {typeof(Result<BookSearchResult[]>), (actual, expected) => ((Result<BookSearchResult[]>)actual).ShouldEqual((Result<BookSearchResult[]>)expected) },
                {typeof(Result<LibraryBook[]>), (actual, expected) => ((Result<LibraryBook[]>)actual).ShouldEqual((Result<LibraryBook[]>)expected) },
            });
        }

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
