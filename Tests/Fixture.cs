using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Joshilewis.Cqrs.Query;
using Lending.Execution;
using Lending.Execution.Auth;
using Lending.Execution.DI;
using Lending.ReadModels.Relational.BookAdded;
using Lending.ReadModels.Relational.LibraryOpened;
using Lending.ReadModels.Relational.LinkAccepted;
using Lending.ReadModels.Relational.LinkRequested;
using Lending.ReadModels.Relational.SearchForBook;
using Microsoft.Owin.Testing;
using NUnit.Framework;
using ServiceStack.ServiceModel.Serialization;
using StructureMap;
using static Tests.FixtureExtensions.DIExtensions;
using static Tests.FixtureExtensions.ApiExtensions;
using static Tests.FixtureExtensions.EventStoreExtensions;
using static Tests.FixtureExtensions.PersistenceExtensions;

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
            SetUpDependcyProvision(new TestRegistry());
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
