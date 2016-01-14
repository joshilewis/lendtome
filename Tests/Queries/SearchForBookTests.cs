using System;
using Lending.Cqrs.Query;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.RemoveBookFromLibrary;
using Lending.ReadModels.Relational.SearchForBook;
using NUnit.Framework;
using static Tests.DefaultTestData;

namespace Tests.Queries
{
    /// <summary>
    /// As a User I want to Search for Books in my Connections' Libraries so that I can find out if any of my Connections have the Book 
    /// I want to Borrow.
    /// </summary>
    public class SearchForBookTests : FixtureWithEventStoreAndNHibernate
    {

        /// <summary>
        /// GIVEN User1 has Registered AND User2 has Registered AND User1 has Request to Connect with User2 AND User2 
        /// has Accepted the Connection from User1
        /// WHEN User1 Searches for a Book with the Search Term "Extreme Programming Explained"
        /// THEN A successful result is returned with an empty list of owners
        /// </summary>
        [Test]
        public void SearchingForBookNotOwnedByAnyConnectionShouldReturnEmptyList()
        {
            var expectedResult = new Result<BookSearchResult[]>(new BookSearchResult[] { });

            Given(Library1Opened, Library2Opened, Link1To2Accepted);
            When(new SearchForBook(Library1Id, ExtremeProgrammingExplained));
            Then(x => ((Result<BookSearchResult[]>)x).ShouldEqual(expectedResult));
        }

        /// <summary>
        /// GIVEN User1 has Registered
        /// WHEN User1 Searches for a Book with the Search Term "Extreme Programming Explained"
        /// THEN A failed result is returned, with reason 'User has no Connections'
        /// </summary>
        [Test]
        public void SearchingForBookWithNoConnectionsShouldFail()
        {
            var expectedResult = new Result<BookSearchResult[]>(new BookSearchResult[] { });

            Given(Library1Opened);
            When(new SearchForBook(Library1Id, ExtremeProgrammingExplained));
            Then(x => ((Result<BookSearchResult[]>)x).ShouldEqual(expectedResult));

        }

        /// <summary>
        /// GIVEN User1, User2, User3 and User4 have all Registered, AND User1 is Connected to User2, User3 and User4 
        /// AND User2 and User4 have Added the Book ("Extreme Programming Explained", "Kent Beck") to their Libraries 
        /// AND User3 has Added the Book "Test-Driven Development", "Kent Beck") to her Library
        /// WHEN User1 Searches for a Book with the Search Term "Extreme Programming Eplained"
        /// THEN A successful result is returned with content('User2:("Extreme Programming Explained", "Kent Beck")', 
        /// 'User4':("Extreme Programming Explained", "Kent Beck"))'
        /// </summary>
        [Test]
        public void SearchingForBookWithSingleMatchingTitleInManyLibrariesShouldReturnAllOwners()
        {
            var expectedResult = new Result<BookSearchResult[]>(new BookSearchResult[]
            {
                new BookSearchResult(Library2Id, Library2Opened.Name, ExtremeProgrammingExplained, KentBeck),
                new BookSearchResult(Library4Id, Library4Opened.Name, ExtremeProgrammingExplained, KentBeck),
            });

            Given(Library1Opened, Library2Opened, Library3Opened, Library4Opened, 
                Link1To2Accepted, Link1To3Accepted, Link1To4Accepted,
                xpeByKbAddTo4, xpeByKbAddTo2, tddByKbAddTo3);
            When(new SearchForBook(Library1Id, ExtremeProgrammingExplained));
            Then(x => ((Result<BookSearchResult[]>)x).ShouldEqual(expectedResult));

        }

        /// <summary>
        /// GIVEN User1, User2, User3, User4 and User5 have all Registered, 
        /// AND User1 is Connected to User2, User3, User4 and User5 
        /// AND User2 and User4 have Added the Book ("Extreme Programming Explained", "Kent Beck") to their Libraries 
        /// AND User3 has Added the Book "Test-Driven Development", "Kent Beck") to her Library 
        /// AND User5 has Added the Book ("Extreme Snowboard Stunts", "Some Skiier")
        /// WHEN User1 Searches for a Book with the Search Term "Extreme"
        /// THEN A successful result is returned with content('User2:("Extreme Programming Explained", "Kent Beck")', 
        /// 'User4':("Extreme Programming Explained", "Kent Beck"), 'User5':("Extreme Snowboard Stunts", "Some Skiier"))
        /// </summary>
        [Test]
        public void SearchingForBookWithManyMatchingTitlesInManyLibrariesShouldReturnAllOwnersAndBooks()
        {
            var expectedResult = new Result<BookSearchResult[]>(new BookSearchResult[]
            {
                new BookSearchResult(Library2Id, Library2Opened.Name, ExtremeProgrammingExplained, KentBeck),
                new BookSearchResult(Library4Id, Library4Opened.Name, ExtremeProgrammingExplained, KentBeck),
                new BookSearchResult(Library5Id, Library5Opened.Name, ExtremeSnowboardStunts, SomeSkiier), 
            });

            Given(Library1Opened, Library2Opened, Library3Opened, Library4Opened, Library5Opened, 
                Link1To2Accepted, Link1To3Accepted, Link1To4Accepted, Link1To5Accepted,
                xpeByKbAddTo4, xpeByKbAddTo2, tddByKbAddTo3, essBySSAddTo5);
            When(new SearchForBook(Library1Id, "Extreme"));
            Then(x => ((Result<BookSearchResult[]>)x).ShouldEqual(expectedResult));

        }

