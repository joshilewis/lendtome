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
    /// <summary>
    /// https://github.com/joshilewis/lending/issues/9
    /// As a User I want to Add Books to my Library so that my Books can be searched by Linked Libraries
    /// </summary>
    public class AddBookToLibrarySpecs : Fixture
    {
        [Test]
        public void BookCanBeAddedToLibrary()
        {
            var transactionId = Guid.Empty;
            var userId = "userId";

            var libraryId = OpenLibrary(transactionId, userId, "library1", "library1Picture");

            AddsBookToLibrary(transactionId, libraryId, userId, "Title", "Author", "isbn", 1982);

            BookAddedSucccessfully();
            EventsSavedForAggregate<Library>(libraryId,
                new LibraryOpened(transactionId, libraryId, "library1", userId, "library1Picture"),
                new BookAddedToLibrary(transactionId, libraryId, "Title", "Author", "isbn", 1982));
        }

        [Test]
        public void AddingDuplicateBookIsIgnored()
        {
            var transactionId = Guid.Empty;
            var userId = "userId";

            var libraryId = OpenLibrary(transactionId, userId, "library1", "library1Picture");
            AddsBookToLibrary(transactionId, libraryId, userId, "Title", "Author", "isbn", 1982);

            AddsBookToLibrary(transactionId, libraryId, userId, "Title", "Author", "isbn", 1982);

            BookAddedSucccessfully();
            EventsSavedForAggregate<Library>(libraryId,
                new LibraryOpened(transactionId, libraryId, "library1", userId, "library1Picture"),
                new BookAddedToLibrary(transactionId, libraryId, "Title", "Author", "isbn", 1982)
            );
        }

        [Test]
        public void CanAddARemovedBook()
        {
            var transactionId = Guid.Empty;
            var userId = "userId";

            var libraryId = OpenLibrary(transactionId, userId, "library1", "library1Picture");
            AddsBookToLibrary(transactionId, libraryId, userId, "Title", "Author", "isbn", 1982);
            RemovesBookFromLibrary(transactionId, libraryId, userId, "Title", "Author", "isbn", 1982);

            AddsBookToLibrary(transactionId, libraryId, userId, "Title", "Author", "isbn", 1982);

            BookAddedSucccessfully();
            EventsSavedForAggregate<Library>(libraryId,
                new LibraryOpened(transactionId, libraryId, "library1", userId, "library1Picture"),
                new BookAddedToLibrary(transactionId, libraryId, "Title", "Author", "isbn", 1982),
                new BookRemovedFromLibrary(transactionId, libraryId, "Title", "Author", "isbn", 1982),
                new BookAddedToLibrary(transactionId, libraryId, "Title", "Author", "isbn", 1982)
            );
        }

        [Test]
        public void UnauthorizedUserCantAddBook()
        {
            var transactionId = Guid.Empty;
            var userId = "userId";

            var libraryId = OpenLibrary(transactionId, userId, "library1", "library1Picture");

            AddsBookToLibrary(transactionId, libraryId, "Unauthorised user", "Title", "Author", "isbn", 1982);

            UnauthorisedCommandIgnored("Unauthorised user", typeof(Library), libraryId);
            EventsSavedForAggregate<Library>(libraryId,
                new LibraryOpened(transactionId, libraryId, "library1", userId, "library1Picture"));
        }

    }
}
