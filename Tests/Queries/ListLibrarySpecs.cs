using NUnit.Framework;
using System;
using Lending.ReadModels.Relational.ListLibrayLinks;
using static Joshilewis.Testing.Helpers.ApiExtensions;
using static Tests.AutomationExtensions;

namespace Tests.Queries
{
    [TestFixture]
    public class ListLibrarySpecs : Fixture
    {
        [Test]
        public void UserCanListOnlyLibrariesTheyAdminister()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            var user2Id = Guid.NewGuid();
            var user3Id = Guid.NewGuid();
            Runner.RunScenario(
                given => UserRegisters(userId, "user1", "email1", "user1Picture"),
                and => UserRegisters(user2Id, "user2", "email2", "user2Picture"),
                and => UserRegisters(user3Id, "user3", "email3", "user3Picture"),
                and => LibraryOpened(transactionId, userId, "library1"),
                and => LibraryOpened(transactionId, user2Id, "library2"),
                and => LibraryOpened(transactionId, user3Id, "library3"),

                when => GetEndpoint("/libraries/").As(user2Id),
                then => ResponseIs(new LibrarySearchResult(user2Id, "library2", "user2Picture")
                ));
            ;
        }
    }
}