        /// <summary>
        /// GIVEN User1, User2, User3, User4 and User5 have all Registered, 
        /// AND User1 is Connected to User2, User3, User4 and User5 
        /// AND User2 and User4 have Added the Book ("Extreme Programming Explained", "Kent Beck") to their Libraries 
        /// AND User3 has Added the Book "Test-Driven Development", "Kent Beck") to her Library 
        /// AND User5 has Added the Book ("Extreme Snowboard Stunts", "Some Skiier")
        /// WHEN User1 Searches for a Book with the Search Term "Kent Beck"
        /// THEN A successful result is returned with content ('User2:("Extreme Programming Explained", "Kent Beck")',
        ///  "Kent Beck"), 'User3':("Test-Driven Development", "Kent Beck"), 'User4':("Extreme Programming Explained"))
        /// </summary>
        [Test]
        public void SearchingForBookWithSingleMatchingAuthorInManyLibrariesShouldReturnAllOwnersAndBooks()
        {
            var expectedResult = new Result<BookSearchResult[]>(new BookSearchResult[]
            {
                new BookSearchResult(Library2Id, Library2Opened.Name, ExtremeProgrammingExplained, KentBeck),
                new BookSearchResult(Library3Id, Library3Opened.Name, TestDrivenDevelopment, KentBeck),
                new BookSearchResult(Library4Id, Library4Opened.Name, ExtremeProgrammingExplained, KentBeck),
            });

            Given(Library1Opened, Library2Opened, Library3Opened, Library4Opened, Library5Opened,
                Link1To2Accepted, Link1To3Accepted, Link1To4Accepted, Link1To5Accepted,
                xpeByKbAddTo2, tddByKbAddTo3, xpeByKbAddTo4, essBySSAddTo5);
            When(new SearchForBook(Library1Id, KentBeck));
            Then(x => ((Result<BookSearchResult[]>)x).ShouldEqual(expectedResult));

        }

        /// <summary>
        /// GIVEN User1, User2, User3, User4, User5 and User6 have all Registered, 
        /// AND User1 is Connected to User2, User3, User4, User5 and User6 
        /// AND User2 and User4 have Added the Book ("Extreme Programming Explained", "Kent Beck") to their Libraries 
        /// AND User3 has Added the Book "Test-Driven Development", "Kent Beck") to her Library 
        /// AND User5 has Added the Book ("Extreme Snowboard Stunts", "Some Skiier") 
        /// AND User6 has Added the Book ("Beck: A Musical Maestro", "Some Author")
        /// WHEN User1 Searches for a Book with the Search Term "Beck"
        /// THEN A successful result is returned with content('User2:("Extreme Programming Explained", "Kent Beck")', "Kent Beck"), 
        /// 'User3':("Test-Driven Development", "Kent Beck"), 'User4':("Extreme Programming Explained"), 'User6':("Beck: A Musical Maestro", "Some Author"))
        /// </summary>
        [Test]
        public void SearchingForBookWithManyMatchingTitlesAndAuthorsInManyLibrariesShouldReturnAllOwnersAndBooks()
        {
            var expectedResult = new Result<BookSearchResult[]>(new BookSearchResult[]
            {
                new BookSearchResult(Library2Id, Library2Opened.Name, ExtremeProgrammingExplained, KentBeck),
                new BookSearchResult(Library3Id, Library3Opened.Name, TestDrivenDevelopment, KentBeck),
                new BookSearchResult(Library4Id, Library4Opened.Name, ExtremeProgrammingExplained, KentBeck),
                new BookSearchResult(Library6Id, Library6Opened.Name, BeckAMusicalMaestro, SomeAuthor),
            });

            Given(Library1Opened, Library2Opened, Library3Opened, Library4Opened, Library5Opened, Library6Opened,
                Link1To2Accepted, Link1To3Accepted, Link1To4Accepted, Link1To5Accepted, Link1To6Accepted,
                xpeByKbAddTo2, tddByKbAddTo3, xpeByKbAddTo4, essBySSAddTo5, bBySAAddTo6);
            When(new SearchForBook(Library1Id, "Beck"));
            Then(x => ((Result<BookSearchResult[]>)x).ShouldEqual(expectedResult));

        }

