using System;
using Lending.Domain.Model;
using Lending.Domain.OpenLibrary;
using NUnit.Framework;
using static Joshilewis.Testing.Helpers.EventStoreExtensions;
using static Tests.AutomationExtensions;

namespace Tests.Commands
{
    [TestFixture]
    public class OpenLibrarySpecs : Fixture
    {
        [Test]
        public void UserCanOpenALibrary()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            When(() => OpenLibrary(transactionId, userId, "library1"));
            Then1(() => LibraryOpenedSuccessfully());
            AndEventsSavedForAggregate<Library>(userId, new LibraryOpened(transactionId, userId, "library1", userId));
        }

        [Test]
        public void UserCanOpenASecondLibrary()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            Given(() => OpenLibrary(transactionId, userId, "library1"));
            When(() => OpenLibrary(transactionId, userId, "library1"));
            Then1(() => LibraryOpenedSuccessfully());
            AndEventsSavedForAggregate<Library>(userId, 
                new LibraryOpened(transactionId, userId, "library1", userId),
                new LibraryOpened(transactionId, userId, "library1", userId));
        }


    }
}
