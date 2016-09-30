using System;
using Lending.Domain.AcceptLink;
using Lending.Domain.Model;
using Lending.Domain.OpenLibrary;
using Lending.Domain.RequestLink;
using NUnit.Framework;
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
        public void AcceptUnrequestedLinkIsIgnored()
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

            AndEventsSavedForAggregate<Library>(userId, new LibraryOpened(transactionId, userId, "library1", userId));
            AndEventsSavedForAggregate<Library>(user2Id, new LibraryOpened(transactionId, user2Id, "library2", user2Id));
        }

        [Test]
        public void AcceptLinkForLinkedLibrariesIsIgnored()
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

            AndEventsSavedForAggregate<Library>(userId,
                new LibraryOpened(transactionId, userId, "library1", userId),
                new LinkRequested(transactionId, userId, user2Id));
            AndEventsSavedForAggregate<Library>(user2Id,
                new LibraryOpened(transactionId, user2Id, "library2", user2Id),
                new LinkRequestReceived(transactionId, user2Id, userId));
        }

    }
}
