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
            var userId = Guid.NewGuid();
            Runner.RunScenario(
                given => UserRegisters(userId, "user1", "email1", "user1Picture"),
                and => OpensLibrary(transactionId, userId, "library1"),
                and => AddsBookToLibrary(transactionId, userId, userId, "Title", "Author", "isbn", 1982),

                when => RemovesBookFromLibrary(transactionId, userId, userId, "Title", "Author", "isbn", 1982),

                then => BookRemovedSucccessfully(),
                and => EventsSavedForAggregate<Library>(userId,
                    new LibraryOpened(transactionId, userId, "library1", userId),
                    new BookAddedToLibrary(transactionId, userId, "Title", "Author", "isbn", 1982),
                    new BookRemovedFromLibrary(transactionId, userId, "Title", "Author", "isbn", 1982)
                ));
        }

        [Test]
        public void RemovingBookNotInLibraryIgnored()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            Runner.RunScenario(
                given => UserRegisters(userId, "user1", "email1", "user1Picture"),
                and => OpensLibrary(transactionId, userId, "library1"),

                when => RemovesBookFromLibrary(transactionId, userId, userId, "Title", "Author", "isbn", 1982),

                then => IgnoreBecauseBookNotInLibrary(),
                and => EventsSavedForAggregate<Library>(userId,
                    new LibraryOpened(transactionId, userId, "library1", userId)
                ));
        }

        [Test]
        public void UnauthorizedUserCantRemoveBook()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            Runner.RunScenario(
                given => UserRegisters(userId, "user1", "email1", "user1Picture"),
                and => OpensLibrary(transactionId, userId, "library1"),

                when => RemovesBookFromLibrary(transactionId, userId, Guid.Empty, "Title", "Author", "isbn", 1982),

                then => UnauthorisedCommandIgnored(Guid.Empty, typeof(Library), userId),
                and => EventsSavedForAggregate<Library>(userId,
                    new LibraryOpened(transactionId, userId, "library1", userId)
                ));
        }

    }
}
