using System;
using System.Text;
using EventStore.ClientAPI;
using Lending.Domain;
using Lending.Domain.Model;
using Lending.Domain.Persistence;
using Lending.Domain.RequestConnection;
using NUnit.Framework;
using ServiceStack.Text;

namespace Tests.RequestConnection
{
    /// <summary>
    /// https://github.com/joshilewis/lending/issues/6
    /// </summary>
    [TestFixture]
    public class RequestConnectionHandlerTests : FixtureWithEventStoreAndNHibernate
    {
        /// <summary>
        /// GIVEN User1 exists AND User2 exists AND they are not connected AND there is an existing connection request from User1 to User2
        ///WHEN User1 requests a connection to User2
        ///THEN no request is created AND User1 is informed that the request failed because a connection request exists and is pending
        /// </summary>
        [Test]
        public void ExistingConnectionRequestFromSourceToTargetShouldBeRejected()
        {
            Guid processId = Guid.NewGuid();
            var user1 = User.Register(processId, Guid.NewGuid(), "User 1", "email1");
            var user2 = User.Register(processId, Guid.NewGuid(), "User 2", "email2");
            user1.RequestConnectionTo(processId, user2.Id);
            SaveAggregates(user1, user2);

            var registeredUser1 = new RegisteredUser(user1.Id, user1.UserName);
            var registeredUser2 = new RegisteredUser(user2.Id, user2.UserName);
            SaveEntities(registeredUser1, registeredUser2);

            var request = new Lending.Domain.RequestConnection.RequestConnection(processId, user1.Id, user1.Id, user2.Id);
            var expectedResponse = new Result(User.ConnectionAlreadyRequested);

            var sut = new RequestConnectionForRequestingUser(() => Session, ()=> Repository, null);
            Result actualResult = sut.HandleCommand(request);
            WriteRepository();

            actualResult.ShouldEqual(expectedResponse);

            StreamEventsSlice slice = Connection.ReadStreamEventsForwardAsync($"user-{user1.Id}", 0, 10, false).Result;
            Assert.That(slice.Events.Length, Is.EqualTo(2));
        }

        /// <summary>
        /// GIVEN User1 exists AND User2 exists AND they are not connected AND there are no connection requests between them
        ///WHEN User1 requests a connection to User2
        ///THEN the request is created AND User2 is informed of the connection request
        /// </summary>
        [Test]
        public void NoExistingConnectionRequestShouldEmitEvent()
        {
            Guid processId = Guid.NewGuid();
            var user1 = User.Register(processId, Guid.NewGuid(), "User 1", "email1");
            var user2 = User.Register(processId, Guid.NewGuid(), "User 2", "email2");

            SaveAggregates(user1, user2);

            var registeredUser1 = new RegisteredUser(user1.Id, user1.UserName);
            var registeredUser2 = new RegisteredUser(user2.Id, user2.UserName);
            SaveEntities(registeredUser1, registeredUser2);
            CommitTransactionAndOpenNew();

            var request = new Lending.Domain.RequestConnection.RequestConnection(processId, user1.Id, user1.Id, user2.Id);
            var expectedResponse = new Result();
            var expectedConnectionRequestedEvent = new ConnectionRequested(processId, user1.Id, user2.Id);
            var expectedReceivedConnectionRequest = new ConnectionRequestReceived(processId, user2.Id, user1.Id);

            var sut = new RequestConnectionForRequestingUser(() => Session, () => Repository, new RequestConnectionForTargetUser(() => Session, () => Repository, null));
            Result actualResult = sut.HandleCommand(request);
            WriteRepository();
            actualResult.ShouldEqual(expectedResponse);

            StreamEventsSlice slice = Connection.ReadStreamEventsForwardAsync($"user-{user1.Id}", 0, 10, false).Result;
            Assert.That(slice.Events.Length, Is.EqualTo(2));

            var value = Encoding.UTF8.GetString(slice.Events[1].Event.Data);
            ConnectionRequested actual = value.FromJson<ConnectionRequested>();
            actual.ShouldEqual(expectedConnectionRequestedEvent);

            slice = Connection.ReadStreamEventsForwardAsync($"user-{user2.Id}", 0, 10, false).Result;
            Assert.That(slice.Events.Length, Is.EqualTo(2));

            value = Encoding.UTF8.GetString(slice.Events[1].Event.Data);
            ConnectionRequestReceived actual1 = value.FromJson<ConnectionRequestReceived>();
            actual1.ShouldEqual(expectedReceivedConnectionRequest);


        }

        /// <summary>
        /// GIVEN User1 AND User2 does not exist
        ///WHEN User1 requests a connection to User2
        ///THEN no request is created AND User1 is notified that the request failed because there is no such user
        /// </summary>
        [Test]
        public void ConnectionRequestToNonExistentTargetShouldBeRejected()
        {
            var user1 = User.Register(Guid.NewGuid(), Guid.Empty, "User 1", "email1");
            SaveAggregates(user1);

            var request = new Lending.Domain.RequestConnection.RequestConnection(Guid.NewGuid(), user1.Id, user1.Id, Guid.NewGuid());
            var expectedResponse = new Result(RequestConnectionForRequestingUser.TargetUserDoesNotExist);

            var sut = new RequestConnectionForRequestingUser(() => Session, () => Repository, null);
            Result actualResult = sut.HandleCommand(request);
            WriteRepository();
            actualResult.ShouldEqual(expectedResponse);

            StreamEventsSlice slice = Connection.ReadStreamEventsForwardAsync($"user-{user1.Id}", 0, 10, false).Result;
            Assert.That(slice.Events.Length, Is.EqualTo(1));


        }

