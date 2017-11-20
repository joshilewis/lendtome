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
            var user1Id = "user1Id";
            var user2Id = "user2Id";
            var library1Id = OpenLibrary(transactionId, user1Id, "library1", "library1Picture");
            var library2Id = OpenLibrary(transactionId, user2Id, "library2", "library2Picture");
            RequestsLibraryLink(transactionId, library1Id, user1Id, library2Id);

            AcceptsLibraryLink(transactionId, library2Id, user2Id, library1Id);

            LibrariesLinked();
            EventsSavedForAggregate<Library>(library1Id,
                new LibraryOpened(transactionId, library1Id, "library1", user1Id, "library1Picture"),
                new LinkRequested(transactionId, library1Id, library2Id),
                new LinkCompleted(transactionId, library1Id, library2Id));
            EventsSavedForAggregate<Library>(library2Id,
                new LibraryOpened(transactionId, library2Id, "library2", user2Id, "library2Picture"),
                new LinkRequestReceived(transactionId, library2Id, library1Id),
                new LinkAccepted(transactionId, library2Id, library1Id));
        }

        [Test]
        public void AcceptUnrequestedLinkIsIgnored()
        {
            var transactionId = Guid.Empty;
            var user1Id = "user1Id";
            var user2Id = "user2Id";
            var library1Id = OpenLibrary(transactionId, user1Id, "library1", "library1Picture");
            var library2Id = OpenLibrary(transactionId, user2Id, "library2", "library2Picture");

            AcceptsLibraryLink(transactionId, library2Id, user2Id, library1Id);

            AcceptUnrequestedLinkIgnored();
            EventsSavedForAggregate<Library>(library1Id,
                new LibraryOpened(transactionId, library1Id, "library1", user1Id, "library1Picture"));
            EventsSavedForAggregate<Library>(library2Id,
                new LibraryOpened(transactionId, library2Id, "library2", user2Id, "library2Picture"));
        }

        [Test]
        public void AcceptLinkForLinkedLibrariesIsIgnored()
        {
            var transactionId = Guid.Empty;
            var user1Id = "user1Id";
            var user2Id = "user2Id";
            var library1Id = OpenLibrary(transactionId, user1Id, "library1", "library1Picture");
            var library2Id = OpenLibrary(transactionId, user2Id, "library2", "library2Picture");
            RequestsLibraryLink(transactionId, library1Id, user1Id, library2Id);
            AcceptsLibraryLink(transactionId, library2Id, user2Id, library1Id);

            AcceptsLibraryLink(transactionId, library2Id, user2Id, library1Id);

            AcceptLinkForLinkedLibrariesIgnored();
            EventsSavedForAggregate<Library>(library1Id,
                new LibraryOpened(transactionId, library1Id, "library1", user1Id, "library1Picture"),
                new LinkRequested(transactionId, library1Id, library2Id),
                new LinkCompleted(transactionId, library1Id, library2Id));
            EventsSavedForAggregate<Library>(library2Id,
                new LibraryOpened(transactionId, library2Id, "library2", user2Id, "library2Picture"),
                new LinkRequestReceived(transactionId, library2Id, library1Id),
                new LinkAccepted(transactionId, library2Id, library1Id));
        }

        [Test]
        public void UnuathorizedUserCantAcceptLink()
        {
            var transactionId = Guid.Empty;
            var user1Id = "user1Id";
            var user2Id = "user2Id";
            var library1Id = OpenLibrary(transactionId, user1Id, "library1", "library1Picture");
            var library2Id = OpenLibrary(transactionId, user2Id, "library2", "library2Picture");
            RequestsLibraryLink(transactionId, library1Id, user1Id, library2Id);

            AcceptsLibraryLink(transactionId, library2Id, "Unauthorised user", library1Id);

            UnauthorisedCommandIgnored("Unauthorised user", typeof(Library), library2Id);
            EventsSavedForAggregate<Library>(library1Id,
                new LibraryOpened(transactionId, library1Id, "library1", user1Id, "library1Picture"),
                new LinkRequested(transactionId, library1Id, library2Id));
            EventsSavedForAggregate<Library>(library2Id,
                new LibraryOpened(transactionId, library2Id, "library2", user2Id, "library2Picture"),
                new LinkRequestReceived(transactionId, library2Id, library1Id));
        }

    }
}
