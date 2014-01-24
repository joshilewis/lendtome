using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;
using NHibernate;
using NHibernate.Criterion;
using NUnit.Framework;

namespace Tests.ConnectRequestHandler
{
    [TestFixture]
    public class ConnectRequestHandlerTests : DatabaseFixtureBase
    {

        [Test]
        public void Test_Success()
        {
            //Arrange
            var fromUser = new User("from", "fromEmail");
            var toUser = new User("to", "toEmail");

            Session.Save(fromUser);
            Session.Save(toUser);

            CommitTransactionAndOpenNew();

            var expectedConnection = new Connection(fromUser, toUser);
            var expectedResponse = new ConnectResponse();
            var request = new ConnectRequest() {FromUserId = fromUser.Id, ToUserId = toUser.Id};

            //Act

            var sut = new ConnectRequestHandler(() => Session);
            ConnectResponse actualResponse = sut.HandleConnectRequest(request);

            //Assert

            actualResponse.ShouldEqual(expectedResponse);

            //Check that the connection was saved in the DB
            CommitTransactionAndOpenNew();

            Connection actualConnection = Session
                .QueryOver<Connection>()
                .SingleOrDefault()
                ;

            actualConnection.ShouldEqual(expectedConnection);
        }

        [Test]
        public void Test_AlreadyConnected()
        {
            //Arrange
            var fromUser = new User("from", "fromEmail");
            var toUser = new User("to", "toEmail");
            var existingConnection = new Connection(fromUser, toUser);
            
            Session.Save(fromUser);
            Session.Save(toUser);
            Session.Save(existingConnection);

            CommitTransactionAndOpenNew();

            var expectedResponse = new ConnectResponse(ConnectResponse.AlreadyConnected);
            var request = new ConnectRequest() { FromUserId = fromUser.Id, ToUserId = toUser.Id };

            //Act

            var sut = new ConnectRequestHandler(() => Session);
            ConnectResponse actualResponse = sut.HandleConnectRequest(request);

            //Assert

            actualResponse.ShouldEqual(expectedResponse);

            //Check that the connection wasn't saved in the DB
            int numberOfConnections = Session
                .QueryOver<Connection>()
                .RowCount()
                ;

            Assert.That(numberOfConnections, Is.EqualTo(1));
        }

    }

    public class ConnectRequestHandler
    {
        private readonly Func<ISession> getSession;

        public ConnectRequestHandler(Func<ISession> sessionFunc)
        {
            this.getSession = sessionFunc;
        }

        protected ConnectRequestHandler()
        {
        }


        public virtual ConnectResponse HandleConnectRequest(ConnectRequest request)
        {
            ISession session = getSession();

            if (ConnectionAlreadyExists(request))
                return new ConnectResponse(ConnectResponse.AlreadyConnected);

            User user1 = session
                .Get<User>(request.FromUserId)
                ;

            User user2 = session
                .Get<User>(request.ToUserId)
                ;

            var connection = new Connection(user1, user2);

            session.Save(connection);

            return new ConnectResponse();
        }

        private bool ConnectionAlreadyExists(ConnectRequest request)
        {
            ISession session = getSession();
            Connection connectionAlias = null;
            User user1Alias = null;
            User user2Alias = null;

            int numberOfExistingConnections = session
                .QueryOver<Connection>(() => connectionAlias)
                .JoinAlias(() => connectionAlias.User1, () => user1Alias)
                .JoinAlias(() => connectionAlias.User2, () => user2Alias)
                .Where(() =>
                    (user1Alias.Id == request.FromUserId && user2Alias.Id == request.ToUserId) ||
                    (user1Alias.Id == request.ToUserId && user2Alias.Id == request.FromUserId))
                .RowCount()
                ;

            return numberOfExistingConnections > 0;
        }
    }


    public class ConnectRequest
    {
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }
    }

    public class ConnectResponse
    {
        public const string AlreadyConnected = "The Users are already connected.";

        public bool Success { get; set; }
        public string FailureDescription { get; set; }

        public ConnectResponse()
        {
            Success = true;
            FailureDescription = null;
        }

        public ConnectResponse(string failureDescription)
        {
            FailureDescription = failureDescription;
        }
    }
}
