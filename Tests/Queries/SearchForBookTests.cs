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
        private Guid userId;
        private Guid user2Id;
        private Guid user3Id;
        private Guid user4Id;
        private Guid user5Id;
        private Guid user6Id;

        public override void SetUp()
        {
            base.SetUp();
            transactionId = Guid.Empty;
            userId = Guid.NewGuid();
            user2Id = Guid.NewGuid();
            user3Id = Guid.NewGuid();
            user4Id = Guid.NewGuid();
            user5Id = Guid.NewGuid();
            user6Id = Guid.NewGuid();
        }

        [Test]
        public void SearchingForBookNotOwnedByAnyConnection()
        {
            Runner.RunScenario(
            given => UserRegisters(userId, "user1", "email1", "user1Picture"),
            and => UserRegisters(user2Id, "user2", "email2", "user2Picture"),
            and => LibraryOpened(transactionId, userId, "library1"),
            and => LibraryOpened(transactionId, user2Id, "library2"),
            and => LinkRequested(transactionId, userId, user2Id),
            and => LinkAccepted(transactionId, user2Id, userId),

            when => GetEndpoint("books/Extreme Programming Explained").As(userId),
            then => ResponseIs(new BookSearchResult[] {}));
        }

        [Test]
        public void SearchingForBookWithNoLinks()
        {
            Runner.RunScenario(
            given => UserRegisters(userId, "user1", "email1", "user1Picture"),
            and => LibraryOpened(transactionId, userId, "library1"),

            when => GetEndpoint("books/Extreme Programming Explained").As(userId),
            then => ResponseIs(new BookSearchResult[] {}));
        }

        [Test]
        public void SearchingForBookWithSingleMatchingTitleInManyLibrariesShouldReturnAllOwners()
        {
            Runner.RunScenario(
            given => Users1To6Registered(),
            and => Libraries1To6Opened(),
            and => Library1RequestedLinksToOtherLibraries(),
            and => LibrariesAcceptedLinkToLinkToLibrary1(),

            and => BookAddedToLibrary(transactionId, user2Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, user3Id, "Test-Driven Development", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, user4Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),

            when => GetEndpoint("books/Extreme Programming Explained").As(userId),
            then => ResponseIs(
                new BookSearchResult(user2Id, "library2", "user2Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(user4Id, "library4", "user4Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000)));
        }

        [Test]
        public void SearchingForBookWithManyMatchingTitlesInManyLibrariesShouldReturnAllOwnersAndBooks()
        {
            Runner.RunScenario(
            given => Users1To6Registered(),
            and => Libraries1To6Opened(),
            and => Library1RequestedLinksToOtherLibraries(),
            and => LibrariesAcceptedLinkToLinkToLibrary1(),

            and => BookAddedToLibrary(transactionId, user2Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, user3Id, "Test-Driven Development", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, user4Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, user5Id, "Extreme Snowboard Stunts", "Some Skiier", "ISBN", 2000),

            when => GetEndpoint("books/Extreme").As(userId),
            then => ResponseIs(
                new BookSearchResult(user2Id, "library2", "user2Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(user4Id, "library4", "user4Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(user5Id, "library5", "user5Picture", "Extreme Snowboard Stunts", "Some Skiier", "ISBN", 2000)));
        }

        [Test]
        public void SearchingForBookWithSingleMatchingAuthorInManyLibrariesShouldReturnAllOwnersAndBooks()
        {
            Runner.RunScenario(
            given => Users1To6Registered(),
            and => Libraries1To6Opened(),
            and => Library1RequestedLinksToOtherLibraries(),
            and => LibrariesAcceptedLinkToLinkToLibrary1(),

            and => BookAddedToLibrary(transactionId, user2Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, user3Id, "Test-Driven Development", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, user4Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, user5Id, "Extreme Snowboard Stunts", "Some Skiier", "ISBN", 2000),

            when => GetEndpoint("books/Kent Beck").As(userId),
            then => ResponseIs(
                new BookSearchResult(user2Id, "library2", "user2Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(user3Id, "library3", "user3Picture", "Test-Driven Development", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(user4Id, "library4", "user4Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000)));
        }

        [Test]
        public void SearchingForBookWithManyMatchingTitlesAndAuthorsInManyLibrariesShouldReturnAllOwnersAndBooks()
        {
            Runner.RunScenario(
            given => Users1To6Registered(),
            and => Libraries1To6Opened(),
            and => Library1RequestedLinksToOtherLibraries(),
            and => LibrariesAcceptedLinkToLinkToLibrary1(),

            and => BookAddedToLibrary(transactionId, user2Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, user3Id, "Test-Driven Development", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, user4Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, user5Id, "Extreme Snowboard Stunts", "Some Skiier", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, user6Id, "Beck: A Musical Maestro", "Some Author", "ISBN", 2000),

            when => GetEndpoint("books/Beck").As(userId),
            then => ResponseIs(
                new BookSearchResult(user2Id, "library2", "user2Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(user3Id, "library3", "user3Picture", "Test-Driven Development", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(user4Id, "library4", "user4Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(user6Id, "library6", "user6Picture", "Beck: A Musical Maestro", "Some Author", "ISBN", 2000)
                ));
        }

        [Test]
        public void ExcludeUnlinkedLibrariesInSearch()
        {
            Runner.RunScenario(
            given => Users1To6Registered(),
            and => Libraries1To6Opened(),
            and => Library1RequestedLinksToOtherLibraries(),

            and => LinkAccepted(transactionId, user2Id, userId),
            and => LinkAccepted(transactionId, user3Id, userId),
            and => LinkAccepted(transactionId, user5Id, userId),
            and => LinkAccepted(transactionId, user6Id, userId),

            and => BookAddedToLibrary(transactionId, user2Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, user3Id, "Test-Driven Development", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, user4Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, user5Id, "Extreme Snowboard Stunts", "Some Skiier", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, user6Id, "Beck: A Musical Maestro", "Some Author", "ISBN", 2000),

            when => GetEndpoint("books/Beck").As(userId),
            then => ResponseIs(
                new BookSearchResult(user2Id, "library2", "user2Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(user3Id, "library3", "user3Picture", "Test-Driven Development", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(user6Id, "library6", "user6Picture", "Beck: A Musical Maestro", "Some Author", "ISBN", 2000)
                ));
        }

        [Test]
        public void ExcludeRemovedBooksFromSearchResults()
        {
            Runner.RunScenario(
            given => Users1To6Registered(),
            and => Libraries1To6Opened(),
            and => Library1RequestedLinksToOtherLibraries(),
            and => LibrariesAcceptedLinkToLinkToLibrary1(),

            and => BookAddedToLibrary(transactionId, user2Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, user3Id, "Test-Driven Development", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, user4Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, user5Id, "Extreme Snowboard Stunts", "Some Skiier", "ISBN", 2000),
            and => BookAddedToLibrary(transactionId, user6Id, "Beck: A Musical Maestro", "Some Author", "ISBN", 2000),
            and => BookRemovedFromLibrary(transactionId, user4Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),

            when => GetEndpoint("books/Beck").As(userId),
            then => ResponseIs(
                new BookSearchResult(user2Id, "library2", "user2Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(user3Id, "library3", "user3Picture", "Test-Driven Development", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(user6Id, "library6", "user6Picture", "Beck: A Musical Maestro", "Some Author", "ISBN", 2000)
                ));
        }

        private void Users1To6Registered()
        {
            UserRegisters(userId, "user1", "email1", "user1Picture");
            UserRegisters(user2Id, "user2", "email2", "user2Picture");
            UserRegisters(user3Id, "user3", "email3", "user3Picture");
            UserRegisters(user4Id, "user4", "email4", "user4Picture");
            UserRegisters(user5Id, "user5", "email5", "user5Picture");
            UserRegisters(user6Id, "user6", "email6", "user6Picture");
        }

        private void Libraries1To6Opened()
        {
             LibraryOpened(transactionId, userId, "library1");
             LibraryOpened(transactionId, user2Id, "library2");
             LibraryOpened(transactionId, user3Id, "library3");
             LibraryOpened(transactionId, user4Id, "library4");
             LibraryOpened(transactionId, user5Id, "library5");
             LibraryOpened(transactionId, user6Id, "library6");
        }

        private void Library1RequestedLinksToOtherLibraries()
        {
             LinkRequested(transactionId, userId, user2Id);
             LinkRequested(transactionId, userId, user3Id);
             LinkRequested(transactionId, userId, user4Id);
             LinkRequested(transactionId, userId, user5Id);
             LinkRequested(transactionId, userId, user6Id);
        }

        private void LibrariesAcceptedLinkToLinkToLibrary1()
        {
             LinkAccepted(transactionId, user2Id, userId);
             LinkAccepted(transactionId, user3Id, userId);
             LinkAccepted(transactionId, user4Id, userId);
             LinkAccepted(transactionId, user5Id, userId);
             LinkAccepted(transactionId, user6Id, userId);
        }

    }
}
