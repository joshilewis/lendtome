using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
