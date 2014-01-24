using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;
using NHibernate;
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
    }

    public class ConnectRequestHandler
    {
        private readonly Func<ISession> getSession;

        public ConnectRequestHandler(Func<ISession> sessionFunc)
        {
            this.getSession = sessionFunc;
        }

        protected ConnectRequestHandler() { }


        public virtual ConnectResponse HandleConnectRequest(ConnectRequest request)
        {
            ISession session = getSession();
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
    }

    public class ConnectRequest
    {
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }
    }

    public class ConnectResponse
    {
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
