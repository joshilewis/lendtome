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
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            Given(() => UserRegisters(user2Id, "user2", "email2", "user2Picture"));
            Given(() => LibraryOpened(transactionId, userId, "library1"));
            Given(() => LibraryOpened(transactionId, user2Id, "library2"));
            Given(() => LinkRequested(transactionId, userId, user2Id));
            Given(() => LinkAccepted(transactionId, user2Id, userId));

            WhenGetEndpoint("books/Extreme Programming Explained").As(userId);
            ThenResponseIs(new BookSearchResult[] {});
        }

        [Test]
        public void SearchingForBookWithNoLinks()
        {
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            Given(() => LibraryOpened(transactionId, userId, "library1"));
            WhenGetEndpoint("books/Extreme Programming Explained").As(userId);
            ThenResponseIs(new BookSearchResult[] {});
        }

        [Test]
        public void SearchingForBookWithSingleMatchingTitleInManyLibrariesShouldReturnAllOwners()
        {
            Given(() => Users1To6Registered());
            Given(() => Libraries1To6Opened());
            Given(() => Library1RequestedLinksToOtherLibraries());
            Given(() => LibrariesAcceptedLinkToLinkToLibrary1());

            Given(() => BookAddedToLibrary(transactionId, user2Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000));
            Given(() => BookAddedToLibrary(transactionId, user3Id, "Test-Driven Development", "Kent Beck", "ISBN", 2000));
            Given(() => BookAddedToLibrary(transactionId, user4Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000));

            WhenGetEndpoint("books/Extreme Programming Explained").As(userId);
            ThenResponseIs(
                new BookSearchResult(user2Id, "library2", "user2Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(user4Id, "library4", "user4Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000));

        }

        [Test]
        public void SearchingForBookWithManyMatchingTitlesInManyLibrariesShouldReturnAllOwnersAndBooks()
        {
            Given(() => Users1To6Registered());
            Given(() => Libraries1To6Opened());
            Given(() => Library1RequestedLinksToOtherLibraries());
            Given(() => LibrariesAcceptedLinkToLinkToLibrary1());

            Given(() => BookAddedToLibrary(transactionId, user2Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000));
            Given(() => BookAddedToLibrary(transactionId, user3Id, "Test-Driven Development", "Kent Beck", "ISBN", 2000));
            Given(() => BookAddedToLibrary(transactionId, user4Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000));
            Given(() => BookAddedToLibrary(transactionId, user5Id, "Extreme Snowboard Stunts", "Some Skiier", "ISBN", 2000));

            WhenGetEndpoint("books/Extreme").As(userId);
            ThenResponseIs(
                new BookSearchResult(user2Id, "library2", "user2Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(user4Id, "library4", "user4Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(user5Id, "library5", "user5Picture", "Extreme Snowboard Stunts", "Some Skiier", "ISBN", 2000));

        }

        [Test]
        public void SearchingForBookWithSingleMatchingAuthorInManyLibrariesShouldReturnAllOwnersAndBooks()
        {
            Given(() => Users1To6Registered());
            Given(() => Libraries1To6Opened());
            Given(() => Library1RequestedLinksToOtherLibraries());
            Given(() => LibrariesAcceptedLinkToLinkToLibrary1());

            Given(() => BookAddedToLibrary(transactionId, user2Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000));
            Given(() => BookAddedToLibrary(transactionId, user3Id, "Test-Driven Development", "Kent Beck", "ISBN", 2000));
            Given(() => BookAddedToLibrary(transactionId, user4Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000));
            Given(() => BookAddedToLibrary(transactionId, user5Id, "Extreme Snowboard Stunts", "Some Skiier", "ISBN", 2000));

            WhenGetEndpoint("books/Kent Beck").As(userId);
            ThenResponseIs(
                new BookSearchResult(user2Id, "library2", "user2Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(user3Id, "library3", "user3Picture", "Test-Driven Development", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(user4Id, "library4", "user4Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000));

        }

        [Test]
        public void SearchingForBookWithManyMatchingTitlesAndAuthorsInManyLibrariesShouldReturnAllOwnersAndBooks()
        {
            Given(() => Users1To6Registered());
            Given(() => Libraries1To6Opened());
            Given(() => Library1RequestedLinksToOtherLibraries());
            Given(() => LibrariesAcceptedLinkToLinkToLibrary1());

            Given(() => BookAddedToLibrary(transactionId, user2Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000));
            Given(() => BookAddedToLibrary(transactionId, user3Id, "Test-Driven Development", "Kent Beck", "ISBN", 2000));
            Given(() => BookAddedToLibrary(transactionId, user4Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000));
            Given(() => BookAddedToLibrary(transactionId, user5Id, "Extreme Snowboard Stunts", "Some Skiier", "ISBN", 2000));
            Given(() => BookAddedToLibrary(transactionId, user6Id, "Beck: A Musical Maestro", "Some Author", "ISBN", 2000));

            WhenGetEndpoint("books/Beck").As(userId);
            ThenResponseIs(
                new BookSearchResult(user2Id, "library2", "user2Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(user3Id, "library3", "user3Picture", "Test-Driven Development", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(user4Id, "library4", "user4Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(user6Id, "library6", "user6Picture", "Beck: A Musical Maestro", "Some Author", "ISBN", 2000)
                );

        }

        [Test]
        public void ExcludeUnlinkedLibrariesInSearch()
        {
            Given(() => Users1To6Registered());
            Given(() => Libraries1To6Opened());
            Given(() => Library1RequestedLinksToOtherLibraries());

            Given(() => LinkAccepted(transactionId, user2Id, userId));
            Given(() => LinkAccepted(transactionId, user3Id, userId));
            Given(() => LinkAccepted(transactionId, user5Id, userId));
            Given(() => LinkAccepted(transactionId, user6Id, userId));

            Given(() => BookAddedToLibrary(transactionId, user2Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000));
            Given(() => BookAddedToLibrary(transactionId, user3Id, "Test-Driven Development", "Kent Beck", "ISBN", 2000));
            Given(() => BookAddedToLibrary(transactionId, user4Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000));
            Given(() => BookAddedToLibrary(transactionId, user5Id, "Extreme Snowboard Stunts", "Some Skiier", "ISBN", 2000));
            Given(() => BookAddedToLibrary(transactionId, user6Id, "Beck: A Musical Maestro", "Some Author", "ISBN", 2000));

            WhenGetEndpoint("books/Beck").As(userId);
            ThenResponseIs(
                new BookSearchResult(user2Id, "library2", "user2Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(user3Id, "library3", "user3Picture", "Test-Driven Development", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(user6Id, "library6", "user6Picture", "Beck: A Musical Maestro", "Some Author", "ISBN", 2000)
                );

        }

        [Test]
        public void ExcludeRemovedBooksFromSearchResults()
        {
            Given(() => Users1To6Registered());
            Given(() => Libraries1To6Opened());
            Given(() => Library1RequestedLinksToOtherLibraries());
            Given(() => LibrariesAcceptedLinkToLinkToLibrary1());

            Given(() => BookAddedToLibrary(transactionId, user2Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000));
            Given(() => BookAddedToLibrary(transactionId, user3Id, "Test-Driven Development", "Kent Beck", "ISBN", 2000));
            Given(() => BookAddedToLibrary(transactionId, user4Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000));
            Given(() => BookAddedToLibrary(transactionId, user5Id, "Extreme Snowboard Stunts", "Some Skiier", "ISBN", 2000));
            Given(() => BookAddedToLibrary(transactionId, user6Id, "Beck: A Musical Maestro", "Some Author", "ISBN", 2000));
            Given(() => BookRemovedFromLibrary(transactionId, user4Id, "Extreme Programming Explained", "Kent Beck", "ISBN", 2000));

            WhenGetEndpoint("books/Beck").As(userId);
            ThenResponseIs(
                new BookSearchResult(user2Id, "library2", "user2Picture", "Extreme Programming Explained", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(user3Id, "library3", "user3Picture", "Test-Driven Development", "Kent Beck", "ISBN", 2000),
                new BookSearchResult(user6Id, "library6", "user6Picture", "Beck: A Musical Maestro", "Some Author", "ISBN", 2000)
                );

        }

        private void Users1To6Registered()
        {
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            Given(() => UserRegisters(user2Id, "user2", "email2", "user2Picture"));
            Given(() => UserRegisters(user3Id, "user3", "email3", "user3Picture"));
            Given(() => UserRegisters(user4Id, "user4", "email4", "user4Picture"));
            Given(() => UserRegisters(user5Id, "user5", "email5", "user5Picture"));
            Given(() => UserRegisters(user6Id, "user6", "email6", "user6Picture"));
        }

        private void Libraries1To6Opened()
        {
            Given(() => LibraryOpened(transactionId, userId, "library1"));
            Given(() => LibraryOpened(transactionId, user2Id, "library2"));
            Given(() => LibraryOpened(transactionId, user3Id, "library3"));
            Given(() => LibraryOpened(transactionId, user4Id, "library4"));
            Given(() => LibraryOpened(transactionId, user5Id, "library5"));
            Given(() => LibraryOpened(transactionId, user6Id, "library6"));
        }

        private void Library1RequestedLinksToOtherLibraries()
        {
            Given(() => LinkRequested(transactionId, userId, user2Id));
            Given(() => LinkRequested(transactionId, userId, user3Id));
            Given(() => LinkRequested(transactionId, userId, user4Id));
            Given(() => LinkRequested(transactionId, userId, user5Id));
            Given(() => LinkRequested(transactionId, userId, user6Id));
        }

        private void LibrariesAcceptedLinkToLinkToLibrary1()
        {
            Given(() => LinkAccepted(transactionId, user2Id, userId));
            Given(() => LinkAccepted(transactionId, user3Id, userId));
            Given(() => LinkAccepted(transactionId, user4Id, userId));
            Given(() => LinkAccepted(transactionId, user5Id, userId));
            Given(() => LinkAccepted(transactionId, user6Id, userId));
        }

    }
}
