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
    [TestFixture]
    public class RemoveBookFromLibrarySpecs : Fixture
    {
        [Test]
        public void CanRemoveABookFromLibrary()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            Given(() => OpenLibrary(transactionId, userId, "library1"));
            Given(() => AddBookToLibrary1(transactionId, userId, userId, "Title", "Author", "isbn", 1982));
            When(() => RemoveBookFromLibrary(transactionId, userId, userId, "Title", "Author", "isbn", 1982));
            Then1(() => BookRemovedSucccessfully());
            AndGETTo($"/libraries/{userId}/books/").Returns(new BookSearchResult[] { });
            AndEventsSavedForAggregate<Library>(userId,
                new LibraryOpened(transactionId, userId, "library1", userId),
                new BookAddedToLibrary(transactionId, userId, "Title", "Author", "isbn", 1982),
                new BookRemovedFromLibrary(transactionId, userId, "Title", "Author", "isbn", 1982)
            );
        }

        [Test]
        public void CantRemoveABookNotInLibrary()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            Given(() => OpenLibrary(transactionId, userId, "library1"));
            When(() => RemoveBookFromLibrary(transactionId, userId, userId, "Title", "Author", "isbn", 1982));
            Then1(() => BookNotInLibrary());
            AndGETTo($"/libraries/{userId}/books/").Returns(new BookSearchResult[] { });
            AndEventsSavedForAggregate<Library>(userId,
                new LibraryOpened(transactionId, userId, "library1", userId)
            );
        }

        [Test]
        public void UnauthorizedUserCantRemoveBook()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            Given(() => OpenLibrary(transactionId, userId, "library1"));
            When(() => RemoveBookFromLibrary(transactionId, userId, Guid.Empty, "Title", "Author", "isbn", 1982));
            Then1(() => UnauthorisedCommandIgnored(Guid.Empty, typeof(Library), userId));
            AndGETTo($"/libraries/{userId}/books/").Returns(new BookSearchResult[] { });
            AndEventsSavedForAggregate<Library>(userId,
                new LibraryOpened(transactionId, userId, "library1", userId)
            );
        }

    }
}
