using System;
using Lending.Cqrs.Query;
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
            Given(joshuaLewisLibraryOpened, suzaanHepburnLibraryOpened, josieDoe3LibraryOpened, audreyHepburn4LibraryOpened);
            When(new SearchForLibrary("Lew"));
            Then(actualResult => ((Result<OpenedLibrary[]>)actualResult).ShouldEqual(new Result<OpenedLibrary[]>(new OpenedLibrary[]
            {
                new OpenedLibrary(Library1Id, joshuaLewisLibraryOpened.Name), 
            })));
        }

        /// <summary>
        /// GIVEN Users with the following names 'Joshua Lewis', 'Suzaan Hepburn', 'Joshua Doe', 'Audrey Hepburn' have Registered 
        /// WHEN I Search for Users with the search string 'lEw' 
        /// THEN 'Joshua Lewis' gets returne
        /// </summary>
        [Test]
        public void SearchingForLibraryWithSingleMatchWithWrongCaseShouldReturnThatUser()
        {
            Given(joshuaLewisLibraryOpened, suzaanHepburnLibraryOpened, josieDoe3LibraryOpened, audreyHepburn4LibraryOpened);
            When(new SearchForLibrary("lEw"));
            Then(actualResult => ((Result<OpenedLibrary[]>)actualResult).ShouldEqual(new Result<OpenedLibrary[]>(new OpenedLibrary[]
            {
                new OpenedLibrary(Library1Id, joshuaLewisLibraryOpened.Name),
            })));
        }

        /// <summary>
        /// GIVEN Users with the following names 'Joshua Lewis', 'Suzaan Hepburn', 'Joshua Doe', 'Audrey Hepburn' have Registered 
        /// WHEN I Search for Users with the search string 'Pet'
        /// THEN 'Joshua Lewis' gets returne
        /// </summary>
        [Test]
        public void SearchingForLibraryWithNoMatchesShouldReturnEmptyList()
        {
            Given(joshuaLewisLibraryOpened, suzaanHepburnLibraryOpened, josieDoe3LibraryOpened, audreyHepburn4LibraryOpened);
            When(new SearchForLibrary("Pet"));
            Then(actualResult => ((Result<OpenedLibrary[]>)actualResult).ShouldEqual(new Result<OpenedLibrary[]>(new OpenedLibrary[]
            {
            })));

        }

        /// <summary>
        /// GIVEN Users with the following names 'Joshua Lewis', 'Suzaan Hepburn', 'Joshua Doe', 'Audrey Hepburn' have Registered 
        /// WHEN I Search for Users with the search string 'Jos'
        /// THEN 'Joshua Lewis' gets returne
        /// </summary>
        [Test]
        public void SearchingForLibraryWithTwoMatchsShouldReturnTwoUsers()
        {
            Given(joshuaLewisLibraryOpened, suzaanHepburnLibraryOpened, josieDoe3LibraryOpened, audreyHepburn4LibraryOpened);
            When(new SearchForLibrary("Jos"));
            Then(actualResult => ((Result<OpenedLibrary[]>)actualResult).ShouldEqual(new Result<OpenedLibrary[]>(new OpenedLibrary[]
            {
                new OpenedLibrary(Library1Id, joshuaLewisLibraryOpened.Name),
                new OpenedLibrary(Library3Id, josieDoe3LibraryOpened.Name),
            })));
        }

    }
}
