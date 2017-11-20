using System;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.Model;
using Lending.Domain.OpenLibrary;
using Lending.Domain.RemoveBookFromLibrary;
using NUnit.Framework;
using static Joshilewis.Testing.Helpers.EventStoreExtensions;
using static Tests.AutomationExtensions;

namespace Tests.Commands
{
    [TestFixture]
    public class RemoveBookFromLibrarySpecs : Fixture
    {
        [Test]
        public void CanRemoveABookFromLibrary()
        {
            var transactionId = Guid.Empty;
            var userId = "userId";

            UserRegisters(userId, "user1", "email1", "user1Picture");
            var libraryId = OpenLibrary(transactionId, userId, "library1");
            AddsBookToLibrary(transactionId, libraryId, userId, "Title", "Author", "isbn", 1982);

            RemovesBookFromLibrary(transactionId, libraryId, userId, "Title", "Author", "isbn", 1982);

            BookRemovedSucccessfully();
            EventsSavedForAggregate<Library>(libraryId,
                new LibraryOpened(transactionId, libraryId, "library1", userId),
                new BookAddedToLibrary(transactionId, libraryId, "Title", "Author", "isbn", 1982),
                new BookRemovedFromLibrary(transactionId, libraryId, "Title", "Author", "isbn", 1982)
            );
        }

        [Test]
        public void RemovingBookNotInLibraryIgnored()
        {
            var transactionId = Guid.Empty;
            var userId = "userId";

            UserRegisters(userId, "user1", "email1", "user1Picture");
            var libraryId = OpenLibrary(transactionId, userId, "library1");

            RemovesBookFromLibrary(transactionId, libraryId, userId, "Title", "Author", "isbn", 1982);

            IgnoreBecauseBookNotInLibrary();
            EventsSavedForAggregate<Library>(libraryId,
                new LibraryOpened(transactionId, libraryId, "library1", userId)
            );
        }

        [Test]
        public void UnauthorizedUserCantRemoveBook()
        {
            var transactionId = Guid.Empty;
            var userId = "userId";

            UserRegisters(userId, "user1", "email1", "user1Picture");
            var libraryId = OpenLibrary(transactionId, userId, "library1");

            RemovesBookFromLibrary(transactionId, libraryId, "Unauthorised user", "Title", "Author", "isbn",
                1982);

            UnauthorisedCommandIgnored("Unauthorised user", typeof(Library), libraryId);
            EventsSavedForAggregate<Library>(libraryId,
                new LibraryOpened(transactionId, libraryId, "library1", userId)
            );
        }

    }
}
