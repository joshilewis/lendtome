using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs.Query;
using Lending.Domain.AcceptConnection;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.RegisterUser;
using Lending.Domain.RequestConnection;
using Lending.ReadModels.Relational.ConnectionAccepted;
using Lending.ReadModels.Relational.SearchForBook;
using NUnit.Framework;

namespace Tests.ReadModels
{
    /// <summary>
    /// As a User I want to Search for Books in my Connections' Libraries so that I can find out if any of my Connections have the Book 
    /// I want to Borrow.
    /// </summary>
    public class SearchForBookTests : FixtureWithEventStoreAndNHibernate
    {

        #region Fields
        private Guid processId = Guid.Empty;
        private Guid user1Id;
        private Guid user2Id;
        private Guid user3Id;
        private Guid user4Id;
        private Guid user5Id;
        private Guid user6Id;

        //Events
        private UserRegistered user1Registered;
        private UserRegistered user2Registered;
        private UserRegistered user3Registered;
        private UserRegistered user4Registered;
        private UserRegistered user5Registered;
        private UserRegistered user6Registered;

        private ConnectionAccepted conn1To2Accepted;
        private ConnectionAccepted conn1To3Accepted;
        private ConnectionAccepted conn1To4Accepted;
        private ConnectionAccepted conn1To5Accepted;
        private ConnectionAccepted conn1To6Accepted;
        private BookAddedToLibrary tddByKbAddTo3;
        private BookAddedToLibrary xpeByKbAddTo4;
        private BookAddedToLibrary xpeByKbAddTo2;
        private BookAddedToLibrary essBySSAddTo5;
        private BookAddedToLibrary bBySAAddTo6;

        private const string TestDrivenDevelopment = "Test-Driven Development";
        private const string KentBeck = "Kent Beck";
        private const string Isbn = "Isbn";
        private const string ExtremeProgrammingExplained = "Extreme Programming Explained";
        private const string ExtremeSnowboardStunts = "Extreme Snowboard Stunts";
        private const string SomeSkiier = "Some Skiier";
        private const string BeckAMusicalMaestro = "Beck: A musical Maestro";
        private const string SomeAuthor = "Some Author";

        public override void SetUp()
        {
            base.SetUp();
            user1Id = Guid.NewGuid();
            user2Id = Guid.NewGuid();
            user3Id = Guid.NewGuid();
            user4Id = Guid.NewGuid();
            user5Id = Guid.NewGuid();
            user6Id = Guid.NewGuid();
            processId = Guid.NewGuid();

            user1Registered = new UserRegistered(processId, user1Id, 1, "User1", "Email1");
            user2Registered = new UserRegistered(processId, user2Id, 2, "User2", "Email2");
            user3Registered = new UserRegistered(processId, user3Id, 3, "User3", "Email3");
            user4Registered = new UserRegistered(processId, user4Id, 4, "User4", "Email4");
            user5Registered = new UserRegistered(processId, user5Id, 5, "User5", "Email5");
            user6Registered = new UserRegistered(processId, user6Id, 6, "User6", "Email6");

            conn1To2Accepted = new ConnectionAccepted(processId, user2Id, user1Id);
            conn1To3Accepted = new ConnectionAccepted(processId, user3Id, user1Id);
            conn1To4Accepted = new ConnectionAccepted(processId, user4Id, user1Id);
            conn1To5Accepted = new ConnectionAccepted(processId, user5Id, user1Id);
            conn1To6Accepted = new ConnectionAccepted(processId, user6Id, user1Id);

            xpeByKbAddTo2 = new BookAddedToLibrary(processId, user2Id, ExtremeProgrammingExplained, KentBeck, Isbn);
            xpeByKbAddTo4 = new BookAddedToLibrary(processId, user4Id, ExtremeProgrammingExplained, KentBeck, Isbn);
            tddByKbAddTo3 = new BookAddedToLibrary(processId, user3Id, TestDrivenDevelopment, KentBeck, Isbn);
            essBySSAddTo5 = new BookAddedToLibrary(processId, user5Id, ExtremeSnowboardStunts, SomeSkiier, Isbn);
            bBySAAddTo6 = new BookAddedToLibrary(processId, user6Id, BeckAMusicalMaestro, SomeAuthor, Isbn);
        }

        #endregion


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

    }
}
