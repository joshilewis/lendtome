using System;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.Model;
using Lending.Domain.OpenLibrary;
using Lending.Domain.RemoveBookFromLibrary;
using Lending.ReadModels.Relational.SearchForBook;
using NUnit.Framework;
using static Joshilewis.Testing.Helpers.ApiExtensions;
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
        public void AddingNewBookToLibraryShouldSucceed()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            Given(() => OpenLibrary(transactionId, userId, "library1"));
            When(() => AddBookToLibrary1(transactionId, userId, userId, "Title", "Author", "isbn", 1982));
            Then1(() => BookAddedSucccessfully());
            AndGETTo($"/libraries/{userId}/books/")
                .Returns(new BookSearchResult(userId, "library1", "Title", "Author", "isbn", 1982));
            AndEventsSavedForAggregate<Library>(userId, 
                new LibraryOpened(transactionId, userId, "library1", userId),
                new BookAddedToLibrary(transactionId, userId, "Title", "Author", "isbn", 1982));
        }

        [Test]
        public void AddingDuplicateBookToLibraryShouldFail()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            Given(() => OpenLibrary(transactionId, userId, "library1"));
            Given(() => AddBookToLibrary1(transactionId, userId, userId, "Title", "Author", "isbn", 1982));
            When(() => AddBookToLibrary1(transactionId, userId, userId, "Title", "Author", "isbn", 1982));
            Then1(() => DuplicateBookNotAdded());
            AndGETTo($"/libraries/{userId}/books/")
                .Returns(new BookSearchResult(userId, "library1", "Title", "Author", "isbn", 1982));
            AndEventsSavedForAggregate<Library>(userId,
                new LibraryOpened(transactionId, userId, "library1", userId),
                new BookAddedToLibrary(transactionId, userId, "Title", "Author", "isbn", 1982));
        }

        [Test]
        public void AddingPreviouslyRemovedBookToLibraryShouldSucceed()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            Given(() => OpenLibrary(transactionId, userId, "library1"));
            Given(() => AddBookToLibrary1(transactionId, userId, userId, "Title", "Author", "isbn", 1982));
            Given(() => RemoveBookFromLibrary(transactionId, userId, userId, "Title", "Author", "isbn", 1982));
            When(() => AddBookToLibrary1(transactionId, userId, userId, "Title", "Author", "isbn", 1982));
            Then1(() => BookAddedSucccessfully());
            AndGETTo($"/libraries/{userId}/books/")
                .Returns(new BookSearchResult(userId, "library1", "Title", "Author", "isbn", 1982));
            AndEventsSavedForAggregate<Library>(userId,
                new LibraryOpened(transactionId, userId, "library1", userId),
                new BookAddedToLibrary(transactionId, userId, "Title", "Author", "isbn", 1982),
                new BookRemovedFromLibrary(transactionId, userId, "Title", "Author", "isbn", 1982),
                new BookAddedToLibrary(transactionId, userId, "Title", "Author", "isbn", 1982)
            );
        }

        [Test]
        public void UnauthorizedAddBookAddBookShouldFail()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            Given(() => OpenLibrary(transactionId, userId, "library1"));
            When(() => AddBookToLibrary1(transactionId, userId, Guid.Empty, "Title", "Author", "isbn", 1982));
            Then1(() => UnauthorisedCommandRejected(Guid.Empty, typeof(Library), userId));
            AndGETTo($"/libraries/{userId}/books/").Returns(new BookSearchResult[] { });
            AndEventsSavedForAggregate<Library>(userId, 
                new LibraryOpened(transactionId, userId, "library1", userId));
        }

    }
}
