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
            this.GivenCommand(JoshuaLewisOpensLibrary).IsPOSTedTo("/libraries");
            this.GivenCommand(SuzaanHepburnOpensLibrary).IsPOSTedTo("/libraries");
            this.GivenCommand(JosieDoeOpensLibrary).IsPOSTedTo("/libraries");
            this.GivenCommand(AudreyHepburnOpensLibrary).IsPOSTedTo("/libraries");
            this.WhenGetEndpoint("libraries/Lew");
            this.ThenResponseIs(new[]
            {
                new OpenedLibrary(Library1Id, JoshuaLewisLibraryOpened.Name, Library1Id),
            });
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
            this.GivenCommand(JoshuaLewisOpensLibrary).IsPOSTedTo("/libraries");
            this.GivenCommand(SuzaanHepburnOpensLibrary).IsPOSTedTo("/libraries");
            this.GivenCommand(JosieDoeOpensLibrary).IsPOSTedTo("/libraries");
            this.GivenCommand(AudreyHepburnOpensLibrary).IsPOSTedTo("/libraries");
            this.WhenGetEndpoint("libraries/lEw");
            this.ThenResponseIs(new[]
            {
                new OpenedLibrary(Library1Id, JoshuaLewisLibraryOpened.Name, Library1Id),
            });
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
            this.GivenCommand(JoshuaLewisOpensLibrary).IsPOSTedTo("/libraries");
            this.GivenCommand(SuzaanHepburnOpensLibrary).IsPOSTedTo("/libraries");
            this.GivenCommand(JosieDoeOpensLibrary).IsPOSTedTo("/libraries");
            this.GivenCommand(AudreyHepburnOpensLibrary).IsPOSTedTo("/libraries");
            this.WhenGetEndpoint("libraries/Pet");
            this.ThenResponseIs(new OpenedLibrary[] {});
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
            this.GivenCommand(JoshuaLewisOpensLibrary).IsPOSTedTo("/libraries");
            this.GivenCommand(SuzaanHepburnOpensLibrary).IsPOSTedTo("/libraries");
            this.GivenCommand(JosieDoeOpensLibrary).IsPOSTedTo("/libraries");
            this.GivenCommand(AudreyHepburnOpensLibrary).IsPOSTedTo("/libraries");
            this.WhenGetEndpoint("libraries/Jos");
            this.ThenResponseIs(new[]
            {
                new OpenedLibrary(Library1Id, JoshuaLewisLibraryOpened.Name, Library1Id),
                new OpenedLibrary(Library3Id, JosieDoeLibraryOpened.Name, Library3Id),
            });
            AndEventsSavedForAggregate<Library>(Library1Id, JoshuaLewisLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library2Id, SuzaanHepburnLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library3Id, JosieDoeLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library4Id, AudreyHepburnLibraryOpened);
        }

    }
}
