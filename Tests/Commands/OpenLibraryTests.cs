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
            WhenCommand(OpenLibrary1).IsPOSTedTo("/libraries");
            Then(Http201Created);
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened);
            AndGETTo<OpenedLibrary[]>("/libraries/").Returns(x => x.ShouldEqual(new[]
            {
                new OpenedLibrary(Library1Id, Library1Opened.Name, Library1Id),
            }
                ));
        }

        [Test]
        public void OpenLibraryForUserWithAnOpenLibraryShouldFailBecauseLibraryAlreadyOpened()
        {
            GivenCommand(OpenLibrary1).IsPOSTedTo("/libraries");
            WhenCommand(OpenLibrary1).IsPOSTedTo("/libraries");
            Then(Http400Because(OpenLibraryHandler.UserAlreadyOpenedLibrary));
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened);
            AndGETTo<OpenedLibrary[]>("/libraries/").Returns(x => x.ShouldEqual(new[]
            {
                new OpenedLibrary(Library1Id, Library1Opened.Name, Library1Id),
            }
                ));
        }

        [Test]
        public void ListLibrariesShouldShowOnlyLibrariesAdministeredByUser()
        {
            GivenCommands(OpenLibrary1, OpenLibrary2, OpenLibrary3).ArePOSTedTo("/libraries");
            WhenGetEndpoint("/libraries/", Library2Id);
            Then<Result<OpenedLibrary[]>>(x => x.ShouldEqual(new Result<OpenedLibrary[]>(new[]
            {
                new OpenedLibrary(Library2Id, Library2Opened.Name, Library2Id),
            })));
        }

    }
}