        /// <summary>
        /// GIVEN User1, User2, User3, User4, User5 and User6 have all Registered, 
        /// AND User1 is Connected to User2, User3, User5 and User6 
        /// AND User2 and User4 have Added the Book ("Extreme Programming Explained", "Kent Beck") to their Libraries 
        /// AND User3 has Added the Book "Test-Driven Development", "Kent Beck") to her Library 
        /// AND User5 has Added the Book ("Extreme Snowboard Stunts", "Some Skiier") 
        /// AND User6 has Added the Book ("Beck: A Musical Maestro", "Some Author")
        /// WHEN User1 Searches for a Book with the Search Term "Beck"
        /// THEN A successful result is returned with content('User2:("Extreme Programming Explained", "Kent Beck")', "Kent Beck"), 
        /// 'User3':("Test-Driven Development", "Kent Beck"), 'User6':("Beck: A Musical Maestro", "Some Author"))
        /// </summary>
        [Test]
        public void SearchingForBookWithManyMatchesShouldExcludeUnconnectedLibraries()
        {
            var expectedResult = new Result<BookSearchResult[]>(new BookSearchResult[]
            {
                new BookSearchResult(Library2Id, Library2Opened.Name, ExtremeProgrammingExplained, KentBeck),
                new BookSearchResult(Library3Id, Library3Opened.Name, TestDrivenDevelopment, KentBeck),
                new BookSearchResult(Library6Id, Library6Opened.Name, BeckAMusicalMaestro, SomeAuthor),
            });

            Given(Library1Opened, Library2Opened, Library3Opened, Library4Opened, Library5Opened, Library6Opened,
                Link1To2Accepted, Link1To3Accepted, Link1To5Accepted, Link1To6Accepted,
                xpeByKbAddTo2, tddByKbAddTo3, xpeByKbAddTo4, essBySSAddTo5, bBySAAddTo6);
            When(new SearchForBook(Library1Id, "Beck"));
            Then(x => ((Result<BookSearchResult[]>)x).ShouldEqual(expectedResult));

        }

        /// <summary>
        /// GIVEN User1, User2, User3, User4, User5 and User6 have all Registered, 
        /// AND User1 is Connected to User2, User3, User4, User5 and User6 
        /// AND User2 and User4 have Added the Book ("Extreme Programming Explained", "Kent Beck") to their Libraries 
        /// AND User3 has Added the Book ("Test-Driven Development", "Kent Beck") to her Library 
        /// AND User5 has Added the Book ("Extreme Snowboard Stunts", "Some Skiier") 
        /// AND User6 has Added the Book ("Beck: A Musical Maestro", "Some Author")
        /// AND User4 Removes the Book ("Extreme Programming Explained", "Kent Beck") from their Library
        /// WHEN User1 Searches for a Book with the Search Term "Beck"
        /// THEN A successful result is returned with content('User2:("Extreme Programming Explained", "Kent Beck")', "Kent Beck"), 
        /// 'User3':("Test-Driven Development", "Kent Beck"), 'User6':("Beck: A Musical Maestro", "Some Author"))
        /// </summary>
        [Test]
        public void SearchingForBookWithManyMatchingTitlesAndAuthorsInManyLibrariesShouldReturnAllOwnersAndBooksExceptRemovedBooks()
        {
            var expectedResult = new Result<BookSearchResult[]>(new BookSearchResult[]
            {
                new BookSearchResult(Library2Id, Library2Opened.Name, ExtremeProgrammingExplained, KentBeck),
                new BookSearchResult(Library3Id, Library3Opened.Name, TestDrivenDevelopment, KentBeck),
                new BookSearchResult(Library6Id, Library6Opened.Name, BeckAMusicalMaestro, SomeAuthor),
            });

            Given(Library1Opened, Library2Opened, Library3Opened, Library4Opened, Library5Opened, Library6Opened,
                Link1To2Accepted, Link1To3Accepted, Link1To4Accepted, Link1To5Accepted, Link1To6Accepted,
                xpeByKbAddTo2, tddByKbAddTo3, xpeByKbAddTo4, essBySSAddTo5, bBySAAddTo6, xpeByKbRemoveFrom4);
            When(new SearchForBook(Library1Id, "Beck"));
            Then(x => ((Result<BookSearchResult[]>)x).ShouldEqual(expectedResult));

        }

    }
}
