﻿using System;
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
            this.GivenCommand(OpenLibrary1).IsPOSTedTo("/libraries/");
            this.GivenCommand(OpenLibrary2).IsPOSTedTo("/libraries/");
            this.GivenCommand(Library1RequestsLinkToLibrary2).IsPOSTedTo($"/libraries/{Library1Id}/links/request/");
            this.GivenCommand(Library2AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library2Id}/links/accept/");
            this.WhenGetEndpoint("books/Extreme Programming Explained").As(Library1Id);
            this.ThenResponseIs(new BookSearchResult[] {});
        }

        /// <summary>
        /// GIVEN User1 has Registered
        /// WHEN User1 Searches for a Book with the Search Term "Extreme Programming Explained"
        /// THEN A failed result is returned, with reason 'User has no Connections'
        /// </summary>
        [Test]
        public void SearchingForBookWithNoConnectionsShouldFail()
        {
            this.GivenCommand(OpenLibrary1).IsPOSTedTo("/libraries/");
            this.WhenGetEndpoint("books/Extreme Programming Explained").As(Library1Id);
            this.ThenResponseIs(new BookSearchResult[] {});
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
            this.GivenCommands(OpenLibrary1, OpenLibrary2, OpenLibrary3, OpenLibrary4).ArePOSTedTo("/libraries/");
            this.GivenCommands(Library1RequestsLinkToLibrary2, Library1RequestsLinkToLibrary3, Library1RequestsLinkToLibrary4)
                .ArePOSTedTo($"/libraries/{Library1Id}/links/request");
            this.GivenCommand(Library2AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library2Id}/links/accept");
            this.GivenCommand(Library3AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library3Id}/links/accept");
            this.GivenCommand(Library4AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library4Id}/links/accept");
            this.GivenCommand(Lib2AddsXpeByKb).IsPOSTedTo($"/libraries/{Library2Id}/books/add/");
            this.GivenCommand(Lib3AddsTddByKb).IsPOSTedTo($"/libraries/{Library3Id}/books/add/");
            this.GivenCommand(Lib4AddsXpeByKb).IsPOSTedTo($"/libraries/{Library4Id}/books/add/");
            this.WhenGetEndpoint("books/Extreme Programming Explained").As(Library1Id);
            this.ThenResponseIs(
                new BookSearchResult(Library2Id, OpenLibrary2.Name, ExtremeProgrammingExplained, KentBeck, Isbn),
                new BookSearchResult(Library4Id, OpenLibrary4.Name, ExtremeProgrammingExplained, KentBeck, Isbn));

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
            this.GivenCommands(OpenLibrary1, OpenLibrary2, OpenLibrary3, OpenLibrary4, OpenLibrary5)
                .ArePOSTedTo("/libraries/");
            this.GivenCommands(Library1RequestsLinkToLibrary2, Library1RequestsLinkToLibrary3, Library1RequestsLinkToLibrary4,
                Library1RequestsLinkToLibrary5)
                .ArePOSTedTo($"/libraries/{Library1Id}/links/request");
            this.GivenCommand(Library2AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library2Id}/links/accept");
            this.GivenCommand(Library3AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library3Id}/links/accept");
            this.GivenCommand(Library4AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library4Id}/links/accept");
            this.GivenCommand(Library5AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library5Id}/links/accept");
            this.GivenCommand(Lib2AddsXpeByKb).IsPOSTedTo($"/libraries/{Library2Id}/books/add/");
            this.GivenCommand(Lib3AddsTddByKb).IsPOSTedTo($"/libraries/{Library3Id}/books/add/");
            this.GivenCommand(Lib4AddsXpeByKb).IsPOSTedTo($"/libraries/{Library4Id}/books/add/");
            this.GivenCommand(Lib5AddsEssBySs).IsPOSTedTo($"/libraries/{Library5Id}/books/add/");
            this.WhenGetEndpoint("books/Extreme").As(Library1Id);
            this.ThenResponseIs(
                new BookSearchResult(Library2Id, OpenLibrary2.Name, ExtremeProgrammingExplained, KentBeck, Isbn),
                new BookSearchResult(Library4Id, OpenLibrary4.Name, ExtremeProgrammingExplained, KentBeck, Isbn),
                new BookSearchResult(Library5Id, OpenLibrary5.Name, ExtremeSnowboardStunts, SomeSkiier, Isbn));

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
            this.GivenCommands(OpenLibrary1, OpenLibrary2, OpenLibrary3, OpenLibrary4, OpenLibrary5)
                .ArePOSTedTo("/libraries/");
            this.GivenCommands(Library1RequestsLinkToLibrary2, Library1RequestsLinkToLibrary3, Library1RequestsLinkToLibrary4,
                Library1RequestsLinkToLibrary5)
                .ArePOSTedTo($"/libraries/{Library1Id}/links/request");
            this.GivenCommand(Library2AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library2Id}/links/accept");
            this.GivenCommand(Library3AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library3Id}/links/accept");
            this.GivenCommand(Library4AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library4Id}/links/accept");
            this.GivenCommand(Library5AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library5Id}/links/accept");
            this.GivenCommand(Lib2AddsXpeByKb).IsPOSTedTo($"/libraries/{Library2Id}/books/add/");
            this.GivenCommand(Lib3AddsTddByKb).IsPOSTedTo($"/libraries/{Library3Id}/books/add/");
            this.GivenCommand(Lib4AddsXpeByKb).IsPOSTedTo($"/libraries/{Library4Id}/books/add/");
            this.GivenCommand(Lib5AddsEssBySs).IsPOSTedTo($"/libraries/{Library5Id}/books/add/");
            this.WhenGetEndpoint("books/Kent Beck").As(Library1Id);
            this.ThenResponseIs(
                new BookSearchResult(Library2Id, OpenLibrary2.Name, ExtremeProgrammingExplained, KentBeck, Isbn),
                new BookSearchResult(Library3Id, OpenLibrary3.Name, TestDrivenDevelopment, KentBeck, Isbn),
                new BookSearchResult(Library4Id, OpenLibrary4.Name, ExtremeProgrammingExplained, KentBeck, Isbn));
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
            this.GivenCommands(OpenLibrary1, OpenLibrary2, OpenLibrary3, OpenLibrary4, OpenLibrary5, OpenLibrary6)
                .ArePOSTedTo("/libraries/");
            this.GivenCommands(Library1RequestsLinkToLibrary2, Library1RequestsLinkToLibrary3, Library1RequestsLinkToLibrary4,
                Library1RequestsLinkToLibrary5, Library1RequestsLinkToLibrary6)
                .ArePOSTedTo($"/libraries/{Library1Id}/links/request");
            this.GivenCommand(Library2AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library2Id}/links/accept");
            this.GivenCommand(Library3AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library3Id}/links/accept");
            this.GivenCommand(Library4AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library4Id}/links/accept");
            this.GivenCommand(Library5AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library5Id}/links/accept");
            this.GivenCommand(Library6AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library6Id}/links/accept");
            this.GivenCommand(Lib2AddsXpeByKb).IsPOSTedTo($"/libraries/{Library2Id}/books/add/");
            this.GivenCommand(Lib3AddsTddByKb).IsPOSTedTo($"/libraries/{Library3Id}/books/add/");
            this.GivenCommand(Lib4AddsXpeByKb).IsPOSTedTo($"/libraries/{Library4Id}/books/add/");
            this.GivenCommand(Lib5AddsEssBySs).IsPOSTedTo($"/libraries/{Library5Id}/books/add/");
            this.GivenCommand(Lib6AddsBBySA).IsPOSTedTo($"/libraries/{Library6Id}/books/add/");
            this.WhenGetEndpoint("books/Beck").As(Library1Id);
            this.ThenResponseIs(
                new BookSearchResult(Library2Id, OpenLibrary2.Name, ExtremeProgrammingExplained, KentBeck, Isbn),
                new BookSearchResult(Library3Id, OpenLibrary3.Name, TestDrivenDevelopment, KentBeck, Isbn),
                new BookSearchResult(Library4Id, OpenLibrary4.Name, ExtremeProgrammingExplained, KentBeck, Isbn),
                new BookSearchResult(Library6Id, OpenLibrary6.Name, BeckAMusicalMaestro, SomeAuthor, Isbn));
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
            this.GivenCommands(OpenLibrary1, OpenLibrary2, OpenLibrary3, OpenLibrary4, OpenLibrary5, OpenLibrary6)
                .ArePOSTedTo("/libraries/");
            this.GivenCommands(Library1RequestsLinkToLibrary2, Library1RequestsLinkToLibrary3, Library1RequestsLinkToLibrary4,
                Library1RequestsLinkToLibrary5, Library1RequestsLinkToLibrary6)
                .ArePOSTedTo($"/libraries/{Library1Id}/links/request");
            this.GivenCommand(Library2AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library2Id}/links/accept");
            this.GivenCommand(Library3AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library3Id}/links/accept");
            this.GivenCommand(Library5AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library5Id}/links/accept");
            this.GivenCommand(Library6AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library6Id}/links/accept");
            this.GivenCommand(Lib2AddsXpeByKb).IsPOSTedTo($"/libraries/{Library2Id}/books/add/");
            this.GivenCommand(Lib3AddsTddByKb).IsPOSTedTo($"/libraries/{Library3Id}/books/add/");
            this.GivenCommand(Lib4AddsXpeByKb).IsPOSTedTo($"/libraries/{Library4Id}/books/add/");
            this.GivenCommand(Lib5AddsEssBySs).IsPOSTedTo($"/libraries/{Library5Id}/books/add/");
            this.GivenCommand(Lib6AddsBBySA).IsPOSTedTo($"/libraries/{Library6Id}/books/add/");
            this.WhenGetEndpoint("books/Beck").As(Library1Id);
            this.ThenResponseIs(
                new BookSearchResult(Library2Id, OpenLibrary2.Name, ExtremeProgrammingExplained, KentBeck, Isbn),
                new BookSearchResult(Library3Id, OpenLibrary3.Name, TestDrivenDevelopment, KentBeck, Isbn),
                new BookSearchResult(Library6Id, OpenLibrary6.Name, BeckAMusicalMaestro, SomeAuthor, Isbn));

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
        public void
            SearchingForBookWithManyMatchingTitlesAndAuthorsInManyLibrariesShouldReturnAllOwnersAndBooksExceptRemovedBooks
            ()
        {
            this.GivenCommands(OpenLibrary1, OpenLibrary2, OpenLibrary3, OpenLibrary4, OpenLibrary5, OpenLibrary6)
                .ArePOSTedTo("/libraries/");
            this.GivenCommands(Library1RequestsLinkToLibrary2, Library1RequestsLinkToLibrary3,
                Library1RequestsLinkToLibrary4,
                Library1RequestsLinkToLibrary5, Library1RequestsLinkToLibrary6)
                .ArePOSTedTo($"/libraries/{Library1Id}/links/request");
            this.GivenCommand(Library2AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library2Id}/links/accept");
            this.GivenCommand(Library3AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library3Id}/links/accept");
            this.GivenCommand(Library4AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library4Id}/links/accept");
            this.GivenCommand(Library5AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library5Id}/links/accept");
            this.GivenCommand(Library6AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library6Id}/links/accept");
            this.GivenCommand(Lib2AddsXpeByKb).IsPOSTedTo($"/libraries/{Library2Id}/books/add/");
            this.GivenCommand(Lib3AddsTddByKb).IsPOSTedTo($"/libraries/{Library3Id}/books/add/");
            this.GivenCommand(Lib4AddsXpeByKb).IsPOSTedTo($"/libraries/{Library4Id}/books/add/");
            this.GivenCommand(Lib5AddsEssBySs).IsPOSTedTo($"/libraries/{Library5Id}/books/add/");
            this.GivenCommand(Lib6AddsBBySA).IsPOSTedTo($"/libraries/{Library6Id}/books/add/");
            this.GivenCommand(Lib4RemovesXpeByKb).IsPOSTedTo($"/libraries/{Library4Id}/books/remove/");
            this.WhenGetEndpoint("books/Beck").As(Library1Id);
            this.ThenResponseIs(
                new BookSearchResult(Library2Id, OpenLibrary2.Name, ExtremeProgrammingExplained, KentBeck, Isbn),
                new BookSearchResult(Library3Id, OpenLibrary3.Name, TestDrivenDevelopment, KentBeck, Isbn),
                new BookSearchResult(Library6Id, OpenLibrary6.Name, BeckAMusicalMaestro, SomeAuthor, Isbn));

        }

    }
}
