﻿using System;
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
            WhenCommand(OpenLibrary1).IsPUTedTo("/libraries");
            Then(Http201Created);
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened);
            AndGETTo<Result<OpenedLibrary[]>>("/libraries/").Returns(new Result<OpenedLibrary[]>(new[]
            {
                new OpenedLibrary(Library1Id, Library1Opened.Name, Library1Id),
            }
                ));
        }

        [Test]
        public void OpenLibraryForUserWithAnOpenLibraryShouldFailBecauseLibraryAlreadyOpened()
        {
            GivenCommand(OpenLibrary1).IsPUTedTo("/libraries");
            WhenCommand(OpenLibrary1).IsPUTedTo("/libraries");
            Then(Http400Because(OpenLibraryHandler.UserAlreadyOpenedLibrary));
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened);
            AndGETTo<Result<OpenedLibrary[]>>("/libraries/").Returns(new Result<OpenedLibrary[]>(new[]
            {
                new OpenedLibrary(Library1Id, Library1Opened.Name, Library1Id),
            }
                ));
        }

    }
}
