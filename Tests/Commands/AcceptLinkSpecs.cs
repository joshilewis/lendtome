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
            Runner.RunScenario(
                given => UserRegisters(userId, "user1", "email1", "user1Picture"),
                and => UserRegisters(user2Id, "user2", "email2", "user2Picture"),
                and => OpenLibrary(transactionId, userId, "library1"),
                and => OpenLibrary(transactionId, user2Id, "library2"),
                and => RequestLibraryLink(transactionId, userId, userId, user2Id),

                when => AcceptLibraryLink(transactionId, user2Id, user2Id, userId),

                then => LibrariesLinked(),
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
        public void AcceptUnrequestedLinkIsIgnored()
        {
            var transactionId = Guid.Empty;
            var userId = Guid.NewGuid();
            var user2Id = Guid.NewGuid();
            Runner.RunScenario(
                given => UserRegisters(userId, "user1", "email1", "user1Picture"),
                and => UserRegisters(user2Id, "user2", "email2", "user2Picture"),
                and => OpenLibrary(transactionId, userId, "library1"),
                and => OpenLibrary(transactionId, user2Id, "library2"),

                when => AcceptLibraryLink(transactionId, user2Id, user2Id, userId),
                
                then => AcceptUnrequestedLinkIgnored(),
                and =>
                    EventsSavedForAggregate<Library>(userId,
                        new LibraryOpened(transactionId, userId, "library1", userId)),
                and =>
                    EventsSavedForAggregate<Library>(user2Id,
                        new LibraryOpened(transactionId, user2Id, "library2", user2Id)));
        }

        [Test]
        public void AcceptLinkForLinkedLibrariesIsIgnored()
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

                when => AcceptLibraryLink(transactionId, user2Id, user2Id, userId),

                then => AcceptLinkForLinkedLibrariesIgnored(),
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
        public void UnuathorizedUserCantAcceptLink()
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

                when => AcceptLibraryLink(transactionId, user2Id, Guid.Empty, userId),

                then => UnauthorisedCommandIgnored(Guid.Empty, typeof(Library), user2Id),
                and => EventsSavedForAggregate<Library>(userId,
                    new LibraryOpened(transactionId, userId, "library1", userId),
                    new LinkRequested(transactionId, userId, user2Id)),
                and => EventsSavedForAggregate<Library>(user2Id,
                    new LibraryOpened(transactionId, user2Id, "library2", user2Id),
                    new LinkRequestReceived(transactionId, user2Id, userId)));
        }

    }
}
