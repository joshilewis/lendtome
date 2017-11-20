using System;
using Lending.ReadModels.Relational.SearchForBook;
using NUnit.Framework;
using static Joshilewis.Testing.Helpers.ApiExtensions;
using static Tests.AutomationExtensions;

namespace Tests.Queries
{
    public class SearchForBookTests : Fixture
    {
        private Guid transactionId;
        private string user1Id = "user1Id";
        private string user2Id = "user2Id";
        private string user3Id = "user3Id";
        private string user4Id = "user4Id";
        private string user5Id = "user5Id";
        private string user6Id = "user6Id";
        private Guid library1Id;
        private Guid library2Id;
        private Guid library3Id;
        private Guid library4Id;
        private Guid library5Id;
        private Guid library6Id;

        public override void SetUp()
        {
            base.SetUp();
            transactionId = Guid.Empty;
            library1Id = Guid.NewGuid();
            library2Id = Guid.NewGuid();
            library3Id = Guid.NewGuid();
            library4Id = Guid.NewGuid();
            library5Id = Guid.NewGuid();
            library6Id = Guid.NewGuid();
        }

        [Test]
        public void SearchingForBookNotOwnedByAnyConnection()
        {
            Runner.RunScenario(
            given => LibraryOpened(transactionId, user1Id, library1Id, "library1", "library1Picture"),
            and => LibraryOpened(transactionId, user2Id, library2Id, "library2", "library1Picture"),
            and => LinkRequested(transactionId, library1Id, library2Id),
            and => LinkAccepted(transactionId, library2Id, library1Id),

            when => GetEndpoint("books/Extreme Programming Explained").As(user1Id),
            then => ResponseIs(new BookSearchResult[] {}));
        }

        [Test]
        public void SearchingForBookWithNoLinks()
        {
            Runner.RunScenario(
            given => LibraryOpened(transactionId, user1Id, library1Id, "library1", "library1Picture"),

            when => GetEndpoint("books/Extreme Programming Explained").As(user1Id),
            then => ResponseIs(new BookSearchResult[] {}));
        }

        [Test]
        public void SearchingForBookWithSingleMatchingTitleInManyLibrariesShouldReturnAllOwners()
        {
            Runner.RunScenario(
            given => Libraries1To6Opened(),
            and => Library1RequestedLinksToOtherLibraries(),
            and => LibrariesAcceptedLinkToLinkToLibrary1(),

            and => BookAddedToLibrary(transactionId, library2Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, library3Id, "Test-Driven Development", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, library4Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),

            when => GetEndpoint("books/Extreme Programming Explained").As(user1Id),
            then => ResponseIs(
                new BookSearchResult(library2Id, "library2", "library2Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(library4Id, "library4", "library4Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000)));
        }

