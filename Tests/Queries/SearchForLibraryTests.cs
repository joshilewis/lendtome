using System;
using Lending.Cqrs.Query;
using Lending.Domain.Model;
using Lending.Domain.OpenLibrary;
using Lending.ReadModels.Relational.SearchForLibrary;
using NUnit.Framework;
using static Tests.DefaultTestData;

namespace Tests.Queries
{
    [TestFixture]
    public class SearchForLibraryTests: FixtureWithEventStoreAndNHibernate
    {

        /// <summary>
        /// GIVEN Users with the following names 'Joshua Lewis', 'Suzaan Hepburn', 'Joshua Doe', 'Audrey Hepburn' have Registered 
        /// WHEN I Search for Users with the search string 'Lew' 
        /// THEN 'Joshua Lewis' gets returne
        /// </summary>
        [Test]
        public void SearchingForLibraryWithSingleMatchShouldReturnThatUser()
        {
            Given(JoshuaLewisOpensLibrary, SuzaanHepburnOpensLibrary, JosieDoeOpensLibrary, AudreyHepburnOpensLibrary);
            WhenGetEndpoint("libraries/Lew");
            Then<Result<OpenedLibrary[]>>(actualResult => ((Result<OpenedLibrary[]>)actualResult).ShouldEqual(new Result<OpenedLibrary[]>(new OpenedLibrary[]
            {
                new OpenedLibrary(Library1Id, JoshuaLewisLibraryOpened.Name), 
            })));
            AndEventsSavedForAggregate<Library>(Library1Id, JoshuaLewisLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library2Id, SuzaanHepburnLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library3Id, JosieDoeLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library4Id, AudreyHepburnLibraryOpened);
        }

        /// <summary>
        /// GIVEN Users with the following names 'Joshua Lewis', 'Suzaan Hepburn', 'Joshua Doe', 'Audrey Hepburn' have Registered 
        /// WHEN I Search for Users with the search string 'lEw' 
        /// THEN 'Joshua Lewis' gets returne
        /// </summary>
        [Test]
        public void SearchingForLibraryWithSingleMatchWithWrongCaseShouldReturnThatUser()
        {
            Given(JoshuaLewisOpensLibrary, SuzaanHepburnOpensLibrary, JosieDoeOpensLibrary, AudreyHepburnOpensLibrary);
            WhenGetEndpoint("libraries/lEw");
            Then<Result<OpenedLibrary[]>>(actualResult => ((Result<OpenedLibrary[]>)actualResult).ShouldEqual(new Result<OpenedLibrary[]>(new OpenedLibrary[]
            {
                new OpenedLibrary(Library1Id, JoshuaLewisLibraryOpened.Name),
            })));
            AndEventsSavedForAggregate<Library>(Library1Id, JoshuaLewisLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library2Id, SuzaanHepburnLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library3Id, JosieDoeLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library4Id, AudreyHepburnLibraryOpened);
        }

        /// <summary>
        /// GIVEN Users with the following names 'Joshua Lewis', 'Suzaan Hepburn', 'Joshua Doe', 'Audrey Hepburn' have Registered 
        /// WHEN I Search for Users with the search string 'Pet'
        /// THEN 'Joshua Lewis' gets returne
        /// </summary>
        [Test]
        public void SearchingForLibraryWithNoMatchesShouldReturnEmptyList()
        {
            Given(JoshuaLewisOpensLibrary, SuzaanHepburnOpensLibrary, JosieDoeOpensLibrary, AudreyHepburnOpensLibrary);
            WhenGetEndpoint("libraries/Pet");
            Then<Result<OpenedLibrary[]>>(actualResult => ((Result<OpenedLibrary[]>)actualResult).ShouldEqual(new Result<OpenedLibrary[]>(new OpenedLibrary[]
            {
            })));
            AndEventsSavedForAggregate<Library>(Library1Id, JoshuaLewisLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library2Id, SuzaanHepburnLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library3Id, JosieDoeLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library4Id, AudreyHepburnLibraryOpened);

        }

        /// <summary>
        /// GIVEN Users with the following names 'Joshua Lewis', 'Suzaan Hepburn', 'Joshua Doe', 'Audrey Hepburn' have Registered 
        /// WHEN I Search for Users with the search string 'Jos'
        /// THEN 'Joshua Lewis' gets returne
        /// </summary>
        [Test]
        public void SearchingForLibraryWithTwoMatchsShouldReturnTwoUsers()
        {
            Given(JoshuaLewisOpensLibrary, SuzaanHepburnOpensLibrary, JosieDoeOpensLibrary, AudreyHepburnOpensLibrary);
            WhenGetEndpoint("libraries/Jos");
            Then<Result<OpenedLibrary[]>>(actualResult => ((Result<OpenedLibrary[]>)actualResult).ShouldEqual(new Result<OpenedLibrary[]>(new OpenedLibrary[]
            {
                new OpenedLibrary(Library1Id, JoshuaLewisLibraryOpened.Name),
                new OpenedLibrary(Library3Id, JosieDoeLibraryOpened.Name),
            })));
            AndEventsSavedForAggregate<Library>(Library1Id, JoshuaLewisLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library2Id, SuzaanHepburnLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library3Id, JosieDoeLibraryOpened);
            AndEventsSavedForAggregate<Library>(Library4Id, AudreyHepburnLibraryOpened);
        }

    }
}
