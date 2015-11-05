using System;
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
    /// https://github.com/joshilewis/lending/issues/7
    /// As a User I want to Accept a Requested Connection from another User So that we can see each other's Library.
    /// </summary>
    [TestFixture]
    public class AcceptConnectionTests : FixtureWithEventStoreAndNHibernate
    {
        #region Fields
        private Guid processId = Guid.Empty;
        private Guid user1Id;
        private Guid user2Id;

        //Commands
        private RegisterUser user1Registers;
        private RegisterUser user2Registers;
        private RequestConnection user1RequestsConnectionToUser2;
        private AcceptConnection user2AcceptsRequestFrom1;

        //Events
        private UserRegistered user1Registered;
        private UserRegistered user2Registered;
        private ConnectionRequested connectionRequestedFrom1To2;
        private ConnectionRequestReceived connectionRequestFrom1To2Received;
        private ConnectionCompleted connectionCompleted;
        private ConnectionAccepted connectionAccepted;

        //Results
        private readonly Result succeed = new Result();
        private readonly Result failBecauseNoPendingConnectionRequest = new Result(User.NoPendingConnectionRequest);
        private readonly Result failBecauseUsersAlreadyConnected = new Result(User.UsersAlreadyConnected); 
        #endregion

        public override void SetUp()
        {
            base.SetUp();
            user1Id = Guid.NewGuid();
            user2Id = Guid.NewGuid();
            processId = Guid.NewGuid();
            user1Registers = new RegisterUser(processId, user1Id, 1, "user1", "email1");
            user2Registers = new RegisterUser(processId, user2Id, 2, "user2", "email2");
            user1RequestsConnectionToUser2 = new RequestConnection(processId, user1Id, user1Id, user2Id);
            user1Registered = new UserRegistered(processId, user1Id, 1, user1Registers.UserName,
                user1Registers.PrimaryEmail);
            connectionRequestedFrom1To2 = new ConnectionRequested(processId, user1Id,
                user2Id);
            user2Registered = new UserRegistered(processId, user2Id, 2, user2Registers.UserName,
                user2Registers.PrimaryEmail);
            connectionRequestFrom1To2Received = new ConnectionRequestReceived(processId, user2Id,
                user1Id);
            user2AcceptsRequestFrom1 = new AcceptConnection(processId, user2Id, user2Id,
                user1Id);

            connectionCompleted = new ConnectionCompleted(processId, user1Id, user2Id);
            connectionAccepted = new ConnectionAccepted(processId, user2Id, user1Id);
        }

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
