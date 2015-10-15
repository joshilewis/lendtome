using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Lending.Cqrs;
using Lending.Domain;
using Lending.Domain.AcceptConnection;
using Lending.Domain.Model;
using Lending.Domain.Persistence;
using Lending.Domain.RequestConnection;
using NUnit.Framework;
using ServiceStack.Text;

namespace Tests.AcceptConnection
{
    /// <summary>
    /// https://github.com/joshilewis/lending/issues/7
    /// </summary>
    [TestFixture]
    public class AcceptConnectionHandlerTests : FixtureWithEventStoreAndNHibernate
    {
        /// <summary>
        /// GIVEN User1 exists AND User2 exists AND they are not connected AND User1 has requested to Connect to User2
        ///WHEN User2 accepts the connection request from User1
        ///THEN User1 and User2 are connected
        /// </summary>
        [Test]
        public void AcceptConnectionForUnconnectedUsersWithNoPendingRequestsShouldSucceed()
        {
            var processId = Guid.NewGuid();
            var user1 = User.Register(processId, Guid.NewGuid(), "User 1", "email1");
            var user2 = User.Register(processId, Guid.NewGuid(), "User 2", "email2");
            user1.RequestConnection(processId, user2.Id);
            user2.InitiateConnectionAcceptance(processId, user1.Id);
            SaveAggregates(user1, user2);

            var registeredUser1 = new RegisteredUser(user1.Id, user1.UserName);
            var registeredUser2 = new RegisteredUser(user2.Id, user2.UserName);
            SaveEntities(registeredUser1, registeredUser2);

            var request = new Lending.Domain.AcceptConnection.AcceptConnection(processId, user2.Id, user2.Id, user1.Id);
            var expectedResponse = new Result();
            var expectedReceivedConnectionAccepted = new ConnectionAccepted(processId, user2.Id, user1.Id);
            var expectedRequestedConnectionAccepted = new ConnectionCompleted(processId, user1.Id, user2.Id);

            var sut = new AcceptConnectionHandler(() => Session, () => Repository);
            Result actualResult = sut.HandleCommand(request);
            WriteRepository();

            actualResult.ShouldEqual(expectedResponse);

            StreamEventsSlice slice = Connection.ReadStreamEventsForwardAsync($"user-{user2.Id}", 0, 10, false).Result;
            Assert.That(slice.Events.Length, Is.EqualTo(3));
            var value = Encoding.UTF8.GetString(slice.Events[2].Event.Data);
            ConnectionAccepted actual = value.FromJson<ConnectionAccepted>();
            actual.ShouldEqual(expectedReceivedConnectionAccepted);

            slice = Connection.ReadStreamEventsForwardAsync($"user-{user1.Id}", 0, 10, false).Result;
            Assert.That(slice.Events.Length, Is.EqualTo(3));
            value = Encoding.UTF8.GetString(slice.Events[2].Event.Data);
            ConnectionCompleted actual1 = value.FromJson<ConnectionCompleted>();
            actual1.ShouldEqual(expectedRequestedConnectionAccepted);

        }

        /// <summary>
        ///GIVEN User1 exists AND User2 exists AND they are not connected AND there are no Connection Requests between them
        ///WHEN User2 accepts the connection request from User1
        ///THEN No connection is made and User2 is informed that the acceptance failed because there is no Connection Request between them
        /// </summary>
        [Test]
        public void AcceptConnectionWithNoPendingRequestShouldFail()
        {
            var processId = Guid.NewGuid();
            var user1 = User.Register(processId, Guid.NewGuid(), "User 1", "email1");
            var user2 = User.Register(processId, Guid.NewGuid(), "User 2", "email2");
            SaveAggregates(user1, user2);

            var registeredUser1 = new RegisteredUser(user1.Id, user1.UserName);
            var registeredUser2 = new RegisteredUser(user2.Id, user2.UserName);
            SaveEntities(registeredUser1, registeredUser2);

            var request = new Lending.Domain.AcceptConnection.AcceptConnection(processId, user1.Id, user1.Id, user2.Id);
            var expectedResponse = new Result(User.ConnectionRequestNotReceived);

            var sut = new AcceptConnectionHandler(() => Session, () => Repository);
            Result actualResult = sut.HandleCommand(request);
            WriteRepository();

            actualResult.ShouldEqual(expectedResponse);

            StreamEventsSlice slice = Connection.ReadStreamEventsForwardAsync($"user-{user1.Id}", 0, 10, false).Result;
            Assert.That(slice.Events.Length, Is.EqualTo(1));
        }

        /// <summary>
        ///GIVEN User1 exists AND User2 exists AND they are connected
        ///WHEN User2 accepts the connection request from User1
        ///THEN No connection is made and User2 is informed that the acceptance failed because they are already connected
        /// </summary>
        [Test]
        public void AcceptConnectionForConnectedUsersShouldFail()
        {
            var processId = Guid.NewGuid();
            var user1 = User.Register(processId, Guid.NewGuid(), "User 1", "email1");
            var user2 = User.Register(processId, Guid.NewGuid(), "User 2", "email2");
            user1.RequestConnection(processId, user2.Id);
            user2.InitiateConnectionAcceptance(processId, user1.Id);
            user2.AcceptConnection(processId, user1.Id);
            user1.CompleteConnection(processId, user2.Id);
            SaveAggregates(user1, user2);

            var registeredUser1 = new RegisteredUser(user1.Id, user1.UserName);
            var registeredUser2 = new RegisteredUser(user2.Id, user2.UserName);
            SaveEntities(registeredUser1, registeredUser2);

            var request = new Lending.Domain.AcceptConnection.AcceptConnection(processId, user2.Id, user2.Id, user1.Id);
            var expectedResponse = new Result(User.UsersAlreadyConnected);

            var sut = new AcceptConnectionHandler(() => Session, () => Repository);
            Result actualResult = sut.HandleCommand(request);
            WriteRepository();

            actualResult.ShouldEqual(expectedResponse);

            StreamEventsSlice slice = Connection.ReadStreamEventsForwardAsync($"user-{user2.Id}", 0, 10, false).Result;
            Assert.That(slice.Events.Length, Is.EqualTo(3));

            slice = Connection.ReadStreamEventsForwardAsync($"user-{user1.Id}", 0, 10, false).Result;
            Assert.That(slice.Events.Length, Is.EqualTo(3));

        }

    }
}
