using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using EventStore.ClientAPI;
using Lending.Core;
using Lending.Core.ConnectionRequest;
using Lending.Core.Model;
using Lending.Core.NewUser;
using Lending.Execution.EventStore;
using NUnit.Framework;
using ServiceStack.Text;

namespace Tests.Connect
{
    [TestFixture]
    public class ConnectRequestHandlerTests : DatabaseAndEventStoreFixtureBase
    {
        /// <summary>
        /// GIVEN User1 exists AND User2 exists AND they are not connected AND there is an existing connection request from User1 to User2
        ///WHEN User1 requests a connection to User2
        ///THEN no request is created AND User1 is informed that the request failed because a connection request exists and is pending
        /// </summary>
        [Test]
        public void ExistingConnectionRequestFrom1To2ShouldBeRejected()
        {
            var user1Added = new UserAdded(1L, "User 1", "email1");
            var user2Added = new UserAdded(2L, "User 2", "email2");

            var connectionRequested = new ConnectionRequested(Guid.NewGuid(), 1, 2);

            var request = new ConnectionRequest((long)user1Added.Id, (long)user2Added.Id);
            var expectedResponse = new BaseResponse(ConnectionRequestHandler.ConnectionAlreadyRequested);

            EventStoreRepository eventEmitter = new EventStoreRepository(new ConcurrentQueue<StreamEventTuple>());

            var sut = new ConnectionRequestHandler(eventEmitter);
            BaseResponse actualResponse = sut.HandleRequest(request);

            actualResponse.ShouldEqual(expectedResponse);

            ConcurrentQueue<StreamEventTuple> actualQueue = eventEmitter.Queue;
            Assert.That(actualQueue, Is.Empty);
        }

        /// <summary>
        /// GIVEN User1 exists AND User2 exists AND they are not connected AND there are no connection requests between them
        ///WHEN User1 requests a connection to User2
        ///THEN the request is created AND User2 is informed of the connection request
        /// </summary>
        [Test]
        public void NoExistingConnectionRequestShouldEmitEvent()
        {
            var user1Added = new UserAdded(1, "User 1", "email1");
            var user2Added = new UserAdded(2, "User 2", "email2");

            var stream = "User-" + user1Added.Id;
            WriteEvents(new StreamEventTuple(stream, user1Added), new StreamEventTuple("User-2", user2Added));

            var request = new ConnectionRequest(1, 2);
            var expectedResponse = new BaseResponse();
            var expectedEvent = new ConnectionRequested(Guid.Empty, 1, 2);

            var sut = new ConnectionRequestHandler(Emitter);
            BaseResponse actualResponse = sut.HandleRequest(request);
            WriteEmittedEvents();
            actualResponse.ShouldEqual(expectedResponse);

            StreamEventsSlice slice = Connection.ReadStreamEventsForwardAsync(stream, 0, 10, false).Result;
            Assert.That(slice.Events.Length, Is.EqualTo(2));

            var value = Encoding.UTF8.GetString(slice.Events[1].Event.Data);
            ConnectionRequested actual = value.FromJson<ConnectionRequested>();
            actual.ShouldEqual(expectedEvent);
        }
    }
}
