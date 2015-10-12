using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using EventStore.ClientAPI;
using Lending.Domain;
using Lending.Domain.ConnectionRequest;
using Lending.Domain.Model;
using Lending.Domain.Persistence;
using Lending.Execution.Auth;
using Lending.Execution.EventStore;
using NUnit.Framework;
using ServiceStack.Text;

namespace Tests.Connect
{
    [TestFixture]
    public class ConnectionRequestHandlerTests : DatabaseAndEventStoreFixtureBase
    {
        /// <summary>
        /// GIVEN User1 exists AND User2 exists AND they are not connected AND there is an existing connection request from User1 to User2
        ///WHEN User1 requests a connection to User2
        ///THEN no request is created AND User1 is informed that the request failed because a connection request exists and is pending
        /// </summary>
        [Test]
        public void ExistingConnectionRequestFromSourceToTargetShouldBeRejected()
        {
            var user1 = User.Register(Guid.NewGuid(), "User 1", "email1");
            var user2 = User.Register(Guid.NewGuid(), "User 2", "email2");
            user1.RequestConnectionTo(user2.Id);
            SaveAggregates(user1, user2);

            var registeredUser1 = new RegisteredUser(user1.Id, user1.UserName);
            var registeredUser2 = new RegisteredUser(user2.Id, user2.UserName);
            SaveEntities(registeredUser1, registeredUser2);

            var request = new ConnectionRequest(user2.Id);
            var expectedResponse = new BaseResponse(ConnectionRequestHandler.ConnectionAlreadyRequested);

            var sut = new ConnectionRequestHandler(() => Session, ()=> Repository);
            BaseResponse actualResponse = sut.HandleRequest(request, user1.Id);
            WriteRepository();

            actualResponse.ShouldEqual(expectedResponse);

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
            var user1 = User.Register(Guid.NewGuid(), "User 1", "email1");
            var user2 = User.Register(Guid.NewGuid(), "User 2", "email2");

            SaveAggregates(user1, user2);

            var registeredUser1 = new RegisteredUser(user1.Id, user1.UserName);
            var registeredUser2 = new RegisteredUser(user2.Id, user2.UserName);
            SaveEntities(registeredUser1, registeredUser2);
            CommitTransactionAndOpenNew();

            var request = new ConnectionRequest(user2.Id);
            var expectedResponse = new BaseResponse();
            var expectedEvent = new ConnectionRequested(Guid.Empty, user1.Id, user2.Id);
            var expectedConnectionRequest = new PendingConnectionRequest(user1.Id, user2.Id);

            var sut = new ConnectionRequestHandler(() => Session, () => Repository);
            BaseResponse actualResponse = sut.HandleRequest(request, user1.Id);
            WriteRepository();
            actualResponse.ShouldEqual(expectedResponse);

            StreamEventsSlice slice = Connection.ReadStreamEventsForwardAsync($"user-{user1.Id}", 0, 10, false).Result;
            Assert.That(slice.Events.Length, Is.EqualTo(2));

            var value = Encoding.UTF8.GetString(slice.Events[1].Event.Data);
            ConnectionRequested actual = value.FromJson<ConnectionRequested>();
            actual.ShouldEqual(expectedEvent);

            PendingConnectionRequest actualConnectionRequest = Session.Get<PendingConnectionRequest>(user1.Id);
            actualConnectionRequest.ShouldEqual(expectedConnectionRequest);
        }

        /// <summary>
        /// GIVEN User1 AND User2 does not exist
        ///WHEN User1 requests a connection to User2
        ///THEN no request is created AND User1 is notified that the request failed because there is no such user
        /// </summary>
        [Test]
        public void ConnectionRequestToNonExistentTargetShouldBeRejected()
        {
            var user1 = User.Register(Guid.NewGuid(), "User 1", "email1");
            SaveAggregates(user1);

            var request = new ConnectionRequest(Guid.NewGuid());
            var expectedResponse = new BaseResponse(ConnectionRequestHandler.TargetUserDoesNotExist);

            var sut = new ConnectionRequestHandler(() => Session, () => Repository);
            BaseResponse actualResponse = sut.HandleRequest(request, user1.Id);
            WriteRepository();
            actualResponse.ShouldEqual(expectedResponse);

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
            var user1 = User.Register(Guid.NewGuid(), "User 1", "email1");
            var user2 = User.Register(Guid.NewGuid(), "User 2", "email2");
            user2.RequestConnectionTo(user1.Id);
            SaveAggregates(user1, user2);

            var registeredUser1 = new RegisteredUser(user1.Id, user1.UserName);
            var registeredUser2 = new RegisteredUser(user2.Id, user2.UserName);
            SaveEntities(registeredUser1, registeredUser2, new PendingConnectionRequest(user2.Id, user1.Id));

            var request = new ConnectionRequest(user2.Id);
            var expectedResponse = new BaseResponse(ConnectionRequestHandler.ReverseConnectionAlreadyRequested);

            var sut = new ConnectionRequestHandler(() => Session, () => Repository);
            BaseResponse actualResponse = sut.HandleRequest(request, user1.Id);
            WriteRepository();

            actualResponse.ShouldEqual(expectedResponse);

            StreamEventsSlice slice = Connection.ReadStreamEventsForwardAsync($"user-{user1.Id}", 0, 10, false).Result;
            Assert.That(slice.Events.Length, Is.EqualTo(1));
        }

        /// <summary>
        /// GIVEN User1 exists AND User2 exists AND they are already connected
        ///WHEN User1 requests a connection to User2
        ///THEN no request is created AND User1 is informed that the request failed because they are already connected
        /// </summary>
        [Test]
        public void ConnectionRequestToConnectedUsersShouldBeRejected()
        {
            
        }
    }
}
