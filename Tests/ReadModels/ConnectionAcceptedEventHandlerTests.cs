using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs;
using Lending.Domain.AcceptConnection;
using Lending.ReadModels.Relational.ConnectionAccepted;
using NUnit.Framework;

namespace Tests.ReadModels
{
    [TestFixture]
    public class ConnectionAcceptedEventHandlerTests : FixtureWithEventStoreAndNHibernate
    {
        [Test]
        public void Test()
        {
            var connectionAccepted = new ConnectionAccepted(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            var userConnection = new UserConnection(connectionAccepted.ProcessId, connectionAccepted.RequestingUserId,
                connectionAccepted.AggregateId);

            var sut = new ConnectionAcceptedEventHandler(() => Session);
            sut.When(connectionAccepted);

            CommitTransactionAndOpenNew();

            var userConnections = Session.QueryOver<UserConnection>()
                .List();

            Assert.That(userConnections.Count, Is.EqualTo(1));
            userConnections[0].ShouldEqual(userConnection);

        }
    }
}
