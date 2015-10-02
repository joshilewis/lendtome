using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using EventStore.ClientAPI;
using Lending.Domain;
using Lending.Domain.ConnectionRequest;
using Lending.Domain.NewUser;
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
            var user1Added = new UserAdded(Guid.NewGuid(), "User 1", "email1");
            var user2Added = new UserAdded(Guid.NewGuid(), "User 2", "email2");

            var connectionRequested = new ConnectionRequested(Guid.NewGuid(), user1Added.Id, user2Added.Id);

            var request = new ConnectionRequest(user1Added.Id, user2Added.Id);
            var expectedResponse = new BaseResponse(ConnectionRequestHandler.ConnectionAlreadyRequested);

            var sut = new ConnectionRequestHandler(Repository);
            BaseResponse actualResponse = sut.HandleRequest(request);

            actualResponse.ShouldEqual(expectedResponse);

        }

        /// <summary>
        /// GIVEN User1 exists AND User2 exists AND they are not connected AND there are no connection requests between them
        ///WHEN User1 requests a connection to User2
        ///THEN the request is created AND User2 is informed of the connection request
        /// </summary>
        [Test]
        public void NoExistingConnectionRequestShouldEmitEvent()
        {
            var user1Added = new UserAdded(Guid.NewGuid(), "User 1", "email1");
            var user2Added = new UserAdded(Guid.NewGuid(), "User 2", "email2");

            var stream = "User-" + user1Added.Id;
            WriteEvents(new StreamEventTuple(stream, user1Added), new StreamEventTuple("User-2", user2Added));

            var request = new ConnectionRequest(user1Added.Id, user2Added.Id);
            var expectedResponse = new BaseResponse();
            var expectedEvent = new ConnectionRequested(Guid.Empty, user1Added.Id, user2Added.Id);

            var sut = new ConnectionRequestHandler(Repository);
            BaseResponse actualResponse = sut.HandleRequest(request);
            WriteAggregates();
            actualResponse.ShouldEqual(expectedResponse);

            StreamEventsSlice slice = Connection.ReadStreamEventsForwardAsync(stream, 0, 10, false).Result;
            Assert.That(slice.Events.Length, Is.EqualTo(2));

            var value = Encoding.UTF8.GetString(slice.Events[1].Event.Data);
            ConnectionRequested actual = value.FromJson<ConnectionRequested>();
            actual.ShouldEqual(expectedEvent);
        }
    }
}
