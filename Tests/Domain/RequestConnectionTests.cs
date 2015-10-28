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
        /// <summary>
        /// GIVEN User1 exists AND User2 exists AND they are not connected AND there is an existing connection request from User1 to User2
        /// WHEN User1 requests a connection to User2
        /// THEN no request is created AND User1 is informed that the request failed because a connection request exists and is pending
        /// </summary>
        [Test]
        public void RequestConnectionFromUserWithPendingRequestShouldFail()
        {
            Guid processId = Guid.NewGuid();

            var registerUser1 = new RegisterUser(processId, Guid.NewGuid(), 1, "user1", "email1");
            var registerUser2 = new RegisterUser(processId, Guid.NewGuid(), 2, "user2", "email2");
            var requestConnection = new RequestConnection(processId, registerUser1.UserId, registerUser1.UserId, registerUser2.UserId);

            var secondRequestConnection = new RequestConnection(processId, registerUser1.UserId, registerUser1.UserId, registerUser2.UserId);

            var expectedResult = new Result(User.ConnectionAlreadyRequested);

            var expectedEventsForUser1 = new Event[]
            {
                new UserRegistered(processId, registerUser1.UserId, registerUser1.UserName, registerUser1.PrimaryEmail), 
                new ConnectionRequested(processId, registerUser1.UserId, registerUser2.UserId), 
            };

            var expectedEventsForUser2 = new Event[]
            {
                new UserRegistered(processId, registerUser2.UserId, registerUser2.UserName, registerUser2.PrimaryEmail),
                new ConnectionRequestReceived(processId, registerUser2.UserId, registerUser1.UserId), 
            };

            Given(registerUser1, registerUser2, requestConnection);
            When(secondRequestConnection);
            Then(expectedResult);
            And<User>(registerUser1.UserId, expectedEventsForUser1);
            And<User>(registerUser2.UserId, expectedEventsForUser2);
        }

        /// <summary>
        /// GIVEN User1 exists AND User2 exists AND they are not connected AND there are no connection requests between them
        ///WHEN User1 requests a connection to User2
        ///THEN the request is created AND User2 is informed of the connection request
        /// </summary>
        [Test]
        public void RequestConnectionForUnconnectedUsersShouldSucceed()
        {

            Guid processId = Guid.NewGuid();
            var registerUser1 = new RegisterUser(processId, Guid.NewGuid(), 1, "user1", "email1");
            var registerUser2 = new RegisterUser(processId, Guid.NewGuid(), 2, "user2", "email2");
            var requestConnection = new RequestConnection(processId, registerUser1.UserId, registerUser1.UserId, registerUser2.UserId);
            var expectedResult = new Result();

            var expectedEventsForUser1 = new Event[]
            {
                new UserRegistered(processId, registerUser1.UserId, registerUser1.UserName, registerUser1.PrimaryEmail),
                new ConnectionRequested(processId, registerUser1.UserId, registerUser2.UserId),
            };

            var expectedEventsForUser2 = new Event[]
            {
                new UserRegistered(processId, registerUser2.UserId, registerUser2.UserName, registerUser2.PrimaryEmail),
                new ConnectionRequestReceived(processId, registerUser2.UserId, registerUser1.UserId),
            };

            Given(registerUser1, registerUser2);
            When(requestConnection);
            Then(expectedResult);
            And<User>(registerUser1.UserId, expectedEventsForUser1);
            And<User>(registerUser2.UserId, expectedEventsForUser2);

        }

        /// <summary>
        /// GIVEN User1 AND User2 does not exist
        ///WHEN User1 requests a connection to User2
        ///THEN no request is created AND User1 is notified that the request failed because there is no such user
        /// </summary>
        [Test]
        public void RequestConnectionToNonExistentUserShouldFail()
        {
            Guid processId = Guid.NewGuid();
            var registerUser1 = new RegisterUser(processId, Guid.NewGuid(), 1, "user1", "email1");
            var requestConnection = new RequestConnection(processId, registerUser1.UserId, registerUser1.UserId, Guid.NewGuid());
            var expectedResult = new Result(RequestConnectionHandler.TargetUserDoesNotExist);

            var expectedEventsForUser1 = new Event[]
            {
                new UserRegistered(processId, registerUser1.UserId, registerUser1.UserName, registerUser1.PrimaryEmail),
            };

            Given(registerUser1);
            When(requestConnection);
            Then(expectedResult);
            And<User>(registerUser1.UserId, expectedEventsForUser1);
        }

        /// <summary>
        /// GIVEN User1 exists AND User2 exists AND they are not connected AND there is an existing connection request from User2 to User1
        ///WHEN User1 requests a connection to User2
        ///THEN no request is created AND User1 is informed that the request failed because a connection request exists AND is pending
        /// </summary>
        [Test]
        public void RequestConnectionToUserWithPendingRequestShouldFail()
        {
            Guid processId = Guid.NewGuid();
            var registerUser1 = new RegisterUser(processId, Guid.NewGuid(), 1, "user1", "email1");
            var registerUser2 = new RegisterUser(processId, Guid.NewGuid(), 2, "user2", "email2");
            var requestConnection = new RequestConnection(processId, registerUser2.UserId, registerUser2.UserId, registerUser1.UserId);
            var secondRequestConnection = new RequestConnection(processId, registerUser1.UserId, registerUser1.UserId, registerUser2.UserId);
            var expectedResult = new Result(User.ReverseConnectionAlreadyRequested);

            var expectedEventsForUser1 = new Event[]
            {
                new UserRegistered(processId, registerUser1.UserId, registerUser1.UserName, registerUser1.PrimaryEmail),
                new ConnectionRequestReceived(processId, registerUser1.UserId, registerUser2.UserId),
            };

            var expectedEventsForUser2 = new Event[]
            {
                new UserRegistered(processId, registerUser2.UserId, registerUser2.UserName, registerUser2.PrimaryEmail),
                new ConnectionRequested(processId, registerUser2.UserId, registerUser1.UserId),
            };

            Given(registerUser1, registerUser2, requestConnection);
            When(secondRequestConnection);
            Then(expectedResult);
            And<User>(registerUser1.UserId, expectedEventsForUser1);
            And<User>(registerUser2.UserId, expectedEventsForUser2);
        }

        /// <summary>
        /// GIVEN User1 exists AND User2 exists AND they are already connected
        ///WHEN User1 requests a connection to User2
        ///THEN no request is created AND User1 is informed that the request failed because they are already connected
        /// </summary>
        [Test]
        public void RequestConnectionToConnectedUsersShouldFail()
        {
            Guid processId = Guid.NewGuid();
            var registerUser1 = new RegisterUser(processId, Guid.NewGuid(), 1, "user1", "email1");
            var registerUser2 = new RegisterUser(processId, Guid.NewGuid(), 2, "user2", "email2");
            var requestConnection = new RequestConnection(processId, registerUser1.UserId, registerUser1.UserId, registerUser2.UserId);
            var acceptConnection = new AcceptConnection(processId, registerUser2.UserId, registerUser2.UserId,
                registerUser1.UserId);
            var secondRequestConnection = new RequestConnection(processId, registerUser1.UserId, registerUser1.UserId, registerUser2.UserId);
            var expectedResult = new Result(User.UsersAlreadyConnected);

            var expectedEventsForUser1 = new Event[]
            {
                new UserRegistered(processId, registerUser1.UserId, registerUser1.UserName, registerUser1.PrimaryEmail),
                new ConnectionRequested(processId, registerUser1.UserId, registerUser2.UserId),
                new ConnectionCompleted(processId, registerUser1.UserId, registerUser2.UserId), 
            };

            var expectedEventsForUser2 = new Event[]
            {
                new UserRegistered(processId, registerUser2.UserId, registerUser2.UserName, registerUser2.PrimaryEmail),
                new ConnectionRequestReceived(processId, registerUser2.UserId, registerUser1.UserId),
                new ConnectionAccepted(processId, registerUser2.UserId, registerUser1.UserId), 
            };

            Given(registerUser1, registerUser2, requestConnection, acceptConnection);
            When(secondRequestConnection);
            Then(expectedResult);
            And<User>(registerUser1.UserId, expectedEventsForUser1);
            And<User>(registerUser2.UserId, expectedEventsForUser2);

        }

        /// <summary>
        /// GIVEN User1 exists
        ///WHEN User1 requests a connection to User1
        ///THEN no request is created AND User1 is informed that the request failed because they can't connect to themselves
        /// </summary>
        [Test]
        public void RequestConnectionToSelfShouldFail()
        {
            Guid processId = Guid.NewGuid();
            var registerUser1 = new RegisterUser(processId, Guid.NewGuid(), 1, "user1", "email1");
            var requestConnection = new RequestConnection(processId, registerUser1.UserId, registerUser1.UserId, registerUser1.UserId);
            var expectedResult = new Result(RequestConnectionHandler.CantConnectToSelf);

            var expectedEventsForUser1 = new Event[]
            {
                new UserRegistered(processId, registerUser1.UserId, registerUser1.UserName, registerUser1.PrimaryEmail),
            };

            Given(registerUser1);
            When(requestConnection);
            Then(expectedResult);
            And<User>(registerUser1.UserId, expectedEventsForUser1);
        }

    }
}
