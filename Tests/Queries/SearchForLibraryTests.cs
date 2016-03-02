using System;
using Joshilewis.Testing.Helpers;
using Lending.Domain.Model;
using Lending.Domain.OpenLibrary;
using Lending.ReadModels.Relational.LibraryOpened;
using Lending.ReadModels.Relational.ListLibrayLinks;
using Lending.ReadModels.Relational.SearchForLibrary;
using NUnit.Framework;
using static Tests.TestData;
using static Joshilewis.Testing.Helpers.ApiExtensions;
using static Joshilewis.Testing.Helpers.EventStoreExtensions;

namespace Tests.Queries
{
    [TestFixture]
    public class SearchForLibraryTests: Fixture
    {
        private readonly LibrarySearchResult joshuaLewisLibraryResult = new LibrarySearchResult(Library1Id, JoshuaLewisLibraryOpened.Name, Library1Picture);

        /// <summary>
        /// GIVEN Libraries with the following names 'Joshua Lewis', 'Suzaan Hepburn', 'Joshua Doe', 'Audrey Hepburn' have been Opened 
        /// WHEN I Search for Libraries with the search string 'Lew' 
        /// THEN Lbrary 'Joshua Lewis' gets returned
        /// </summary>
        [Test]
        public void SearchingForLibraryWithSingleMatchShouldReturnThatUser()
        {
            PersistenceExtensions.SaveEntities(User1, User2, User3, User4);
            GivenCommand(JoshuaLewisOpensLibrary).IsPOSTedTo("/libraries");
            GivenCommand(SuzaanHepburnOpensLibrary).IsPOSTedTo("/libraries");
            GivenCommand(JosieDoeOpensLibrary).IsPOSTedTo("/libraries");
            GivenCommand(AudreyHepburnOpensLibrary).IsPOSTedTo("/libraries");
            WhenGetEndpoint("libraries/Lew");
            ThenResponseIs(joshuaLewisLibraryResult);
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
            PersistenceExtensions.SaveEntities(User1, User2, User3, User4);
            GivenCommand(JoshuaLewisOpensLibrary).IsPOSTedTo("/libraries");
            GivenCommand(SuzaanHepburnOpensLibrary).IsPOSTedTo("/libraries");
            GivenCommand(JosieDoeOpensLibrary).IsPOSTedTo("/libraries");
            GivenCommand(AudreyHepburnOpensLibrary).IsPOSTedTo("/libraries");
            WhenGetEndpoint("libraries/lEw");
            ThenResponseIs(joshuaLewisLibraryResult);
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
            PersistenceExtensions.SaveEntities(User1, User2, User3, User4);
            GivenCommand(JoshuaLewisOpensLibrary).IsPOSTedTo("/libraries");
            GivenCommand(SuzaanHepburnOpensLibrary).IsPOSTedTo("/libraries");
            GivenCommand(JosieDoeOpensLibrary).IsPOSTedTo("/libraries");
            GivenCommand(AudreyHepburnOpensLibrary).IsPOSTedTo("/libraries");
            WhenGetEndpoint("libraries/Pet");
            ThenResponseIs(new LibrarySearchResult[] {});
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
            PersistenceExtensions.SaveEntities(User1, User2, User3, User4);
            GivenCommand(JoshuaLewisOpensLibrary).IsPOSTedTo("/libraries");
            GivenCommand(SuzaanHepburnOpensLibrary).IsPOSTedTo("/libraries");
            GivenCommand(JosieDoeOpensLibrary).IsPOSTedTo("/libraries");
            GivenCommand(AudreyHepburnOpensLibrary).IsPOSTedTo("/libraries");
            WhenGetEndpoint("libraries/Jos");
            ThenResponseIs(
                joshuaLewisLibraryResult,
                new LibrarySearchResult(Library3Id, JosieDoeLibraryOpened.Name, Library3Picture));
            AndEventsSavedForAggregate<Library>(Library1Id, JoshuaLewisLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library2Id, SuzaanHepburnLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library3Id, JosieDoeLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library4Id, AudreyHepburnLibraryOpened);
        }

    }
}
