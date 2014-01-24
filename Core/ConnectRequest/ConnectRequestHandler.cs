using System;
using Core.Model;
using NHibernate;

namespace Core.ConnectRequest
{
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
}