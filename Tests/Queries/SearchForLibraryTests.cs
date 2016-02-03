using System;
using Lending.Cqrs.Query;
using Lending.Domain.Model;
using Lending.Domain.OpenLibrary;
using Lending.ReadModels.Relational.LibraryOpened;
using Lending.ReadModels.Relational.SearchForLibrary;
using NUnit.Framework;
using static Tests.DefaultTestData;

namespace Tests.Queries
{
    [TestFixture]
    public class SearchForLibraryTests: FixtureWithEventStoreAndNHibernate
    {

        /// <summary>
        /// GIVEN Libraries with the following names 'Joshua Lewis', 'Suzaan Hepburn', 'Joshua Doe', 'Audrey Hepburn' have been Opened 
        /// WHEN I Search for Libraries with the search string 'Lew' 
        /// THEN Lbrary 'Joshua Lewis' gets returned
        /// </summary>
        [Test]
        public void SearchingForLibraryWithSingleMatchShouldReturnThatUser()
        {
            GivenCommand(JoshuaLewisOpensLibrary).IsPOSTedTo("/libraries");
            GivenCommand(SuzaanHepburnOpensLibrary).IsPOSTedTo("/libraries");
            GivenCommand(JosieDoeOpensLibrary).IsPOSTedTo("/libraries");
            GivenCommand(AudreyHepburnOpensLibrary).IsPOSTedTo("/libraries");
            WhenGetEndpoint("libraries/Lew");
            Then<Result<OpenedLibrary[]>>(x => x.ShouldEqual(new Result<OpenedLibrary[]>(new[]
            {
                new OpenedLibrary(Library1Id, JoshuaLewisLibraryOpened.Name, Library1Id),
            })));
            AndEventsSavedForAggregate<Library>(Library1Id, JoshuaLewisLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library2Id, SuzaanHepburnLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library3Id, JosieDoeLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library4Id, AudreyHepburnLibraryOpened);
        }

        /// <summary>
        /// GIVEN Libraries with the following names 'Joshua Lewis', 'Suzaan Hepburn', 'Joshua Doe', 'Audrey Hepburn' have been Opened 
        /// WHEN I Search for Libraries with the search string 'lEw' 
        /// THEN Library 'Joshua Lewis' gets returned
        /// </summary>
        [Test]
        public void SearchingForLibraryWithSingleMatchWithWrongCaseShouldReturnThatUser()
        {
            GivenCommand(JoshuaLewisOpensLibrary).IsPOSTedTo("/libraries");
            GivenCommand(SuzaanHepburnOpensLibrary).IsPOSTedTo("/libraries");
            GivenCommand(JosieDoeOpensLibrary).IsPOSTedTo("/libraries");
            GivenCommand(AudreyHepburnOpensLibrary).IsPOSTedTo("/libraries");
            WhenGetEndpoint("libraries/lEw");
            Then<Result<OpenedLibrary[]>>(x => x.ShouldEqual(new Result<OpenedLibrary[]>(new[]
            {
                new OpenedLibrary(Library1Id, JoshuaLewisLibraryOpened.Name, Library1Id),
            })));
            AndEventsSavedForAggregate<Library>(Library1Id, JoshuaLewisLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library2Id, SuzaanHepburnLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library3Id, JosieDoeLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library4Id, AudreyHepburnLibraryOpened);
        }

        /// <summary>
        /// GIVEN Libraries with the following names 'Joshua Lewis', 'Suzaan Hepburn', 'Joshua Doe', 'Audrey Hepburn' have been Opened 
        /// WHEN I Search for Libraries with the search string 'Pet'
        /// THEN no Libraries get returned
        /// </summary>
        [Test]
        public void SearchingForLibraryWithNoMatchesShouldReturnEmptyList()
        {
            GivenCommand(JoshuaLewisOpensLibrary).IsPOSTedTo("/libraries");
            GivenCommand(SuzaanHepburnOpensLibrary).IsPOSTedTo("/libraries");
            GivenCommand(JosieDoeOpensLibrary).IsPOSTedTo("/libraries");
            GivenCommand(AudreyHepburnOpensLibrary).IsPOSTedTo("/libraries");
            WhenGetEndpoint("libraries/Pet");
            Then<Result<OpenedLibrary[]>>(x => x.ShouldEqual(new Result<OpenedLibrary[]>(new OpenedLibrary[]
            {
            })));
            AndEventsSavedForAggregate<Library>(Library1Id, JoshuaLewisLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library2Id, SuzaanHepburnLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library3Id, JosieDoeLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library4Id, AudreyHepburnLibraryOpened);

        }

        /// <summary>
        /// GIVEN Libraries with the following names 'Joshua Lewis', 'Suzaan Hepburn', 'Joshua Doe', 'Audrey Hepburn' have been Opened 
        /// WHEN I Search for Libraries with the search string 'Jos'
        /// THEN Libraries 'Joshua Lewis' and 'Josie Doe' get returned
        /// </summary>
        [Test]
        public void SearchingForLibraryWithTwoMatchsShouldReturnTwoLibraries()
        {
            GivenCommand(JoshuaLewisOpensLibrary).IsPOSTedTo("/libraries");
            GivenCommand(SuzaanHepburnOpensLibrary).IsPOSTedTo("/libraries");
            GivenCommand(JosieDoeOpensLibrary).IsPOSTedTo("/libraries");
            GivenCommand(AudreyHepburnOpensLibrary).IsPOSTedTo("/libraries");
            WhenGetEndpoint("libraries/Jos");
            Then<Result<OpenedLibrary[]>>(x => x.ShouldEqual(new Result<OpenedLibrary[]>(new[]
            {
                new OpenedLibrary(Library1Id, JoshuaLewisLibraryOpened.Name, Library1Id),
                new OpenedLibrary(Library3Id, JosieDoeLibraryOpened.Name, Library3Id),
            })));
            AndEventsSavedForAggregate<Library>(Library1Id, JoshuaLewisLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library2Id, SuzaanHepburnLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library3Id, JosieDoeLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library4Id, AudreyHepburnLibraryOpened);
        }

    }
}
