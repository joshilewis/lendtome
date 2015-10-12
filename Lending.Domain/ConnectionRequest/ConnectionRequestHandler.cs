using System;
using Lending.Domain.Model;
using Lending.Domain.Persistence;
using NHibernate;

namespace Lending.Domain.ConnectionRequest
{
    public class ConnectionRequestHandler : BaseAuthenticatedRequestHandler<ConnectionRequest, BaseResponse>
    {
        public const string ConnectionAlreadyRequested = "A connection request for these users already exists";
        public const string TargetUserDoesNotExist = "The target user does not exist";
        public const string ReverseConnectionAlreadyRequested = "A reverse connection request for these users already exists";

        public ConnectionRequestHandler(Func<ISession> sessionFunc, Func<IRepository> repositoryFunc)
            : base(sessionFunc, repositoryFunc)
        {
        }

        public override BaseResponse HandleRequest(ConnectionRequest request)
        {
            RegisteredUser targetUser = Session.Get<RegisteredUser>(request.TargetUserId);
            if (targetUser == null) return new BaseResponse(TargetUserDoesNotExist);

            PendingConnectionRequest reverseRequest = Session.Get<PendingConnectionRequest>(request.TargetUserId);
            if (reverseRequest != null) return new BaseResponse(ReverseConnectionAlreadyRequested);

            User user = User.CreateFromHistory(Repository.GetEventsForAggregate<User>(request.UserId));
            bool connectionRequestSuccessful = user.RequestConnectionTo(request.TargetUserId);

            if (!connectionRequestSuccessful)
                return new BaseResponse(ConnectionAlreadyRequested);

            Repository.Save(user);
            Session.Save(new PendingConnectionRequest(request.UserId, request.TargetUserId));

            return new BaseResponse();
        }

    }
}