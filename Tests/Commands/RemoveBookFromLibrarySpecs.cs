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

            var libraryId = OpenLibrary(transactionId, userId, "library1", "library1Picture");
            AddsBookToLibrary(transactionId, libraryId, userId, "Title", "Author", "isbn", 1982, "picture");

            RemovesBookFromLibrary(transactionId, libraryId, userId, "Title", "Author", "isbn", 1982, "picture");

            BookRemovedSucccessfully();
            EventsSavedForAggregate<Library>(libraryId,
                new LibraryOpened(transactionId, libraryId, "library1", userId, "library1Picture"),
                new BookAddedToLibrary(transactionId, libraryId, "Title", "Author", "isbn", 1982, "picture"),
                new BookRemovedFromLibrary(transactionId, libraryId, "Title", "Author", "isbn", 1982, "picture")
            );
        }

        [Test]
        public void RemovingBookNotInLibraryIgnored()
        {
            var transactionId = Guid.Empty;
            var userId = "userId";

            var libraryId = OpenLibrary(transactionId, userId, "library1", "library1Picture");

            RemovesBookFromLibrary(transactionId, libraryId, userId, "Title", "Author", "isbn", 1982, "picture");

            IgnoreBecauseBookNotInLibrary();
            EventsSavedForAggregate<Library>(libraryId,
                new LibraryOpened(transactionId, libraryId, "library1", userId, "library1Picture")
            );
        }

        [Test]
        public void UnauthorizedUserCantRemoveBook()
        {
            var transactionId = Guid.Empty;
            var userId = "userId";

            var libraryId = OpenLibrary(transactionId, userId, "library1", "library1Picture");

            RemovesBookFromLibrary(transactionId, libraryId, "Unauthorised user", "Title", "Author", "isbn",
                1982, "picture");

            UnauthorisedCommandIgnored("Unauthorised user", typeof(Library), libraryId);
            EventsSavedForAggregate<Library>(libraryId,
                new LibraryOpened(transactionId, libraryId, "library1", userId, "library1Picture")
            );
        }

    }
}
