using System;
using Lending.Core.Model;
using NHibernate;

namespace Lending.Core.Connect
{
    public class ConnectRequestHandler : IRequestHandler<ConnectRequest, BaseResponse>
    {
        public const string AlreadyConnected = "The Users are already connected.";

        private readonly Func<ISession> getSession;

        public ConnectRequestHandler(Func<ISession> sessionFunc)
        {
            this.getSession = sessionFunc;
        }

        protected ConnectRequestHandler() { }

        public virtual BaseResponse HandleRequest(ConnectRequest userAuthIdString)
        {
            ISession session = getSession();

            if (ConnectionAlreadyExists(userAuthIdString))
                return new BaseResponse(AlreadyConnected);

            User user1 = session
                .Get<User>(userAuthIdString.FromUserId)
                ;

            User user2 = session
                .Get<User>(userAuthIdString.ToUserId)
                ;

            var connection = new Connection(user1, user2);

            session.Save(connection);

            return new BaseResponse();
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