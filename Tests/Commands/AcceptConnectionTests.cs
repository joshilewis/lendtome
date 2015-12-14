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
    /// https://github.com/joshilewis/lending/issues/7
    /// As a User I want to Accept a Requested Connection from another User So that we can see each other's Library.
    /// </summary>
    [TestFixture]
    public class AcceptConnectionTests : FixtureWithEventStoreAndNHibernate
    {

        /// <summary>
        /// GIVEN User1 exists AND User2 exists AND they are not connected AND User1 has requested to Connect to User2
        ///WHEN User2 accepts the connection request from User1
        ///THEN User1 and User2 are connected
        /// </summary>
        [Test]
        public void AcceptConnectionForUnconnectedUsersWithNoPendingRequestsShouldSucceed()
        {
            Given(user1Registers, user2Registers, user1RequestsConnectionToUser2);
            When(user2AcceptsRequestFrom1);
            Then(succeed);
            AndEventsSavedForAggregate<User>(user1Id, user1Registered, connectionRequestedFrom1To2, connectionCompleted);
            AndEventsSavedForAggregate<User>(user2Id, user2Registered, connectionRequestFrom1To2Received, connectionAccepted);
        }

        /// <summary>
        ///GIVEN User1 exists AND User2 exists AND they are not connected AND there are no Connection Requests between them
        ///WHEN User2 accepts the connection request from User1
        ///THEN No connection is made and User2 is informed that the acceptance failed because there is no Connection Request between them
        /// </summary>
        [Test]
        public void AcceptConnectionWithNoPendingRequestShouldFail()
        {
            Given(user1Registers, user2Registers);
            When(user2AcceptsRequestFrom1);
            Then(failBecauseNoPendingConnectionRequest);
            AndEventsSavedForAggregate<User>(user1Id, user1Registered);
            AndEventsSavedForAggregate<User>(user2Id, user2Registered);
        }

        /// <summary>
        ///GIVEN User1 exists AND User2 exists AND they are connected
        ///WHEN User2 accepts the connection request from User1
        ///THEN No connection is made and User2 is informed that the acceptance failed because they are already connected
        /// </summary>
        [Test]
        public void AcceptConnectionForConnectedUsersShouldFail()
        {
            Given(user1Registers, user2Registers, user1RequestsConnectionToUser2, user2AcceptsRequestFrom1);
            When(user2AcceptsRequestFrom1);
            Then(failBecauseUsersAlreadyConnected);
            AndEventsSavedForAggregate<User>(user1Id, user1Registered, connectionRequestedFrom1To2, connectionCompleted);
            AndEventsSavedForAggregate<User>(user2Id, user2Registered, connectionRequestFrom1To2Received, connectionAccepted);
        }

    }
}
