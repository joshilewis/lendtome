﻿using System;
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
            var user1Id = "user1Id";
            var user2Id = "user2Id";

            UserRegisters(user1Id, "user1", "email1", "user1Picture");
            UserRegisters(user2Id, "user2", "email2", "user2Picture");
            var library1Id = OpenLibrary(transactionId, user1Id, "library1");
            var library2Id = OpenLibrary(transactionId, user2Id, "library2");

            RequestsLibraryLink(transactionId, library1Id, user1Id, library2Id);

            LinkRequestCreated();
            EventsSavedForAggregate<Library>(library1Id,
                new LibraryOpened(transactionId, library1Id, "library1", user1Id),
                new LinkRequested(transactionId, library1Id, library2Id));
            EventsSavedForAggregate<Library>(library2Id,
                new LibraryOpened(transactionId, library2Id, "library2", user2Id),
                new LinkRequestReceived(transactionId, library2Id, library1Id));
        }

        [Test]
        public void SendingDuplicateLinkRequestIgnored()
        {
            var transactionId = Guid.Empty;
            var user1Id = "user1Id";
            var user2Id = "user2Id";

            UserRegisters(user1Id, "user1", "email1", "user1Picture");
            UserRegisters(user2Id, "user2", "email2", "user2Picture");
            var library1Id = OpenLibrary(transactionId, user1Id, "library1");
            var library2Id = OpenLibrary(transactionId, user2Id, "library2");
            RequestsLibraryLink(transactionId, library1Id, user1Id, library2Id);

            RequestsLibraryLink(transactionId, library1Id, user1Id, library2Id);

            DuplicateLinkRequestIgnored();
            EventsSavedForAggregate<Library>(library1Id,
                new LibraryOpened(transactionId, library1Id, "library1", user1Id),
                new LinkRequested(transactionId, library1Id, library2Id));
            EventsSavedForAggregate<Library>(library2Id,
                new LibraryOpened(transactionId, library2Id, "library2", user2Id),
                new LinkRequestReceived(transactionId, library2Id, library1Id));
        }

        [Test]
        public void CantRequestLinkToNonExistentLibrary()
        {
            var transactionId = Guid.Empty;
            var user1Id = "user1Id";
            var library2Id = Guid.NewGuid();

            UserRegisters(user1Id, "user1", "email1", "user1Picture");
            var library1Id = OpenLibrary(transactionId, user1Id, "library1");

            RequestsLibraryLink(transactionId, library1Id, user1Id, library2Id);

            FailtNotFound();
            EventsSavedForAggregate<Library>(library1Id,
                new LibraryOpened(transactionId, library1Id, "library1", user1Id));
        }

        [Test]
        public void RequestLinkToLibraryWithReverseLinkRequestIsIgnored()
        {
            var transactionId = Guid.Empty;
            var user1Id = "user1Id";
            var user2Id = "user2Id";

            UserRegisters(user1Id, "user1", "email1", "user1Picture");
            UserRegisters(user2Id, "user2", "email2", "user2Picture");
            var library1Id = OpenLibrary(transactionId, user1Id, "library1");
            var library2Id = OpenLibrary(transactionId, user2Id, "library2");
            RequestsLibraryLink(transactionId, library2Id, user2Id, library1Id);

            RequestsLibraryLink(transactionId, library1Id, user1Id, library2Id);

            ReverseLinkRequestIgnored();
            EventsSavedForAggregate<Library>(library1Id,
                new LibraryOpened(transactionId, library1Id, "library1", user1Id),
                new LinkRequestReceived(transactionId, library1Id, library2Id));
            EventsSavedForAggregate<Library>(library2Id,
                new LibraryOpened(transactionId, library2Id, "library2", user2Id),
                new LinkRequested(transactionId, library2Id, library1Id));
        }

        [Test]
        public void RequestLinkToLinkedLibraryIsIgnored()
        {
            var transactionId = Guid.Empty;
            var user1Id = "user1Id";
            var user2Id = "user2Id";

            UserRegisters(user1Id, "user1", "email1", "user1Picture");
            UserRegisters(user2Id, "user2", "email2", "user2Picture");
            var library1Id = OpenLibrary(transactionId, user1Id, "library1");
            var library2Id = OpenLibrary(transactionId, user2Id, "library2");
            RequestsLibraryLink(transactionId, library1Id, user1Id, library2Id);
            AcceptsLibraryLink(transactionId, library2Id, user2Id, library1Id);

            RequestsLibraryLink(transactionId, library1Id, user1Id, library2Id);

            LinkRequestForLinkedLibrariesIgnored();
            EventsSavedForAggregate<Library>(library1Id,
                new LibraryOpened(transactionId, library1Id, "library1", user1Id),
                new LinkRequested(transactionId, library1Id, library2Id),
                new LinkCompleted(transactionId, library1Id, library2Id));
            EventsSavedForAggregate<Library>(library2Id,
                new LibraryOpened(transactionId, library2Id, "library2", user2Id),
                new LinkRequestReceived(transactionId, library2Id, library1Id),
                new LinkAccepted(transactionId, library2Id, library1Id));
        }

        [Test]
        public void RequestLinkToSelfIsIgnored()
        {
            var transactionId = Guid.Empty;
            var user1Id = "user1Id";

            UserRegisters(user1Id, "user1", "email1", "user1Picture");
            var library1Id = OpenLibrary(transactionId, user1Id, "library1");

            RequestsLibraryLink(transactionId, library1Id, user1Id, library1Id);

            LinkRequestedToSelfIgnored();
            EventsSavedForAggregate<Library>(library1Id,
                new LibraryOpened(transactionId, library1Id, "library1", user1Id));
        }

        [Test]
        public void UnauthorizedUserCantRequestLink()
        {
            var transactionId = Guid.Empty;
            var user1Id = "user1Id";

            UserRegisters(user1Id, "user1", "email1", "user1Picture");
            var library1Id = OpenLibrary(transactionId, user1Id, "library1");

            RequestsLibraryLink(transactionId, library1Id, "Unauthorised User", Guid.Empty);

            UnauthorisedCommandIgnored("Unauthorised User", typeof(Library), library1Id);
            EventsSavedForAggregate<Library>(library1Id,
                new LibraryOpened(transactionId, library1Id, "library1", user1Id));
        }
    }

}
