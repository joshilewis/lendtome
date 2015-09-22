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
        //[Test]
        //public void ExistingConnectionRequestFrom1To2ShouldDisallowRequest()
        //{
        //    var user1Added = new UserAdded(1, "User 1", "email1");
        //    var user2Added = new UserAdded(2, "User 2", "email2");

        //    var connectionRequested = new ConnectionRequested(1, 2);

        //    WriteEvents(user1Added, user2Added, connectionRequested);

        //    var request = new ConnectRequest(user1Added.Id, user2Added.Id);
        //    var expectedResponse = new BaseResponse(ConnectRequestHandler.ConnectionAlreadyRequested);

        //    var sut = new ConnectRequestHandler();
        //    BaseResponse actualResponse = sut.HandleRequest(request);

        //    actualResponse.ShouldEqual(expectedResponse);

        //    StreamEventsSlice slice = Connection.ReadStreamEventsBackwardAsync("UserAdded-" + authDto.Id, 0, 10, false).Result;
        //    Assert.That(slice.Events.Count(), Is.EqualTo(1));

        //    var value = Encoding.UTF8.GetString(slice.Events[0].Event.Data);
        //    UserAdded actual = value.FromJson<UserAdded>();
        //    actual.ShouldEqual(expectedEvent);
        //}

        [Test]
        public void NoExistingConnectionRequestShouldEmitEvent()
        {
            var user1Added = new UserAdded(1, "User 1", "email1");
            var user2Added = new UserAdded(2, "User 2", "email2");

            //WriteEvents(new StreamEventTuple("User-1", user1Added), new StreamEventTuple("User-2", user2Added));

            var request = new ConnectionRequest((long)user1Added.Id, (long)user2Added.Id);
            var expectedResponse = new BaseResponse();
            var expectedEvent = new ConnectionRequested(Guid.Empty, (long)user1Added.Id, (long)user2Added.Id);
            var expectedStream = "User-" + user1Added.Id;

            EventStoreEventEmitter eventEmitter = new EventStoreEventEmitter(new ConcurrentQueue<StreamEventTuple>());

            var sut = new ConnectionRequestHandler(eventEmitter);
            BaseResponse actualResponse = sut.HandleRequest(request);

            actualResponse.ShouldEqual(expectedResponse);

            ConcurrentQueue<StreamEventTuple> actualQueue = eventEmitter.Queue;

            Assert.That(actualQueue.Count, Is.EqualTo(1));
            StreamEventTuple tuple = actualQueue.First();
            Assert.That(tuple.Stream, Is.EqualTo(expectedStream));
            ((ConnectionRequested)tuple.Event).ShouldEqual(expectedEvent);

            //StreamEventsSlice slice = Connection.ReadStreamEventsForwardAsync("User-1", 0, 10, false).Result;
            //Assert.That(slice.Events.Count(), Is.EqualTo(3));

            //var value = Encoding.UTF8.GetString(slice.Events[2].Event.Data);
            //ConnectionRequested actual = value.FromJson<ConnectionRequested>();
            //actual.ShouldEqual(expectedEvent);
        }
    }
}
