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
            Runner.RunScenario(
            given => UserRegisters(userId, "user1", "email1", "user1Picture"),
            when => OpenLibrary(transactionId, userId, "library1"),
            then => LibraryOpenedSuccessfully(),
            and => EventsSavedForAggregate<Library>(userId, new LibraryOpened(transactionId, userId, "library1", userId)));
        }

        [Test]
        public void UserCanOpenASecondLibrary()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            Runner.RunScenario(
            given => UserRegisters(userId, "user1", "email1", "user1Picture"),
            and => OpenLibrary(transactionId, userId, "library1"),

            when => OpenLibrary(transactionId, userId, "library1"),

            then => LibraryOpenedSuccessfully(),
            and => EventsSavedForAggregate<Library>(userId, 
                new LibraryOpened(transactionId, userId, "library1", userId),
                new LibraryOpened(transactionId, userId, "library1", userId)));
        }


    }
}
