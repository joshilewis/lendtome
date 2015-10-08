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

        public ConnectionRequestHandler(Func<ISession> sessionFunc, IRepository repository)
            : base(sessionFunc, repository)
        {
        }

        public override BaseResponse HandleRequest(ConnectionRequest request, Guid userId)
        {
            RegisteredUser targetUser = Session.Get<RegisteredUser>(request.TargetUserId);
            if (targetUser == null) return new BaseResponse(TargetUserDoesNotExist);

            User user = User.CreateFromHistory(Repository.GetEventsForAggregate<User>(userId));
            bool connectionRequestSuccessful = user.RequestConnectionTo(request.TargetUserId);

            if (!connectionRequestSuccessful)
                return new BaseResponse(ConnectionAlreadyRequested);

            Repository.Save(user);

            return new BaseResponse();
        }

    }
}