using System;
using System.Net;
using Joshilewis.Testing.Helpers;
using Lending.Domain.AcceptLink;
using Lending.Domain.Model;
using Lending.Domain.OpenLibrary;
using Lending.Domain.RequestLink;
using Lending.ReadModels.Relational.LinkAccepted;
using Lending.ReadModels.Relational.LinkRequested;
using Lending.ReadModels.Relational.ListLibrayLinks;
using NUnit.Framework;
using static Tests.TestData;
using static Joshilewis.Testing.Helpers.ApiExtensions;
using static Joshilewis.Testing.Helpers.EventStoreExtensions;
using static Tests.AutomationExtensions;

namespace Tests.Commands
{
    /// <summary>
    /// https://github.com/joshilewis/lending/issues/6
    /// </summary>
    [TestFixture]
    public class RequestLinkSpecs : Fixture
    {
        private readonly LibrarySearchResult library1 = new LibrarySearchResult(Library1Id, Library1Name, Library1Picture);

        [Test]
        public void RequestLinkForUnlinkedLibrarysShouldSucceed()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            var user2Id = Guid.NewGuid();
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            Given(() => UserRegisters(user2Id, "user2", "email2", "user2Picture"));
            Given(() => OpenLibrary(transactionId, userId, "library1"));
            Given(() => OpenLibrary(transactionId, user2Id, "library2"));
            When(() => RequestLibraryLink(transactionId, userId, userId, user2Id));
            Then1(() => LinkRequestCreated());
            AndGETTo($"/libraries/{userId}/links/sent").As(userId).Returns(new LibrarySearchResult(user2Id, "library2", "user2Picture"));
            AndGETTo($"/libraries/{userId}/links/received").As(user2Id).Returns(new LibrarySearchResult(userId, "library1", "user1Picture"));
            AndGETTo($"/libraries/{userId}/links/").As(userId).Returns(new LibrarySearchResult[] {});
            AndGETTo($"/libraries/{userId}/links/").As(user2Id).Returns(new LibrarySearchResult[] { });
            AndEventsSavedForAggregate<Library>(userId, 
                new LibraryOpened(transactionId, userId, "library1", userId), 
                new LinkRequested(transactionId, userId, user2Id));
            AndEventsSavedForAggregate<Library>(user2Id, 
                new LibraryOpened(transactionId, user2Id, "library2", user2Id), 
                new LinkRequestReceived(transactionId, user2Id, userId));
        }

