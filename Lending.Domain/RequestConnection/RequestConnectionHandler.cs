using System;
using Lending.Domain.Model;
using Lending.Domain.Persistence;
using NHibernate;

namespace Lending.Domain.RequestConnection
{
    public class RequestConnectionHandler : AuthenticatedCommandHandler<RequestConnection, BaseResponse>
    {
        public const string ConnectionAlreadyRequested = "A connection request for these users already exists";
        public const string TargetUserDoesNotExist = "The target user does not exist";
        public const string ReverseConnectionAlreadyRequested = "A reverse connection request for these users already exists";
        public const string CantConnectToSelf = "You can't connect to yourself";

        public RequestConnectionHandler(Func<ISession> sessionFunc, Func<IRepository> repositoryFunc)
            : base(sessionFunc, repositoryFunc)
        {
        }

        public override BaseResponse HandleCommand(RequestConnection command)
        {
            if (command.TargetUserId == command.UserId) return new BaseResponse(CantConnectToSelf);

            RegisteredUser targetUser = Session.Get<RegisteredUser>(command.TargetUserId);
            if (targetUser == null) return new BaseResponse(TargetUserDoesNotExist);

            PendingConnectionRequest reverseRequest = Session.Get<PendingConnectionRequest>(command.TargetUserId);
            if (reverseRequest != null) return new BaseResponse(ReverseConnectionAlreadyRequested);

            User user = User.CreateFromHistory(Repository.GetEventsForAggregate<User>(command.UserId));
            bool connectionRequestSuccessful = user.RequestConnectionTo(command.TargetUserId);

            if (!connectionRequestSuccessful)
                return new BaseResponse(ConnectionAlreadyRequested);

            Repository.Save(user);
            Session.Save(new PendingConnectionRequest(command.UserId, command.TargetUserId));

            return new BaseResponse();
        }

    }
}