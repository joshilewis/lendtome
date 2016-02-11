using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs.Query;
using Lending.Domain.Model;
using Lending.Domain.OpenLibrary;
using Lending.ReadModels.Relational.LibraryOpened;
using NUnit.Framework;
using static Tests.DefaultTestData;
using static Tests.FixtureExtensions.ApiExtensions;

namespace Tests.Commands
{
    [TestFixture]
    public class OpenLibraryTests : FixtureWithEventStoreAndNHibernate
    {
        [Test]
        public void OpenLibraryForUserWithNoLibrariesShouldOpenNewLibrary()
        {
            //Given();
            WhenCommand(OpenLibrary1).IsPOSTedTo("/libraries");
            Then(Http201Created);
            AndGETTo("/libraries/").Returns(new OpenedLibrary(Library1Id, Library1Opened.Name, Library1Id));
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened);
        }

        [Test]
        public void OpenLibraryForUserWithAnOpenLibraryShouldFailBecauseLibraryAlreadyOpened()
        {
            GivenCommand(OpenLibrary1).IsPOSTedTo("/libraries");
            WhenCommand(OpenLibrary1).IsPOSTedTo("/libraries");
            Then(Http400Because(OpenLibraryHandler.UserAlreadyOpenedLibrary));
            AndGETTo("/libraries/").Returns(new OpenedLibrary(Library1Id, Library1Opened.Name, Library1Id));
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened);
        }

        [Test]
        public void ListLibrariesShouldShowOnlyLibrariesAdministeredByUser()
        {
            GivenCommands(OpenLibrary1, OpenLibrary2, OpenLibrary3).ArePOSTedTo("/libraries");
            WhenGetEndpoint("/libraries/").As(Library2Id);
            ThenResponseIs(new OpenedLibrary(Library2Id, Library2Opened.Name, Library2Id));
        }

    }
}
