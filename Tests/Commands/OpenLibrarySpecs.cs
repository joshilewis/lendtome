using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Joshilewis.Testing.Helpers;
using Lending.Domain.Model;
using Lending.Domain.OpenLibrary;
using Lending.ReadModels.Relational.LibraryOpened;
using Lending.ReadModels.Relational.ListLibrayLinks;
using NUnit.Framework;
using static Tests.TestData;
using static Joshilewis.Testing.Helpers.ApiExtensions;
using static Joshilewis.Testing.Helpers.EventStoreExtensions;
using static Tests.LendingPersistenceExtentions;
using static Tests.AutomationExtensions;

namespace Tests.Commands
{
    [TestFixture]
    public class OpenLibrarySpecs : Fixture
    {
        [Test]
        public void OpenLibraryForUserWithNoLibrariesShouldOpenNewLibrary()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            When(() => OpenLibrary(transactionId, userId, "library1"));
            Then1(() => LibraryCreatedSuccessfully());
            AndGETTo("/libraries/").Returns(new LibrarySearchResult(userId, "library1", "user1Picture"));
            AndEventsSavedForAggregate<Library>(userId, new LibraryOpened(transactionId, userId, "library1", userId));
        }

        [Test]
        public void OpenLibraryForUserWithAnOpenLibraryShouldFailBecauseLibraryAlreadyOpened()
        {
            PersistenceExtensions.SaveEntities(User1);
            GivenCommand(OpenLibrary1).IsPOSTedTo("/libraries");
            WhenCommand(OpenLibrary1).IsPOSTedTo("/libraries");
            Then(Http400Because(LibraryOpener.UserAlreadyOpenedLibrary));
            AndGETTo("/libraries/").Returns(new LibrarySearchResult(Library1Id, Library1Name, Library1Picture));
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened);
        }

        [Test]
        public void ListLibrariesShouldShowOnlyLibrariesAdministeredByUser()
        {
            PersistenceExtensions.SaveEntities(User1, User2, User3);
            GivenCommands(OpenLibrary1, OpenLibrary2, OpenLibrary3).ArePOSTedTo("/libraries");
            WhenGetEndpoint("/libraries/").As(Library2Id);
            ThenResponseIs(new LibrarySearchResult(Library2Id, Library2Name, Library2Picture));
        }

    }
}
