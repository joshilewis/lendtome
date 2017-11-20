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
            var user1Id = "user1";
            var user2Id = "user2";
            var user3Id = "user3";
            var library1Id = Guid.NewGuid();
            var library2Id = Guid.NewGuid();
            var library3Id = Guid.NewGuid();
            Runner.RunScenario(
                given => UserRegisters(user1Id, "user1", "email1", "user1Picture"),
                and => UserRegisters(user2Id, "user2", "email2", "user2Picture"),
                and => UserRegisters(user3Id, "user3", "email3", "user3Picture"),
                and => LibraryOpened(transactionId, user1Id, library1Id, "library1"),
                and => LibraryOpened(transactionId, user2Id, library2Id, "library2"),
                and => LibraryOpened(transactionId, user3Id, library3Id, "library3"),

                when => GetEndpoint("/libraries/").As(user2Id),
                then => ResponseIs(new LibrarySearchResult(library2Id, "library2", "user2Picture")
                ));
            ;
        }
    }
}
