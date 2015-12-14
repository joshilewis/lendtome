using System;
using Lending.Cqrs.Query;
using Lending.Domain.AcceptConnection;
using Lending.Domain.Model;
using Lending.Domain.RegisterUser;
using Lending.Domain.RequestConnection;
using NUnit.Framework;
using static Tests.DefaultTestData;

namespace Tests.Commands
{
    /// <summary>
    /// https://github.com/joshilewis/lending/issues/6
    /// </summary>
    [TestFixture]
    public class RequestConnectionTests : FixtureWithEventStoreAndNHibernate
    {

        public override void SetUp()
        {
            base.SetUp();


        }

        /// <summary>
        /// GIVEN User1 exists AND User2 exists AND they are not connected AND there is an existing connection request from User1 to User2
        /// WHEN User1 requests a connection to User2
        /// THEN no request is created AND User1 is informed that the request failed because a connection request exists and is pending
        /// </summary>
        [Test]
        public void RequestConnectionFromUserWithPendingRequestShouldFail()
        {
            Given(user1Registers, user2Registers, user1RequestsConnectionToUser2);
            When(user1Requests2ndConnectionToUser2);
            Then(failBecauseConnectionAlreadyRequested);
            AndEventsSavedForAggregate<User>(user1Id, user1Registered, connectionRequestedFrom1To2);
            AndEventsSavedForAggregate<User>(user2Id, user2Registered, connectionRequestFrom1To2Received);
        }

        /// <summary>
        /// GIVEN User1 Registers AND User2 Registers AND they are not connected AND there are no connection requests between them
        /// WHEN User1 Requests a Connection to User2
        /// THEN the request is created AND User2 is informed of the connection request
        /// </summary>
        [Test]
        public void RequestConnectionForUnconnectedUsersShouldSucceed()
        {
            Given(user1Registers, user2Registers);
            When(user1RequestsConnectionToUser2);
            Then(succeed);
            AndEventsSavedForAggregate<User>(user1Id, user1Registered, connectionRequestedFrom1To2);
            AndEventsSavedForAggregate<User>(user2Id, user2Registered, connectionRequestFrom1To2Received);
        }

        /// <summary>
        /// GIVEN User1 AND User2 does not exist
        ///WHEN User1 requests a connection to User2
        ///THEN no request is created AND User1 is notified that the request failed because there is no such user
        /// </summary>
        [Test]
        public void RequestConnectionToNonExistentUserShouldFail()
        {
            Given(user1Registers);
            When(user1RequestsConnectionToUser2);
            Then(failBecauseTargetUserDoesNotExist);
            AndEventsSavedForAggregate<User>(user1Id, user1Registered);
        }

        /// <summary>
        /// GIVEN User1 exists AND User2 exists AND they are not connected AND there is an existing connection request from User2 to User1
        ///WHEN User1 requests a connection to User2
        ///THEN no request is created AND User1 is informed that the request failed because a connection request exists AND is pending
        /// </summary>
        [Test]
        public void RequestConnectionToUserWithPendingRequestShouldFail()
        {
            Given(user1Registers, user2Registers, user2RequestsConnectionToUser1);
            When(user1RequestsConnectionToUser2);
            Then(failBecauseReverseConnectionAlreadyRequested);
            AndEventsSavedForAggregate<User>(user1Id, user1Registered, connectionRequestFrom2To1Received);
            AndEventsSavedForAggregate<User>(user2Id, user2Registered, connectionRequestedFrom2To1);
        }

        /// <summary>
        /// GIVEN User1 exists AND User2 exists AND they are already connected
        ///WHEN User1 requests a connection to User2
        ///THEN no request is created AND User1 is informed that the request failed because they are already connected
        /// </summary>
        [Test]
        public void RequestConnectionToConnectedUsersShouldFail()
        {
            Given(user1Registers, user2Registers, user1RequestsConnectionToUser2, user2AcceptsRequest1);
            When(user1Requests2ndConnectionToUser2);
            Then(failBecauseUsersAlreadyConnected);
            AndEventsSavedForAggregate<User>(user1Id, user1Registered, connectionRequestedFrom1To2, connectionCompleted);
            AndEventsSavedForAggregate<User>(user2Id, user2Registered, connectionRequestFrom1To2Received, connectionAccepted);

        }

        /// <summary>
        /// GIVEN User1 exists
        ///WHEN User1 requests a connection to User1
        ///THEN no request is created AND User1 is informed that the request failed because they can't connect to themselves
        /// </summary>
        [Test]
        public void RequestConnectionToSelfShouldFail()
        {
            Given(user1Registers);
            When(user1RequestsConnectionToSelf);
            Then(failBecauseCantConnectToSelf);
            AndEventsSavedForAggregate<User>(user1Id, user1Registered);
        }

    }
}