        [Test]
        public void DuplicateLinkRequestShouldBeIgnored()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            var user2Id = Guid.NewGuid();
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            Given(() => UserRegisters(user2Id, "user2", "email2", "user2Picture"));
            Given(() => OpenLibrary(transactionId, userId, "library1"));
            Given(() => OpenLibrary(transactionId, user2Id, "library2"));
            Given(() => RequestLibraryLink(transactionId, userId, userId, user2Id));
            When(() => RequestLibraryLink(transactionId, userId, userId, user2Id));
            Then1(() => DuplicateLinkRequestIgnored());
            AndGETTo($"/libraries/{userId}/links/sent").As(userId).Returns(new LibrarySearchResult(user2Id, "library2", "user2Picture"));
            AndGETTo($"/libraries/{userId}/links/received").As(user2Id).Returns(new LibrarySearchResult(userId, "library1", "user1Picture"));
            AndGETTo($"/libraries/{userId}/links/").As(userId).Returns(new LibrarySearchResult[] { });
            AndGETTo($"/libraries/{userId}/links/").As(user2Id).Returns(new LibrarySearchResult[] { });
            AndEventsSavedForAggregate<Library>(userId,
                new LibraryOpened(transactionId, userId, "library1", userId),
                new LinkRequested(transactionId, userId, user2Id));
            AndEventsSavedForAggregate<Library>(user2Id,
                new LibraryOpened(transactionId, user2Id, "library2", user2Id),
                new LinkRequestReceived(transactionId, user2Id, userId));
        }

        [Test]
        public void RequestLinkToNonExistentLibraryShouldFail()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            var user2Id = Guid.NewGuid();
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            Given(() => OpenLibrary(transactionId, userId, "library1"));
            When(() => RequestLibraryLink(transactionId, userId, userId, user2Id));
            Then1(() => FailtNotFound());
            AndGETTo($"/libraries/{userId}/links/sent").As(userId).Returns(new LibrarySearchResult[] {});
            AndGETTo($"/libraries/{userId}/links/").As(userId).Returns(new LibrarySearchResult[] { });
            AndEventsSavedForAggregate<Library>(userId,
                new LibraryOpened(transactionId, userId, "library1", userId));
        }

        [Test]
        public void RequestLinkToLibraryWithPendingRequestShouldFail()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            var user2Id = Guid.NewGuid();
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            Given(() => UserRegisters(user2Id, "user2", "email2", "user2Picture"));
            Given(() => OpenLibrary(transactionId, userId, "library1"));
            Given(() => OpenLibrary(transactionId, user2Id, "library2"));
            Given(() => RequestLibraryLink(transactionId, user2Id, user2Id, userId));
            When(() => RequestLibraryLink(transactionId, userId, userId, user2Id));
            Then1(() => ReverseLinkRequestIgnored());
            AndGETTo($"/libraries/{userId}/links/received").As(userId).Returns(new LibrarySearchResult(user2Id, "library2", "user2Picture"));
            AndGETTo($"/libraries/{userId}/links/sent").As(user2Id).Returns(new LibrarySearchResult(userId, "library1", "user1Picture"));
            AndGETTo($"/libraries/{userId}/links/").As(userId).Returns(new LibrarySearchResult[] { });
            AndGETTo($"/libraries/{userId}/links/").As(user2Id).Returns(new LibrarySearchResult[] { });
            AndEventsSavedForAggregate<Library>(userId,
                new LibraryOpened(transactionId, userId, "library1", userId),
                new LinkRequestReceived(transactionId, userId, user2Id));
            AndEventsSavedForAggregate<Library>(user2Id,
                new LibraryOpened(transactionId, user2Id, "library2", user2Id),
                new LinkRequested(transactionId, user2Id, userId));
        }

        [Test]
        public void RequestLinkToLinkedLibrariesShouldBeIgnored()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            var user2Id = Guid.NewGuid();
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            Given(() => UserRegisters(user2Id, "user2", "email2", "user2Picture"));
            Given(() => OpenLibrary(transactionId, userId, "library1"));
            Given(() => OpenLibrary(transactionId, user2Id, "library2"));
            Given(() => RequestLibraryLink(transactionId, userId, userId, user2Id));
            Given(() => AcceptLibraryLink(transactionId, user2Id, user2Id, userId));
            When(() => RequestLibraryLink(transactionId, userId, userId, user2Id));
            Then1(() => LinkRequestForLinkedLibrariesIgnored());
            AndGETTo($"/libraries/{userId}/links/sent").As(userId).Returns(new LibrarySearchResult[] { });
            AndGETTo($"/libraries/{userId}/links/received").As(user2Id).Returns(new LibrarySearchResult[] { });
            AndGETTo($"/libraries/{userId}/links/").As(userId).Returns(new LibrarySearchResult(user2Id, "library2", "user2Picture"));
            AndGETTo($"/libraries/{userId}/links/").As(user2Id).Returns(new LibrarySearchResult(userId, "library1", "user1Picture"));
            AndEventsSavedForAggregate<Library>(userId,
                new LibraryOpened(transactionId, userId, "library1", userId),
                new LinkRequested(transactionId, userId, user2Id),
                new LinkCompleted(transactionId, userId, user2Id));
            AndEventsSavedForAggregate<Library>(user2Id,
                new LibraryOpened(transactionId, user2Id, "library2", user2Id),
                new LinkRequestReceived(transactionId, user2Id, userId),
                new LinkAccepted(transactionId, user2Id, userId));
        }

        [Test]
        public void RequestLinkToSelfShouldFail()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            Given(() => OpenLibrary(transactionId, userId, "library1"));
            When(() => RequestLibraryLink(transactionId, userId, userId, userId));
            Then1(() => LinkRequestedToSelfIgnored());
            AndGETTo($"/libraries/{userId}/links/sent").As(userId).Returns(new LibrarySearchResult[] { });
            AndGETTo($"/libraries/{userId}/links/").As(userId).Returns(new LibrarySearchResult[] { });
            AndGETTo($"/libraries/{userId}/links/").As(userId).Returns(new LibrarySearchResult[] { });
            AndEventsSavedForAggregate<Library>(userId,
                new LibraryOpened(transactionId, userId, "library1", userId));
        }

        [Test]
        public void UnauthorizedRequestLinkShouldFail()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            Given(() => OpenLibrary(transactionId, userId, "library1"));
            When(() => RequestLibraryLink(transactionId, userId, Guid.Empty, Guid.Empty));
            Then1(() => UnauthorisedCommandRejected(Guid.Empty, typeof(Library), userId));
            AndGETTo($"/libraries/{userId}/links/sent").As(userId).Returns(new LibrarySearchResult[] { });
            AndGETTo($"/libraries/{userId}/links/").As(userId).Returns(new LibrarySearchResult[] { });
            AndGETTo($"/libraries/{userId}/links/").As(userId).Returns(new LibrarySearchResult[] { });
            AndEventsSavedForAggregate<Library>(userId,
                new LibraryOpened(transactionId, userId, "library1", userId));
        }
    }

}
