using System;
using Lending.Cqrs.Query;
using Lending.Domain.Model;
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

        public override void SetUp()
        {
            base.SetUp();


        }

        /// <summary>
        /// GIVEN Library1 is Open AND Library2 is Open AND they are not Linked AND there is an existing Link Eequest from Library1 to Library2
        /// WHEN Library1 requests to Link to Library2
        /// THEN no request is created AND Library1 is informed that the request failed because a Link Request exists and is Pending
        /// </summary>
        [Test]
        public void RequestLinkFromLibraryWithPendingRequestShouldFail()
        {
            Given(Library1Opens, Library2Opens, Library1RequestsLinkToLibrary2);
            When(Library1Requests2NdLinkToLibrary2);
            Then(FailBecauseLinkAlreadyRequested);
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
            Given(Library1Opens, Library2Opens);
            When(Library1RequestsLinkToLibrary2);
            Then(succeed);
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
            Given(Library1Opens);
            When(Library1RequestsLinkToLibrary2);
            Then(FailBecauseTargetLibraryDoesNotExist);
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
            Given(Library1Opens, Library2Opens, Library2RequestsLinkToLibrary1);
            When(Library1RequestsLinkToLibrary2);
            Then(FailBecauseReverseLinkAlreadyRequested);
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
            Given(Library1Opens, Library2Opens, Library1RequestsLinkToLibrary2, Library2AcceptsLinkFromLibrary1);
            When(Library1Requests2NdLinkToLibrary2);
            Then(FailBecauseLibrariesAlreadyLinked);
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
            Given(Library1Opens);
            When(Library1RequestsLinkToSelf);
            Then(FailBecauseCantLinkToSelf);
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened);
        }
    }
}
