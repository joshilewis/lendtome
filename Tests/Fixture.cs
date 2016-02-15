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
            TestValueEqualityHelpers.SetValueEqualityActions(
                new EqualityAction<RequestedLink[]>((a, e) =>Assert.That(a,Is.EquivalentTo(e).Using((IEqualityComparer<RequestedLink>) new ValueEqualityComparer()))),
                new EqualityAction<LibraryLink[]>((a, e) =>Assert.That(a,Is.EquivalentTo(e).Using((IEqualityComparer<LibraryLink>) new ValueEqualityComparer()))),
                new EqualityAction<LibrarySearchResult[]>((a, e) => Assert.That(a, Is.EquivalentTo(e))),
                new EqualityAction<BookSearchResult[]>((a, e) => Assert.That(a, Is.EquivalentTo(e))),
                new EqualityAction<LibraryBook[]>((a, e) =>Assert.That(a,Is.EquivalentTo(e).Using((IEqualityComparer<LibraryBook>) new ValueEqualityComparer()))));
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
