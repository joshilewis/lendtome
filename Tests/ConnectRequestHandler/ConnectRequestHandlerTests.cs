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

            SaveEntities(new object[]{fromUser, toUser});

            CommitTransactionAndOpenNew();

            var expectedConnection = new Connection(fromUser, toUser);
            var expectedResponse = new ConnectResponse();
            var request = new ConnectRequest() {FromUserId = fromUser.Id, ToUserId = toUser.Id};

            //Act

            var sut = new Core.ConnectRequest.ConnectRequestHandler(() => Session);
            ConnectResponse actualResponse = sut.HandleRequest(request);

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
            
            SaveEntities(new object[] {fromUser, toUser, existingConnection});

            CommitTransactionAndOpenNew();

            var expectedResponse = new ConnectResponse(ConnectResponse.AlreadyConnected);
            var request = new ConnectRequest() { FromUserId = fromUser.Id, ToUserId = toUser.Id };

            //Act

            var sut = new Core.ConnectRequest.ConnectRequestHandler(() => Session);
            ConnectResponse actualResponse = sut.HandleRequest(request);

            //Assert

            actualResponse.ShouldEqual(expectedResponse);

            //Check that the connection wasn't saved in the DB
            int numberOfConnections = Session
                .QueryOver<Connection>()
                .RowCount()
                ;

            Assert.That(numberOfConnections, Is.EqualTo(1));
        }

        [Test]
        public void Test_SuccessWithExistingOtherConnection()
        {
            //Arrange
            var fromUser = new User("from", "fromEmail");
            var toUser = new User("to", "toEmail");
            var otherUser = new User("other", "otherEmail");
            var existingConnection = new Connection(otherUser, toUser);

            SaveEntities(new object[]{fromUser, toUser, otherUser, existingConnection});

            CommitTransactionAndOpenNew();

            var expectedConnection = new Connection(fromUser, toUser);
            var request = new ConnectRequest() { FromUserId = fromUser.Id, ToUserId = toUser.Id };
            var expectedResponse = new ConnectResponse();

            //Act

            var sut = new Core.ConnectRequest.ConnectRequestHandler(() => Session);
            ConnectResponse actualResponse = sut.HandleRequest(request);

            //Assert

            actualResponse.ShouldEqual(expectedResponse);

            //Check that the connection was saved in the DB
            CommitTransactionAndOpenNew();

            Connection connectionAlias = null;
            User user1Alias = null;
            User user2Alias = null;

            Connection actualConnection = Session
                .QueryOver<Connection>(() => connectionAlias)
                .JoinAlias(() => connectionAlias.User1, () => user1Alias)
                .JoinAlias(() => connectionAlias.User2, () => user2Alias)
                .Where(() => (user1Alias.Id == request.FromUserId && user2Alias.Id == request.ToUserId) ||
                             (user1Alias.Id == request.ToUserId && user2Alias.Id == request.FromUserId))
                .SingleOrDefault()
                ;

            actualConnection.ShouldEqual(expectedConnection);
        }

    }
}