        [Test]
        public void SearchingForBookWithManyMatchingTitlesInManyLibrariesShouldReturnAllOwnersAndBooks()
        {
            Runner.RunScenario(
            given => Libraries1To6Opened(),
            and => Library1RequestedLinksToOtherLibraries(),
            and => LibrariesAcceptedLinkToLinkToLibrary1(),

            and => BookAddedToLibrary(transactionId, library2Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, library3Id, "Test-Driven Development", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, library4Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, library5Id, "Extreme Snowboard Stunts", "Some Skiier", "ISBN", 2000),

            when => GetEndpoint("books/Extreme").As(user1Id),
            then => ResponseIs(
                new BookSearchResult(library2Id, "library2", "library2Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(library4Id, "library4", "library4Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(library5Id, "library5", "library5Picture", "Extreme Snowboard Stunts", "Some Skiier", "ISBN", 2000)));
        }

        [Test]
        public void SearchingForBookWithSingleMatchingAuthorInManyLibrariesShouldReturnAllOwnersAndBooks()
        {
            Runner.RunScenario(
            given => Libraries1To6Opened(),
            and => Library1RequestedLinksToOtherLibraries(),
            and => LibrariesAcceptedLinkToLinkToLibrary1(),

            and => BookAddedToLibrary(transactionId, library2Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, library3Id, "Test-Driven Development", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, library4Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, library5Id, "Extreme Snowboard Stunts", "Some Skiier", "ISBN", 2000),

            when => GetEndpoint("books/Kent Beck").As(user1Id),
            then => ResponseIs(
                new BookSearchResult(library2Id, "library2", "library2Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(library3Id, "library3", "library3Picture", "Test-Driven Development", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(library4Id, "library4", "library4Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000)));
        }

        [Test]
        public void SearchingForBookWithManyMatchingTitlesAndAuthorsInManyLibrariesShouldReturnAllOwnersAndBooks()
        {
            Runner.RunScenario(
            given => Libraries1To6Opened(),
            and => Library1RequestedLinksToOtherLibraries(),
            and => LibrariesAcceptedLinkToLinkToLibrary1(),

            and => BookAddedToLibrary(transactionId, library2Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, library3Id, "Test-Driven Development", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, library4Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, library5Id, "Extreme Snowboard Stunts", "Some Skiier", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, library6Id, "Beck: A Musical Maestro", "Some Author", "ISBN", 2000),

            when => GetEndpoint("books/Beck").As(user1Id),
            then => ResponseIs(
                new BookSearchResult(library2Id, "library2", "library2Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(library3Id, "library3", "library3Picture", "Test-Driven Development", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(library4Id, "library4", "library4Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(library6Id, "library6", "library6Picture", "Beck: A Musical Maestro", "Some Author", "ISBN", 2000)
                ));
        }

        [Test]
        public void ExcludeUnlinkedLibrariesInSearch()
        {
            Runner.RunScenario(
            given => Libraries1To6Opened(),
            and => Library1RequestedLinksToOtherLibraries(),

            and => LinkAccepted(transactionId, library2Id, library1Id),
            and => LinkAccepted(transactionId, library3Id, library1Id),
            and => LinkAccepted(transactionId, library5Id, library1Id),
            and => LinkAccepted(transactionId, library6Id, library1Id),

            and => BookAddedToLibrary(transactionId, library2Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, library3Id, "Test-Driven Development", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, library4Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, library5Id, "Extreme Snowboard Stunts", "Some Skiier", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, library6Id, "Beck: A Musical Maestro", "Some Author", "ISBN", 2000),

            when => GetEndpoint("books/Beck").As(user1Id),
            then => ResponseIs(
                new BookSearchResult(library2Id, "library2", "library2Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(library3Id, "library3", "library3Picture", "Test-Driven Development", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(library6Id, "library6", "library6Picture", "Beck: A Musical Maestro", "Some Author", "ISBN", 2000)
                ));
        }

        [Test]
        public void ExcludeRemovedBooksFromSearchResults()
        {
            Runner.RunScenario(
            given => Libraries1To6Opened(),
            and => Library1RequestedLinksToOtherLibraries(),
            and => LibrariesAcceptedLinkToLinkToLibrary1(),

            and => BookAddedToLibrary(transactionId, library2Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, library3Id, "Test-Driven Development", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, library4Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, library5Id, "Extreme Snowboard Stunts", "Some Skiier", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, library6Id, "Beck: A Musical Maestro", "Some Author", "ISBN", 2000),
            and => BookRemovedFromLibrary(transactionId, library4Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),

            when => GetEndpoint("books/Beck").As(user1Id),
            then => ResponseIs(
                new BookSearchResult(library2Id, "library2", "library2Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(library3Id, "library3", "library3Picture", "Test-Driven Development", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(library6Id, "library6", "library6Picture", "Beck: A Musical Maestro", "Some Author", "ISBN", 2000)
                ));
        }

        private void Libraries1To6Opened()
        {
             LibraryOpened(transactionId, user1Id, library1Id, "library1", "library1Picture");
             LibraryOpened(transactionId, user2Id, library2Id, "library2", "library2Picture");
             LibraryOpened(transactionId, user3Id, library3Id, "library3", "library3Picture");
             LibraryOpened(transactionId, user4Id, library4Id, "library4", "library4Picture");
             LibraryOpened(transactionId, user5Id, library5Id, "library5", "library5Picture");
             LibraryOpened(transactionId, user6Id, library6Id, "library6", "library6Picture");
        }

        private void Library1RequestedLinksToOtherLibraries()
        {
             LinkRequested(transactionId, library1Id, library2Id);
             LinkRequested(transactionId, library1Id, library3Id);
             LinkRequested(transactionId, library1Id, library4Id);
             LinkRequested(transactionId, library1Id, library5Id);
             LinkRequested(transactionId, library1Id, library6Id);
        }

        private void LibrariesAcceptedLinkToLinkToLibrary1()
        {
             LinkAccepted(transactionId, library2Id, library1Id);
             LinkAccepted(transactionId, library3Id, library1Id);
             LinkAccepted(transactionId, library4Id, library1Id);
             LinkAccepted(transactionId, library5Id, library1Id);
             LinkAccepted(transactionId, library6Id, library1Id);
        }

    }
}