        /// <summary>
        /// GIVEN User1 exists AND User2 exists AND they are not connected AND there is an existing connection request from User2 to User1
        ///WHEN User1 requests a connection to User2
        ///THEN no request is created AND User1 is informed that the request failed because a connection request exists AND is pending
        /// </summary>
        [Test]
        public void ExistingConnectionRequestFromTargetToSourceShouldBeRejected()
        {
            var processId = Guid.NewGuid();
            var user1 = User.Register(processId, Guid.NewGuid(), "User 1", "email1");
            var user2 = User.Register(processId, Guid.NewGuid(), "User 2", "email2");
            user1.ReceiveConnectionRequest(processId, user2.Id);
            SaveAggregates(user1, user2);

            var registeredUser1 = new RegisteredUser(user1.Id, user1.UserName);
            var registeredUser2 = new RegisteredUser(user2.Id, user2.UserName);
            SaveEntities(registeredUser1, registeredUser2);

            var request = new Lending.Domain.RequestConnection.RequestConnection(Guid.NewGuid(), user1.Id, user1.Id, user2.Id);
            var expectedResponse = new Result(User.ReverseConnectionAlreadyRequested);

            var sut = new RequestConnectionForRequestingUser(() => Session, () => Repository, new RequestConnectionForTargetUser(() => Session, () => Repository, null));
            Result actualResult = sut.HandleCommand(request);
            WriteRepository();

            actualResult.ShouldEqual(expectedResponse);

            StreamEventsSlice slice = Connection.ReadStreamEventsForwardAsync($"user-{user1.Id}", 0, 10, false).Result;
            Assert.That(slice.Events.Length, Is.EqualTo(2));
        }

        /// <summary>
        /// GIVEN User1 exists AND User2 exists AND they are already connected
        ///WHEN User1 requests a connection to User2
        ///THEN no request is created AND User1 is informed that the request failed because they are already connected
        /// </summary>
        [Test]
        public void ConnectionRequestToConnectedUsersShouldBeRejected()
        {
            var processId = Guid.NewGuid();
            var user1 = User.Register(processId, Guid.NewGuid(), "User 1", "email1");
            var user2 = User.Register(processId, Guid.NewGuid(), "User 2", "email2");
            user1.RequestConnectionTo(processId, user2.Id);
            user2.ReceiveConnectionRequest(processId, user1.Id);
            user2.AcceptReceivedConnection(processId, user1.Id);
            user1.NotifyConnectionAccepted(processId, user2.Id);
            SaveAggregates(user1, user2);

            var registeredUser1 = new RegisteredUser(user1.Id, user1.UserName);
            var registeredUser2 = new RegisteredUser(user2.Id, user2.UserName);
            SaveEntities(registeredUser1, registeredUser2);

            var request = new Lending.Domain.RequestConnection.RequestConnection(processId, user1.Id, user1.Id, user2.Id);
            var expectedResponse = new Result(User.UsersAlreadyConnected);

            var sut = new RequestConnectionForRequestingUser(() => Session, () => Repository, new RequestConnectionForTargetUser(() => Session, () => Repository, null));
            Result actualResult = sut.HandleCommand(request);
            WriteRepository();

            actualResult.ShouldEqual(expectedResponse);

            StreamEventsSlice slice = Connection.ReadStreamEventsForwardAsync($"user-{user2.Id}", 0, 10, false).Result;
            Assert.That(slice.Events.Length, Is.EqualTo(3));

            slice = Connection.ReadStreamEventsForwardAsync($"user-{user1.Id}", 0, 10, false).Result;
            Assert.That(slice.Events.Length, Is.EqualTo(3));

        }

        /// <summary>
        /// GIVEN User1 exists
        ///WHEN User1 requests a connection to User1
        ///THEN no request is created AND User1 is informed that the request failed because they can't connect to themselves
        /// </summary>
        [Test]
        public void ConnectionRequestToSelfShouldBeRejected()
        {
            var user1 = User.Register(Guid.NewGuid(), Guid.Empty, "User 1", "email1");
            SaveAggregates(user1);

            var registeredUser1 = new RegisteredUser(user1.Id, user1.UserName);
            SaveEntities(registeredUser1);

            var request = new Lending.Domain.RequestConnection.RequestConnection(Guid.NewGuid(), user1.Id, user1.Id, user1.Id);
            var expectedResponse = new Result(RequestConnectionForRequestingUser.CantConnectToSelf);

            var sut = new RequestConnectionForRequestingUser(() => Session, () => Repository, null);
            Result actualResult = sut.HandleCommand(request);
            WriteRepository();

            actualResult.ShouldEqual(expectedResponse);

            StreamEventsSlice slice = Connection.ReadStreamEventsForwardAsync($"user-{user1.Id}", 0, 10, false).Result;
            Assert.That(slice.Events.Length, Is.EqualTo(1));
        }

    }
}
