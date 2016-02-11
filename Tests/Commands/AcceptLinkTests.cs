using System;
using Lending.Cqrs.Query;
using Lending.Domain.Model;
using Lending.ReadModels.Relational.LinkAccepted;
using Lending.ReadModels.Relational.LinkRequested;
using NUnit.Framework;
using static Tests.DefaultTestData;

namespace Tests.Commands
{
    /// <summary>
    /// https://github.com/joshilewis/lending/issues/7
    /// As a Library I want to Accept a Link Request from another Library So that we can see each other's Books.
    /// </summary>
    [TestFixture]
    public class AcceptLinkTests : FixtureWithEventStoreAndNHibernate
    {
        private readonly LibraryLink[] linkFrom1To2 =
        {
            new LibraryLink(Guid.Empty, OpenedLibrary1, OpenedLibrary2),
        };

        private readonly LibraryLink[] emptyLibraryLinks = {};

        private readonly RequestedLink[] emptyRequestedLinks = {};

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
            this.GivenCommand(OpenLibrary1).IsPOSTedTo($"/libraries");
            this.GivenCommand(OpenLibrary2).IsPOSTedTo($"/libraries");
            this.GivenCommand(Library1RequestsLinkToLibrary2).IsPOSTedTo($"/libraries/{Library1Id}/links/request");
            this.WhenCommand(Library2AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library2Id}/links/accept");
            this.Then(Http200Ok);
            this.AndGETTo($"/libraries/{Library1Id}/links/sent").As(Library1Id).Returns(emptyRequestedLinks);
            this.AndGETTo($"/libraries/{Library1Id}/links/received").As(Library2Id).Returns(emptyRequestedLinks);
            this.AndGETTo($"/libraries/{Library1Id}/links/").As(Library1Id).Returns(linkFrom1To2);
            this.AndGETTo($"/libraries/{Library1Id}/links/").As(Library2Id).Returns(linkFrom1To2);
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened, LinkRequestedFrom1To2, DefaultTestData.LinkCompleted);
            AndEventsSavedForAggregate<Library>(Library2Id, Library2Opened, LinkRequestFrom1To2Received, DefaultTestData.LinkAccepted);
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
            this.GivenCommand(OpenLibrary1).IsPOSTedTo($"/libraries");
            this.GivenCommand(OpenLibrary2).IsPOSTedTo($"/libraries");
            this.WhenCommand(Library2AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library2Id}/links/accept");
            this.Then(this.Http400Because(Library.NoPendingLinkRequest));
            this.AndGETTo($"/libraries/{Library1Id}/links/sent").As(Library1Id).Returns(emptyRequestedLinks);
            this.AndGETTo($"/libraries/{Library1Id}/links/received").As(Library2Id).Returns(emptyRequestedLinks);
            this.AndGETTo($"/libraries/{Library1Id}/links/").As(Library1Id).Returns(emptyLibraryLinks);
            this.AndGETTo($"/libraries/{Library1Id}/links/").As(Library2Id).Returns(emptyLibraryLinks);
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
            this.GivenCommand(OpenLibrary1).IsPOSTedTo($"/libraries");
            this.GivenCommand(OpenLibrary2).IsPOSTedTo($"/libraries");
            this.GivenCommand(Library1RequestsLinkToLibrary2).IsPOSTedTo($"/libraries/{Library1Id}/links/request");
            this.GivenCommand(Library2AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library2Id}/links/accept");
            this.WhenCommand(Library2AcceptsLinkFromLibrary1).IsPOSTedTo($"/libraries/{Library2Id}/links/accept");
            this.Then(this.Http400Because(Library.LibrariesAlreadyLinked));
            this.AndGETTo($"/libraries/{Library1Id}/links/sent").As(Library1Id).Returns(emptyRequestedLinks);
            this.AndGETTo($"/libraries/{Library1Id}/links/received").As(Library2Id).Returns(emptyRequestedLinks);
            this.AndGETTo($"/libraries/{Library1Id}/links/").As(Library1Id)
                .Returns(linkFrom1To2);
            this.AndGETTo($"/libraries/{Library1Id}/links/").As(Library2Id)
                .Returns(linkFrom1To2);
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened, LinkRequestedFrom1To2, DefaultTestData.LinkCompleted);
            AndEventsSavedForAggregate<Library>(Library2Id, Library2Opened, LinkRequestFrom1To2Received, DefaultTestData.LinkAccepted);
        }

        [Test]
        public void AcceptLinkByUnauthorizedUserShouldFail()
        {
            this.GivenCommand(OpenLibrary1).IsPOSTedTo($"/libraries");
            this.GivenCommand(OpenLibrary2).IsPOSTedTo($"/libraries");
            this.GivenCommand(Library1RequestsLinkToLibrary2).IsPOSTedTo($"/libraries/{Library1Id}/links/request");
            this.WhenCommand(UnauthorizedAcceptLink).IsPOSTedTo($"/libraries/{Library2Id}/links/accept");
            this.Then(this.Http403BecauseUnauthorized(UnauthorizedAcceptLink.UserId, Library2Id, typeof (Library)));
            var requestedLinks = new[]
            {
                new RequestedLink(Guid.Empty, OpenedLibrary2, OpenedLibrary1),
            };
            this.AndGETTo($"/libraries/{Library1Id}/links/sent").As(Library1Id).Returns(requestedLinks);
            this.AndGETTo($"/libraries/{Library1Id}/links/received").As(Library2Id).Returns(requestedLinks);
            this.AndGETTo($"/libraries/{Library1Id}/links/").As(Library1Id).Returns(emptyLibraryLinks);
            this.AndGETTo($"/libraries/{Library1Id}/links/").As(Library2Id).Returns(emptyLibraryLinks);
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened, LinkRequestedFrom1To2);
            AndEventsSavedForAggregate<Library>(Library2Id, Library2Opened, LinkRequestFrom1To2Received);
        }

    }
}
