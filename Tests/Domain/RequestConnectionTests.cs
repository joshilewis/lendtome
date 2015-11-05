using System;
using System.Collections.Generic;
using System.Text;
using EventStore.ClientAPI;
using Lending.Cqrs;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Domain.AcceptConnection;
using Lending.Domain.Model;
using Lending.Domain.RegisterUser;
using Lending.Domain.RequestConnection;
using NUnit.Framework;
using ServiceStack.Text;

namespace Tests.Domain
{
    /// <summary>
    /// https://github.com/joshilewis/lending/issues/6
    /// </summary>
    [TestFixture]
    public class RequestConnectionTests : FixtureWithEventStoreAndNHibernate
    {
        #region Fields
        private Guid processId = Guid.Empty;
        private Guid user1Id;
        private Guid user2Id;

        //Commands
        private RegisterUser user1Registers;
        private RegisterUser user2Registers;
        private RequestConnection user1RequestsConnectionToUser2;
        private RequestConnection user1Requests2ndConnectionToUser2;
        private RequestConnection user2RequestsConnectionToUser1;
        private RequestConnection user1RequestsConnectionToSelf;
        private AcceptConnection user2AcceptsRequest1;

        //Events
        private UserRegistered user1Registered;
        private UserRegistered user2Registered;
        private ConnectionRequested connectionRequestedFrom1To2;
        private ConnectionRequested connectionRequestedFrom2To1;
        private ConnectionRequestReceived connectionRequestFrom1To2Received;
        private ConnectionRequestReceived connectionRequestFrom2To1Received;
        private ConnectionCompleted connectionCompleted;
        private ConnectionAccepted connectionAccepted;

        //Results
        private readonly Result failBecauseConnectionAlreadyRequested = new Result(User.ConnectionAlreadyRequested);
        private readonly Result succeed = new Result();
        private readonly Result failBecauseTargetUserDoesNotExist =
            new Result(RequestConnectionHandler.TargetUserDoesNotExist);
        private readonly Result failBecauseReverseConnectionAlreadyRequested =
            new Result(User.ReverseConnectionAlreadyRequested);
        private readonly Result failBecauseUsersAlreadyConnected = new Result(User.UsersAlreadyConnected);
        private readonly Result failBecauseCantConnectToSelf = new Result(RequestConnectionHandler.CantConnectToSelf); 
        #endregion

        public override void SetUp()
        {
            base.SetUp();
            user1Id= Guid.NewGuid();
            user2Id = Guid.NewGuid();
            processId = Guid.NewGuid();
            user1Registers = new RegisterUser(processId, user1Id, 1, "user1", "email1");
            user2Registers = new RegisterUser(processId, user2Id, 2, "user2", "email2");
            user1RequestsConnectionToUser2 = new RequestConnection(processId, user1Id, user1Id, user2Id);
            user2RequestsConnectionToUser1 = new RequestConnection(processId, user2Id, user2Id, user1Id);
            user1Requests2ndConnectionToUser2 = new RequestConnection(processId, user1Id, user1Id, user2Id);
            user1Registered = new UserRegistered(processId, user1Id, 1, user1Registers.UserName,
                user1Registers.PrimaryEmail);
            connectionRequestedFrom1To2 = new ConnectionRequested(processId, user1Id,
                user2Id);
            connectionRequestedFrom2To1 = new ConnectionRequested(processId, user2Id,
                user1Id);
            user2Registered = new UserRegistered(processId, user2Id, 2, user2Registers.UserName,
                user2Registers.PrimaryEmail);
            connectionRequestFrom1To2Received = new ConnectionRequestReceived(processId, user2Id,
                user1Id);
            connectionRequestFrom2To1Received = new ConnectionRequestReceived(processId, user1Id,
                user2Id);
            user1RequestsConnectionToSelf = new RequestConnection(processId, user1Id, user1Id, user1Id);
            user2AcceptsRequest1 = new AcceptConnection(processId, user2Id, user2Id,
                user1Id);

            connectionCompleted = new ConnectionCompleted(processId, user1Id, user2Id);
            connectionAccepted = new ConnectionAccepted(processId, user2Id, user1Id);

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
