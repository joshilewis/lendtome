using System;
using System.Net;
using Lending.Cqrs.Exceptions;
using Lending.Cqrs.Query;
using Lending.Domain.Model;
using Lending.Domain.RequestLink;
using NUnit.Framework;
using static Tests.DefaultTestData;

namespace Tests.Commands
{
    /// <summary>
    /// https://github.com/joshilewis/lending/issues/6
    /// </summary>
    [TestFixture]
    public class RequestLinkTests : FixtureWithEventStoreAndNHibernate
    {
        /// <summary>
        /// GIVEN Library1 is Open AND Library2 is Open AND they are not Linked AND there is an existing Link Eequest from Library1 to Library2
        /// WHEN Library1 requests to Link to Library2
        /// THEN no request is created AND Library1 is informed that the request failed because a Link Request exists and is Pending
        /// </summary>
        [Test]
        public void RequestLinkFromLibraryWithPendingRequestShouldFail()
        {
            GivenCommand(OpenLibrary1).IsPUTedTo($"/libraries");
            GivenCommand(OpenLibrary2).IsPUTedTo($"/libraries");
            GivenCommand(Library1RequestsLinkToLibrary2).IsPUTedTo($"/libraries/{Library1Id}/links/request");
            WhenCommand(Library1Requests2NdLinkToLibrary2).IsPUTedTo($"/libraries/{Library1Id}/links/request");
            Then(Http400Because(Library.LinkAlreadyRequested));
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened, LinkRequestedFrom1To2);
            AndEventsSavedForAggregate<Library>(Library2Id, Library2Opened, LinkRequestFrom1To2Received);
        }

        /// <summary>
        /// GIVEN Library1 is Open AND Library2 is Open AND they are not Linked AND there are no Link Requests between them
        /// WHEN Library1 Requests a Link to Library2
        /// THEN the Request is created AND Library2 is informed of the Link Request
        /// </summary>
        [Test]
        public void RequestLinkForUnLinkedLibrarysShouldSucceed()
        {
            GivenCommand(OpenLibrary1).IsPUTedTo($"/libraries");
            GivenCommand(OpenLibrary2).IsPUTedTo($"/libraries");
            WhenCommand(Library1RequestsLinkToLibrary2).IsPUTedTo($"/libraries/{Library1Id}/links/request");
            Then(Http200Ok);
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened, LinkRequestedFrom1To2);
            AndEventsSavedForAggregate<Library>(Library2Id, Library2Opened, LinkRequestFrom1To2Received);
        }

        /// <summary>
        /// GIVEN Library1 is Open AND Library2 is not Open
        ///WHEN Library1 Requests to Link to Library2
        ///THEN no request is created AND Library1 is notified that the request failed because there is no such Library
        /// </summary>
        [Test]
        public void RequestLinkToNonExistentLibraryShouldFail()
        {
            GivenCommand(OpenLibrary1).IsPUTedTo($"/libraries");
            WhenCommand(Library1RequestsLinkToLibrary2).IsPUTedTo($"/libraries/{Library1Id}/links/request");
            Then(Http404BecauseTargetLibraryDoesNotExist);
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened);
        }

        /// <summary>
        /// GIVEN Library1 is Open AND Library2 is Open AND they are not Linked AND there is an existing Link Request from Library2 to Library1
        ///WHEN Library1 Requests to Link to Library2
        ///THEN no request is created AND Library1 is informed that the request failed because a Link Request exists AND is pending
        /// </summary>
        [Test]
        public void RequestLinkToLibraryWithPendingRequestShouldFail()
        {
            GivenCommand(OpenLibrary1).IsPUTedTo($"/libraries");
            GivenCommand(OpenLibrary2).IsPUTedTo($"/libraries");
            GivenCommand(Library2RequestsLinkToLibrary1).IsPUTedTo($"/libraries/{Library2Id}/links/request");
            WhenCommand(Library1RequestsLinkToLibrary2).IsPUTedTo($"/libraries/{Library1Id}/links/request");
            Then(Http400Because(Library.ReverseLinkAlreadyRequested));
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened, LinkRequestFrom2To1Received);
            AndEventsSavedForAggregate<Library>(Library2Id, Library2Opened, LinkRequestedFrom2To1);
        }

        /// <summary>
        /// GIVEN Library1 is Open AND Library2 is Open AND they are already Linked
        ///WHEN Library1 Requests to Link to Library2
        ///THEN no request is created AND Library1 is informed that the request failed because they are already Linked
        /// </summary>
        [Test]
        public void RequestLinkToLinkedLibrariesShouldFail()
        {
            GivenCommand(OpenLibrary1).IsPUTedTo($"/libraries");
            GivenCommand(OpenLibrary2).IsPUTedTo($"/libraries");
            GivenCommand(Library1RequestsLinkToLibrary2).IsPUTedTo($"/libraries/{Library1Id}/links/request");
            GivenCommand(Library2AcceptsLinkFromLibrary1).IsPUTedTo($"/libraries/{Library2Id}/links/accept");
            WhenCommand(Library1Requests2NdLinkToLibrary2).IsPUTedTo($"/libraries/{Library1Id}/links/request");
            Then(Http400Because(Library.LibrariesAlreadyLinked));
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened, LinkRequestedFrom1To2, DefaultTestData.LinkCompleted);
            AndEventsSavedForAggregate<Library>(Library2Id, Library2Opened, LinkRequestFrom1To2Received, DefaultTestData.LinkAccepted);

        }

        /// <summary>
        /// GIVEN Library1 exists
        ///WHEN Library1 requests a connection to Library1
        ///THEN no request is created AND Library1 is informed that the request failed because they can't connect to themselves
        /// </summary>
        [Test]
        public void RequestLinkToSelfShouldFail()
        {
            GivenCommand(OpenLibrary1).IsPUTedTo($"/libraries");
            WhenCommand(Library1RequestsLinkToSelf).IsPUTedTo($"/libraries/{Library1Id}/links/request");
            Then(Http400Because(RequestLinkHandler.CantConnectToSelf));
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened);
        }

        [Test]
        public void UnauthorizedRequestLinkShouldFail()
        {
            GivenCommand(OpenLibrary1).IsPUTedTo($"/libraries");
            WhenCommand(UnauthorizedRequestLink).IsPUTedTo($"/libraries/{Library1Id}/links/request");
            Then(Http403BecauseUnauthorized(UnauthorizedRequestLink.UserId, Library1Id, typeof (Library)));
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened);
        }
    }

}
