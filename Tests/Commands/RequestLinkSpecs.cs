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
            Runner.RunScenario(
                given => UserRegisters(userId, "user1", "email1", "user1Picture"),
                and => UserRegisters(user2Id, "user2", "email2", "user2Picture"),
                and => OpenLibrary(transactionId, userId, "library1"),
                and => OpenLibrary(transactionId, user2Id, "library2"),

                when => RequestLibraryLink(transactionId, userId, userId, user2Id),

                then => LinkRequestCreated(),
                and => EventsSavedForAggregate<Library>(userId,
                    new LibraryOpened(transactionId, userId, "library1", userId),
                    new LinkRequested(transactionId, userId, user2Id)),
                and => EventsSavedForAggregate<Library>(user2Id,
                    new LibraryOpened(transactionId, user2Id, "library2", user2Id),
                    new LinkRequestReceived(transactionId, user2Id, userId)));
        }

        [Test]
        public void SendingDuplicateLinkRequestIgnored()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            var user2Id = Guid.NewGuid();
            Runner.RunScenario(
                given => UserRegisters(userId, "user1", "email1", "user1Picture"),
                given => UserRegisters(user2Id, "user2", "email2", "user2Picture"),
                given => OpenLibrary(transactionId, userId, "library1"),
                given => OpenLibrary(transactionId, user2Id, "library2"),
                given => RequestLibraryLink(transactionId, userId, userId, user2Id),

                when => RequestLibraryLink(transactionId, userId, userId, user2Id),

                then => DuplicateLinkRequestIgnored(),
                and => EventsSavedForAggregate<Library>(userId,
                    new LibraryOpened(transactionId, userId, "library1", userId),
                    new LinkRequested(transactionId, userId, user2Id)),
                and => EventsSavedForAggregate<Library>(user2Id,
                    new LibraryOpened(transactionId, user2Id, "library2", user2Id),
                    new LinkRequestReceived(transactionId, user2Id, userId)));
        }

        [Test]
        public void CantRequestLinkToNonExistentLibrary()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            var user2Id = Guid.NewGuid();
            Runner.RunScenario(
                given => UserRegisters(userId, "user1", "email1", "user1Picture"),
                and => OpenLibrary(transactionId, userId, "library1"),

                when => RequestLibraryLink(transactionId, userId, userId, user2Id),

                then => FailtNotFound(),
                and => EventsSavedForAggregate<Library>(userId,
                    new LibraryOpened(transactionId, userId, "library1", userId)));
        }

        [Test]
        public void RequestLinkToLibraryWithReverseLinkRequestIsIgnored()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            var user2Id = Guid.NewGuid();
            Runner.RunScenario(
                given => UserRegisters(userId, "user1", "email1", "user1Picture"),
                and => UserRegisters(user2Id, "user2", "email2", "user2Picture"),
                and => OpenLibrary(transactionId, userId, "library1"),
                and => OpenLibrary(transactionId, user2Id, "library2"),
                and => RequestLibraryLink(transactionId, user2Id, user2Id, userId),

                when => RequestLibraryLink(transactionId, userId, userId, user2Id),

                then => ReverseLinkRequestIgnored(),
                and => EventsSavedForAggregate<Library>(userId,
                    new LibraryOpened(transactionId, userId, "library1", userId),
                    new LinkRequestReceived(transactionId, userId, user2Id)),
                and => EventsSavedForAggregate<Library>(user2Id,
                    new LibraryOpened(transactionId, user2Id, "library2", user2Id),
                    new LinkRequested(transactionId, user2Id, userId)));
        }

        [Test]
        public void RequestLinkToLinkedLibraryIsIgnored()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            var user2Id = Guid.NewGuid();
            Runner.RunScenario(
                given => UserRegisters(userId, "user1", "email1", "user1Picture"),
                and => UserRegisters(user2Id, "user2", "email2", "user2Picture"),
                and => OpenLibrary(transactionId, userId, "library1"),
                and => OpenLibrary(transactionId, user2Id, "library2"),
                and => RequestLibraryLink(transactionId, userId, userId, user2Id),
                and => AcceptLibraryLink(transactionId, user2Id, user2Id, userId),

                when => RequestLibraryLink(transactionId, userId, userId, user2Id),

                then => LinkRequestForLinkedLibrariesIgnored(),
                and => EventsSavedForAggregate<Library>(userId,
                    new LibraryOpened(transactionId, userId, "library1", userId),
                    new LinkRequested(transactionId, userId, user2Id),
                    new LinkCompleted(transactionId, userId, user2Id)),
                and => EventsSavedForAggregate<Library>(user2Id,
                    new LibraryOpened(transactionId, user2Id, "library2", user2Id),
                    new LinkRequestReceived(transactionId, user2Id, userId),
                    new LinkAccepted(transactionId, user2Id, userId)));
        }

        [Test]
        public void RequestLinkToSelfIsIgnored()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            Runner.RunScenario(
                given => UserRegisters(userId, "user1", "email1", "user1Picture"),
                and => OpenLibrary(transactionId, userId, "library1"),

                when => RequestLibraryLink(transactionId, userId, userId, userId),

                then => LinkRequestedToSelfIgnored(),
                and => EventsSavedForAggregate<Library>(userId,
                    new LibraryOpened(transactionId, userId, "library1", userId)));
        }

        [Test]
        public void UnauthorizedUserCantRequestLink()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            Runner.RunScenario(
                given => UserRegisters(userId, "user1", "email1", "user1Picture"),
                and => OpenLibrary(transactionId, userId, "library1"),

                when => RequestLibraryLink(transactionId, userId, Guid.Empty, Guid.Empty),

                then => UnauthorisedCommandIgnored(Guid.Empty, typeof(Library), userId),
                and => EventsSavedForAggregate<Library>(userId,
                    new LibraryOpened(transactionId, userId, "library1", userId)));
        }
    }

}
