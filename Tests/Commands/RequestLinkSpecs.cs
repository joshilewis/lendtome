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
    /// https://github.com/joshilewis/lending/issues/6
    /// </summary>
    [TestFixture]
    public class RequestLinkSpecs : Fixture
    {
        [Test]
        public void CanRequestLinkForUnlinkedLibrary()
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
            AndEventsSavedForAggregate<Library>(userId, 
                new LibraryOpened(transactionId, userId, "library1", userId), 
                new LinkRequested(transactionId, userId, user2Id));
            AndEventsSavedForAggregate<Library>(user2Id, 
                new LibraryOpened(transactionId, user2Id, "library2", user2Id), 
                new LinkRequestReceived(transactionId, user2Id, userId));
        }

        [Test]
        public void SendingDuplicateLinkRequestIgnored()
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
            AndEventsSavedForAggregate<Library>(userId,
                new LibraryOpened(transactionId, userId, "library1", userId),
                new LinkRequested(transactionId, userId, user2Id));
            AndEventsSavedForAggregate<Library>(user2Id,
                new LibraryOpened(transactionId, user2Id, "library2", user2Id),
                new LinkRequestReceived(transactionId, user2Id, userId));
        }

        [Test]
        public void CantRequestLinkToNonExistentLibrary()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            var user2Id = Guid.NewGuid();
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            Given(() => OpenLibrary(transactionId, userId, "library1"));
            When(() => RequestLibraryLink(transactionId, userId, userId, user2Id));
            Then1(() => FailtNotFound());
            AndEventsSavedForAggregate<Library>(userId,
                new LibraryOpened(transactionId, userId, "library1", userId));
        }

        [Test]
        public void RequestLinkToLibraryWithReverseLinkRequestIsIgnored()
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
            AndEventsSavedForAggregate<Library>(userId,
                new LibraryOpened(transactionId, userId, "library1", userId),
                new LinkRequestReceived(transactionId, userId, user2Id));
            AndEventsSavedForAggregate<Library>(user2Id,
                new LibraryOpened(transactionId, user2Id, "library2", user2Id),
                new LinkRequested(transactionId, user2Id, userId));
        }

        [Test]
        public void RequestLinkToLinkedLibraryIsIgnored()
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
        public void CantRequestLinkToSelf()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            Given(() => OpenLibrary(transactionId, userId, "library1"));
            When(() => RequestLibraryLink(transactionId, userId, userId, userId));
            Then1(() => LinkRequestedToSelfIgnored());
            AndEventsSavedForAggregate<Library>(userId,
                new LibraryOpened(transactionId, userId, "library1", userId));
        }

        [Test]
        public void UnauthorizedUserCantRequestLink()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            Given(() => OpenLibrary(transactionId, userId, "library1"));
            When(() => RequestLibraryLink(transactionId, userId, Guid.Empty, Guid.Empty));
            Then1(() => UnauthorisedCommandIgnored(Guid.Empty, typeof(Library), userId));
            AndEventsSavedForAggregate<Library>(userId,
                new LibraryOpened(transactionId, userId, "library1", userId));
        }
    }

}
