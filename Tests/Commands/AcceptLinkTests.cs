using System;
using System.Collections.Generic;
using Joshilewis.Testing.Helpers;
using Lending.Domain.Model;
using Lending.ReadModels.Relational;
using Lending.ReadModels.Relational.LinkAccepted;
using Lending.ReadModels.Relational.LinkRequested;
using Lending.ReadModels.Relational.ListLibrayLinks;
using Lending.ReadModels.Relational.ListRequestedLinks;
using NUnit.Framework;
using static Tests.TestData;
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
    public class AcceptLinkTests : Fixture
    {
        private readonly LibrarySearchResult library2SearchResult = new LibrarySearchResult(Library2Id, Library2Name, Library2Picture);
        private readonly LibrarySearchResult library1SearchResult = new LibrarySearchResult(Library1Id, Library1Name, Library1Picture);

        /// <summary>
        /// GIVEN Library1 AND Library2 are Open AND they are not Linked AND Library1 has Requested to Link to Library2
        /// WHEN Library2 Accepts the Link Request from Library1
        /// THEN HTTP200 is Returned
        /// AND nothing appears in Library1's Sent Link Requests
        /// AND nothing appears in Library2's Received Link Requests
        /// AND Library2 appears in Library1's Links
        /// AND Library1 appears in Library2's Links
        /// </summary>
        [Test]
        public void AcceptLinkForUnconnectedLibrarysWithAPendingRequestShouldSucceed()
        {
            Given(() => UserRegisters(Library1Id, "library1", "email1", "library1Picture"));
            Given(() => UserRegisters(Library2Id, "library2", "email2", "library2Picture"));
            GivenCommand(OpenLibrary1).IsPOSTedTo($"/libraries");
            GivenCommand(OpenLibrary2).IsPOSTedTo($"/libraries");
            GivenCommand(Library1RequestsLinkToLibrary2).IsPOSTedTo($"/libraries/{Library1Id}/links/request");
            WhenCommand(Library2AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library2Id}/links/accept");
            Then(Http200Ok);
            AndGETTo($"/libraries/{Library1Id}/links/sent").As(Library1Id).Returns(EmptyList);
            AndGETTo($"/libraries/{Library1Id}/links/received").As(Library2Id).Returns(EmptyList);
            AndGETTo($"/libraries/{Library1Id}/links/").As(Library1Id).Returns(new LibrarySearchResult(Library2Id, Library2Name, Library2Picture));
            AndGETTo($"/libraries/{Library1Id}/links/").As(Library2Id).Returns(new LibrarySearchResult(Library1Id, Library1Name, Library1Picture));
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened, LinkRequestedFrom1To2, TestData.LinkCompleted);
            AndEventsSavedForAggregate<Library>(Library2Id, Library2Opened, LinkRequestFrom1To2Received, TestData.LinkAccepted);
        }

        /// <summary>
        /// GIVEN Library1 AND Library2 are Open AND they are not Linked AND there are no Link Requests between them
        /// WHEN Library2 accepts the Link Request from Library1
        /// THEN HTTP400 is returned because there are no Link Requests between Library1 and Library2
        /// AND nothing appears in Library1's Sent Link Requests
        /// AND nothing appears in Library2's Received Link Requests
        /// AND nothing appears in Library1's Links
        /// AND nothing appears in Library2's Links
        /// </summary>
        [Test]
        public void AcceptLinkWithNoPendingRequestShouldFail()
        {
            PersistenceExtensions.SaveEntities(User1, User2);
            GivenCommand(OpenLibrary1).IsPOSTedTo($"/libraries");
            GivenCommand(OpenLibrary2).IsPOSTedTo($"/libraries");
            WhenCommand(Library2AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library2Id}/links/accept");
            Then(Http400Because(Library.NoPendingLinkRequest));
            AndGETTo($"/libraries/{Library1Id}/links/sent").As(Library1Id).Returns(EmptyList);
            AndGETTo($"/libraries/{Library1Id}/links/received").As(Library2Id).Returns(EmptyList);
            AndGETTo($"/libraries/{Library1Id}/links/").As(Library1Id).Returns(EmptyList);
            AndGETTo($"/libraries/{Library1Id}/links/").As(Library2Id).Returns(EmptyList);
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened);
            AndEventsSavedForAggregate<Library>(Library2Id, Library2Opened);
        }

        /// <summary>
        /// GIVEN Library1 AND Library2 are open AND they are Linked
        /// WHEN Library2 accepts a Link Request from Library1
        /// THEN HTTP400 is returned because the Libraries are already Linked
        /// AND nothing appears in Library1's Sent Link Requests
        /// AND nothing appears in Library2's Received Link Requests
        /// AND Library2 appears in Library1's Links
        /// AND Library1 appears in Library2's Links
        /// </summary>
        [Test]
        public void AcceptLinkForLinkedLibrariesShouldFail()
        {
            PersistenceExtensions.SaveEntities(User1, User2);
            GivenCommand(OpenLibrary1).IsPOSTedTo($"/libraries");
            GivenCommand(OpenLibrary2).IsPOSTedTo($"/libraries");
            GivenCommand(Library1RequestsLinkToLibrary2).IsPOSTedTo($"/libraries/{Library1Id}/links/request");
            GivenCommand(Library2AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library2Id}/links/accept");
            WhenCommand(Library2AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library2Id}/links/accept");
            Then(Http400Because(Library.LibrariesAlreadyLinked));
            AndGETTo($"/libraries/{Library1Id}/links/sent").As(Library1Id).Returns(EmptyList);
            AndGETTo($"/libraries/{Library1Id}/links/received").As(Library2Id).Returns(EmptyList);
            AndGETTo($"/libraries/{Library1Id}/links/").As(Library1Id).Returns(library2SearchResult);
            AndGETTo($"/libraries/{Library1Id}/links/").As(Library2Id).Returns(library1SearchResult);
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened, LinkRequestedFrom1To2, TestData.LinkCompleted);
            AndEventsSavedForAggregate<Library>(Library2Id, Library2Opened, LinkRequestFrom1To2Received, TestData.LinkAccepted);
        }

        [Test]
        public void AcceptLinkByUnauthorizedUserShouldFail()
        {
            PersistenceExtensions.SaveEntities(User1, User2);
            GivenCommand(OpenLibrary1).IsPOSTedTo($"/libraries");
            GivenCommand(OpenLibrary2).IsPOSTedTo($"/libraries");
            GivenCommand(Library1RequestsLinkToLibrary2).IsPOSTedTo($"/libraries/{Library1Id}/links/request");
            WhenCommand(UnauthorizedAcceptLink).IsPOSTedTo($"/libraries/{Library2Id}/links/accept");
            Then(Http403BecauseUnauthorized(UnauthorizedAcceptLink.UserId, Library2Id, typeof (Library)));
            AndGETTo($"/libraries/{Library1Id}/links/sent").As(Library1Id).Returns(new LibrarySearchResult(Library2Id, Library2Name, Library2Picture));
            AndGETTo($"/libraries/{Library1Id}/links/received").As(Library2Id).Returns(new LibrarySearchResult(Library1Id, Library1Name, Library1Picture));
            AndGETTo($"/libraries/{Library1Id}/links/").As(Library1Id).Returns(EmptyList);
            AndGETTo($"/libraries/{Library1Id}/links/").As(Library2Id).Returns(EmptyList);
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened, LinkRequestedFrom1To2);
            AndEventsSavedForAggregate<Library>(Library2Id, Library2Opened, LinkRequestFrom1To2Received);
        }

    }
}
