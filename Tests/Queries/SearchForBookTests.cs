using System;
using Lending.Cqrs.Query;
using Lending.Domain.AcceptConnection;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.RegisterUser;
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

            Given(user1Registered, user2Registered, conn1To2Accepted);
            When(new SearchForBook(user1Id, ExtremeProgrammingExplained));
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
            var expectedResult = new Result<BookSearchResult[]>(SearchForBookHandler.UserHasNoConnection, new BookSearchResult[] { });

            Given(user1Registered);
            When(new SearchForBook(user1Id, ExtremeProgrammingExplained));
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
                new BookSearchResult(user2Id, user2Registered.UserName, ExtremeProgrammingExplained, KentBeck),
                new BookSearchResult(user4Id, user4Registered.UserName, ExtremeProgrammingExplained, KentBeck),
            });

            Given(user1Registered, user2Registered, user3Registered, user4Registered, 
                conn1To2Accepted, conn1To3Accepted, conn1To4Accepted,
                xpeByKbAddTo4, xpeByKbAddTo2, tddByKbAddTo3);
            When(new SearchForBook(user1Id, ExtremeProgrammingExplained));
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
                new BookSearchResult(user2Id, user2Registered.UserName, ExtremeProgrammingExplained, KentBeck),
                new BookSearchResult(user4Id, user4Registered.UserName, ExtremeProgrammingExplained, KentBeck),
                new BookSearchResult(user5Id, user5Registered.UserName, ExtremeSnowboardStunts, SomeSkiier), 
            });

            Given(user1Registered, user2Registered, user3Registered, user4Registered, user5Registered, 
                conn1To2Accepted, conn1To3Accepted, conn1To4Accepted, conn1To5Accepted,
                xpeByKbAddTo4, xpeByKbAddTo2, tddByKbAddTo3, essBySSAddTo5);
            When(new SearchForBook(user1Id, "Extreme"));
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
                new BookSearchResult(user2Id, user2Registered.UserName, ExtremeProgrammingExplained, KentBeck),
                new BookSearchResult(user3Id, user3Registered.UserName, TestDrivenDevelopment, KentBeck),
                new BookSearchResult(user4Id, user4Registered.UserName, ExtremeProgrammingExplained, KentBeck),
            });

            Given(user1Registered, user2Registered, user3Registered, user4Registered, user5Registered,
                conn1To2Accepted, conn1To3Accepted, conn1To4Accepted, conn1To5Accepted,
                xpeByKbAddTo2, tddByKbAddTo3, xpeByKbAddTo4, essBySSAddTo5);
            When(new SearchForBook(user1Id, KentBeck));
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
                new BookSearchResult(user2Id, user2Registered.UserName, ExtremeProgrammingExplained, KentBeck),
                new BookSearchResult(user3Id, user3Registered.UserName, TestDrivenDevelopment, KentBeck),
                new BookSearchResult(user4Id, user4Registered.UserName, ExtremeProgrammingExplained, KentBeck),
                new BookSearchResult(user6Id, user6Registered.UserName, BeckAMusicalMaestro, SomeAuthor),
            });

            Given(user1Registered, user2Registered, user3Registered, user4Registered, user5Registered, user6Registered,
                conn1To2Accepted, conn1To3Accepted, conn1To4Accepted, conn1To5Accepted, conn1To6Accepted,
                xpeByKbAddTo2, tddByKbAddTo3, xpeByKbAddTo4, essBySSAddTo5, bBySAAddTo6);
            When(new SearchForBook(user1Id, "Beck"));
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
                new BookSearchResult(user2Id, user2Registered.UserName, ExtremeProgrammingExplained, KentBeck),
                new BookSearchResult(user3Id, user3Registered.UserName, TestDrivenDevelopment, KentBeck),
                new BookSearchResult(user6Id, user6Registered.UserName, BeckAMusicalMaestro, SomeAuthor),
            });

            Given(user1Registered, user2Registered, user3Registered, user4Registered, user5Registered, user6Registered,
                conn1To2Accepted, conn1To3Accepted, conn1To5Accepted, conn1To6Accepted,
                xpeByKbAddTo2, tddByKbAddTo3, xpeByKbAddTo4, essBySSAddTo5, bBySAAddTo6);
            When(new SearchForBook(user1Id, "Beck"));
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
                new BookSearchResult(user2Id, user2Registered.UserName, ExtremeProgrammingExplained, KentBeck),
                new BookSearchResult(user3Id, user3Registered.UserName, TestDrivenDevelopment, KentBeck),
                new BookSearchResult(user6Id, user6Registered.UserName, BeckAMusicalMaestro, SomeAuthor),
            });

            Given(user1Registered, user2Registered, user3Registered, user4Registered, user5Registered, user6Registered,
                conn1To2Accepted, conn1To3Accepted, conn1To4Accepted, conn1To5Accepted, conn1To6Accepted,
                xpeByKbAddTo2, tddByKbAddTo3, xpeByKbAddTo4, essBySSAddTo5, bBySAAddTo6, xpeByKbRemoveFrom4);
            When(new SearchForBook(user1Id, "Beck"));
            Then(x => ((Result<BookSearchResult[]>)x).ShouldEqual(expectedResult));

        }

    }
}
