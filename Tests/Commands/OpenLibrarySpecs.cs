using System;
using Lending.Domain.Model;
using Lending.Domain.OpenLibrary;
using Lending.ReadModels.Relational.ListLibrayLinks;
using NUnit.Framework;
using static Joshilewis.Testing.Helpers.ApiExtensions;
using static Joshilewis.Testing.Helpers.EventStoreExtensions;
using static Tests.AutomationExtensions;

namespace Tests.Commands
{
    [TestFixture]
    public class OpenLibrarySpecs : Fixture
    {
        [Test]
        public void OpenLibraryForUserWithNoLibrariesShouldOpenNewLibrary()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            When(() => OpenLibrary(transactionId, userId, "library1"));
            Then1(() => LibraryCreatedSuccessfully());
            AndGETTo("/libraries/").Returns(new LibrarySearchResult(userId, "library1", "user1Picture"));
            AndEventsSavedForAggregate<Library>(userId, new LibraryOpened(transactionId, userId, "library1", userId));
        }

        [Test]
        public void OpenLibraryForUserWithAnOpenLibraryShouldFailBecauseLibraryAlreadyOpened()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            Given(() => OpenLibrary(transactionId, userId, "library1"));
            When(() => OpenLibrary(transactionId, userId, "library1"));
            Then1(() => SecondLibraryNotCreated());
            AndGETTo("/libraries/").Returns(new LibrarySearchResult(userId, "library1", "user1Picture"));
            AndEventsSavedForAggregate<Library>(userId, new LibraryOpened(transactionId, userId, "library1", userId));
        }

        [Test]
        public void ListLibrariesShouldShowOnlyLibrariesAdministeredByUser()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            var user2Id = Guid.NewGuid();
            var user3Id = Guid.NewGuid();
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            Given(() => UserRegisters(user2Id, "user2", "email2", "user2Picture"));
            Given(() => UserRegisters(user3Id, "user3", "email3", "user3Picture"));
            Given(() => OpenLibrary(transactionId, userId, "library1"));
            Given(() => OpenLibrary(transactionId, user2Id, "library2"));
            Given(() => OpenLibrary(transactionId, user3Id, "library3"));
            WhenGetEndpoint("/libraries/").As(user2Id);
            ThenResponseIs(new LibrarySearchResult(user2Id, "library2", "user2Picture"));
        }

    }
}
