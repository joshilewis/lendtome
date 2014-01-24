using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.ConnectRequest;
using Core.Model;
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

            var sut = new Core.ConnectRequest.ConnectRequestHandler(() => Session);
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

            var sut = new Core.ConnectRequest.ConnectRequestHandler(() => Session);
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
}
