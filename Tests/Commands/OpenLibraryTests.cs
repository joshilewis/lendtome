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

namespace Tests.Commands
{
    [TestFixture]
    public class OpenLibraryTests : FixtureWithEventStoreAndNHibernate
    {
        [Test]
        public void OpenLibraryForUserWithNoLibrariesShouldOpenNewLibrary()
        {
            //Given();
            this.WhenCommand(OpenLibrary1).IsPOSTedTo("/libraries");
            this.Then(Http201Created);
            this.AndGETTo("/libraries/").Returns(new OpenedLibrary(Library1Id, Library1Opened.Name, Library1Id));
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened);
        }

        [Test]
        public void OpenLibraryForUserWithAnOpenLibraryShouldFailBecauseLibraryAlreadyOpened()
        {
            this.GivenCommand(OpenLibrary1).IsPOSTedTo("/libraries");
            this.WhenCommand(OpenLibrary1).IsPOSTedTo("/libraries");
            this.Then(this.Http400Because(OpenLibraryHandler.UserAlreadyOpenedLibrary));
            this.AndGETTo("/libraries/").Returns(new OpenedLibrary(Library1Id, Library1Opened.Name, Library1Id));
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened);
        }

        [Test]
        public void ListLibrariesShouldShowOnlyLibrariesAdministeredByUser()
        {
            this.GivenCommands(OpenLibrary1, OpenLibrary2, OpenLibrary3).ArePOSTedTo("/libraries");
            this.WhenGetEndpoint("/libraries/").As(Library2Id);
            this.ThenResponseIs(new OpenedLibrary(Library2Id, Library2Opened.Name, Library2Id));
        }

    }
}
