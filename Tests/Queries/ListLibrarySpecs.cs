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
                given => LibraryOpened(transactionId, user1Id, library1Id, "library1", "library1Picture"),
                and => LibraryOpened(transactionId, user2Id, library2Id, "library2", "library2Picture"),
                and => LibraryOpened(transactionId, user3Id, library3Id, "library3", "library3Picture"),

                when => GetEndpoint("/libraries/").As(user2Id),
                then => ResponseIs(new LibrarySearchResult(library2Id, "library2", "library2Picture")
                ));
        }

        [Test]
        public void SecondOpenedLibraryWillBeIgnored()
        {
            var transactionId = Guid.Empty;
            var user1Id = "user1";
            var library1Id = Guid.NewGuid();
            Runner.RunScenario(
                given => LibraryOpened(transactionId, user1Id, library1Id, "library1", "library1Picture"),

                when => LibraryOpened(transactionId, user1Id, Guid.NewGuid(), "library1", "library1Picture"),
                and => GetEndpoint("/libraries/").As(user1Id),

                then => ResponseIs(new LibrarySearchResult(library1Id, "library1", "library1Picture")
                ));
        }
    }
}
