using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
            //Given();
            WhenCommand(OpenLibrary1).IsPOSTedTo("/libraries");
            Then(Http201Created);
            AndGETTo("/libraries/").Returns(new LibrarySearchResult(Library1Id, Library1Opened.Name));
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened);
        }

        [Test]
        public void OpenLibraryForUserWithAnOpenLibraryShouldFailBecauseLibraryAlreadyOpened()
        {
            GivenCommand(OpenLibrary1).IsPOSTedTo("/libraries");
            WhenCommand(OpenLibrary1).IsPOSTedTo("/libraries");
            Then(Http400Because(OpenLibraryHandler.UserAlreadyOpenedLibrary));
            AndGETTo("/libraries/").Returns(new LibrarySearchResult(Library1Id, Library1Opened.Name));
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened);
        }

        [Test]
        public void ListLibrariesShouldShowOnlyLibrariesAdministeredByUser()
        {
            GivenCommands(OpenLibrary1, OpenLibrary2, OpenLibrary3).ArePOSTedTo("/libraries");
            WhenGetEndpoint("/libraries/").As(Library2Id);
            ThenResponseIs(new LibrarySearchResult(Library2Id, Library2Opened.Name));
        }

    }
}
