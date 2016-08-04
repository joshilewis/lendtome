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

namespace Tests.Commands
{
    [TestFixture]
    public class OpenLibraryTests : Fixture
    {
        [Test]
        public void OpenLibraryForUserWithNoLibrariesShouldOpenNewLibrary()
        {
            PersistenceExtensions.SaveEntities(User1);
            //Given();
            WhenCommand(OpenLibrary1).IsPOSTedTo("/libraries");
            Then(Http201Created);
            AndGETTo("/libraries/").Returns(new LibrarySearchResult(Library1Id, Library1Name, Library1Picture));
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened);
        }

        [Test]
        public void OpenLibraryForUserWithAnOpenLibraryShouldFailBecauseLibraryAlreadyOpened()
        {
            PersistenceExtensions.SaveEntities(User1);
            GivenCommand(OpenLibrary1).IsPOSTedTo("/libraries");
            WhenCommand(OpenLibrary1).IsPOSTedTo("/libraries");
            Then(Http400Because(OpenLibraryHandler.UserAlreadyOpenedLibrary));
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
