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
            var userId = Guid.NewGuid();
            Runner.RunScenario(
                given => UserRegisters(userId, "user1", "email1", "user1Picture"),
                and => OpenLibrary(transactionId, userId, "library1"),

                when => AddBookToLibrary1(transactionId, userId, userId, "Title", "Author", "isbn", 1982),

                then => BookAddedSucccessfully(),
                and => EventsSavedForAggregate<Library>(userId,
                    new LibraryOpened(transactionId, userId, "library1", userId),
                    new BookAddedToLibrary(transactionId, userId, "Title", "Author", "isbn", 1982)));
        }

        [Test]
        public void AddingDuplicateBookIsIgnored()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            Runner.RunScenario(
                given => UserRegisters(userId, "user1", "email1", "user1Picture"),
                and => OpenLibrary(transactionId, userId, "library1"),
                and => AddBookToLibrary1(transactionId, userId, userId, "Title", "Author", "isbn", 1982),

                when => AddBookToLibrary1(transactionId, userId, userId, "Title", "Author", "isbn", 1982),

                then => BookAddedSucccessfully(),
                and => EventsSavedForAggregate<Library>(userId,
                    new LibraryOpened(transactionId, userId, "library1", userId),
                    new BookAddedToLibrary(transactionId, userId, "Title", "Author", "isbn", 1982)
                ));
        }

        [Test]
        public void CanAddARemovedBook()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            Runner.RunScenario(
                given => UserRegisters(userId, "user1", "email1", "user1Picture"),
                and => OpenLibrary(transactionId, userId, "library1"),
                and => AddBookToLibrary1(transactionId, userId, userId, "Title", "Author", "isbn", 1982),
                and => RemoveBookFromLibrary(transactionId, userId, userId, "Title", "Author", "isbn", 1982),

                when => AddBookToLibrary1(transactionId, userId, userId, "Title", "Author", "isbn", 1982),

                then => BookAddedSucccessfully(),
                and => EventsSavedForAggregate<Library>(userId,
                    new LibraryOpened(transactionId, userId, "library1", userId),
                    new BookAddedToLibrary(transactionId, userId, "Title", "Author", "isbn", 1982),
                    new BookRemovedFromLibrary(transactionId, userId, "Title", "Author", "isbn", 1982),
                    new BookAddedToLibrary(transactionId, userId, "Title", "Author", "isbn", 1982)
                ));
        }

        [Test]
        public void UnauthorizedUserCantAddBook()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            Runner.RunScenario(
                given => UserRegisters(userId, "user1", "email1", "user1Picture"),
                and => OpenLibrary(transactionId, userId, "library1"),

                when => AddBookToLibrary1(transactionId, userId, Guid.Empty, "Title", "Author", "isbn", 1982),

                then => UnauthorisedCommandIgnored(Guid.Empty, typeof(Library), userId),
                and => EventsSavedForAggregate<Library>(userId,
                    new LibraryOpened(transactionId, userId, "library1", userId)));
        }

    }
}
