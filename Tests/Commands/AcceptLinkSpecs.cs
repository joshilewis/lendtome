using System;
using Lending.Domain.AcceptLink;
using Lending.Domain.Model;
using Lending.Domain.OpenLibrary;
using Lending.Domain.RequestLink;
using Lending.ReadModels.Relational.ListLibrayLinks;
using NUnit.Framework;
using static Joshilewis.Testing.Helpers.ApiExtensions;
using static Joshilewis.Testing.Helpers.EventStoreExtensions;
using static Tests.AutomationExtensions;

namespace Tests.Commands
{
    /// <summary>
    /// https://github.com/joshilewis/lending/issues/7
    /// As a Library I want to Accept a Link Request from another Library So that we can see each other's Books.
    /// </summary>
    [TestFixture]
    public class AcceptLinkSpecs : Fixture
    {
        [Test]
        public void CanAcceptLinkFromUnlinkedRequester()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            var user2Id = Guid.NewGuid();
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            Given(() => UserRegisters(user2Id, "user2", "email2", "user2Picture"));
            Given(() => OpenLibrary(transactionId, userId, "library1"));
            Given(() => OpenLibrary(transactionId, user2Id, "library2"));
            Given(() => RequestLibraryLink(transactionId, userId, userId, user2Id));

            When(() => AcceptLibraryLink(transactionId, user2Id, user2Id, userId));
            Then1(() => LibrariesLinked());

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
        public void CantAcceptUnrequestedLink()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            var user2Id = Guid.NewGuid();
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            Given(() => UserRegisters(user2Id, "user2", "email2", "user2Picture"));
            Given(() => OpenLibrary(transactionId, userId, "library1"));
            Given(() => OpenLibrary(transactionId, user2Id, "library2"));

            When(() => AcceptLibraryLink(transactionId, user2Id, user2Id, userId));
            Then1(() => AcceptUnrequestedLinkIgnored());

            AndGETTo($"/libraries/{userId}/links/sent").As(userId).Returns(new LibrarySearchResult[] { });
            AndGETTo($"/libraries/{userId}/links/received").As(user2Id).Returns(new LibrarySearchResult[] { });
            AndGETTo($"/libraries/{userId}/links/").As(userId).Returns(new LibrarySearchResult[] { });
            AndGETTo($"/libraries/{user2Id}/links/").As(user2Id).Returns(new LibrarySearchResult[] { });
            AndEventsSavedForAggregate<Library>(userId, new LibraryOpened(transactionId, userId, "library1", userId));
            AndEventsSavedForAggregate<Library>(user2Id, new LibraryOpened(transactionId, user2Id, "library2", user2Id));
        }

        [Test]
        public void CantAcceptLinkForLinkedLibraries()
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
            When(() => AcceptLibraryLink(transactionId, user2Id, user2Id, userId));
            Then1(() => AcceptLinkForLinkedLibrariesIgnored());
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
        public void UnuathorizedUserCantAcceptLink()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            var user2Id = Guid.NewGuid();
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            Given(() => UserRegisters(user2Id, "user2", "email2", "user2Picture"));
            Given(() => OpenLibrary(transactionId, userId, "library1"));
            Given(() => OpenLibrary(transactionId, user2Id, "library2"));
            Given(() => RequestLibraryLink(transactionId, userId, userId, user2Id));

            When(() => AcceptLibraryLink(transactionId, user2Id, Guid.Empty, userId));
            Then1(() => UnauthorisedCommandIgnored(Guid.Empty, typeof(Library), user2Id));

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

    }
}
