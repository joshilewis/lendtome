using System;
using Lending.Cqrs;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Domain.Model;
using Lending.Domain.RegisterUser;

namespace Lending.Domain.RequestConnection
{
    public class RequestConnectionHandler : AuthenticatedCommandHandler<RequestConnection, Result>
    {
        public const string TargetUserDoesNotExist = "The target user does not exist";
        public const string CantConnectToSelf = "You can't connect to yourself";

        public RequestConnectionHandler(Func<IRepository> repositoryFunc, Func<IEventRepository> eventRepositoryFunc)
            : base(repositoryFunc, eventRepositoryFunc)
        {
        }

        public override Result Handle(RequestConnection command)
        {
            if (command.TargetUserId == command.AggregateId) return new Result(CantConnectToSelf);

            RegisteredUser registeredTargetUser = Session.Get<RegisteredUser>(command.TargetUserId);
            if (registeredTargetUser == null) return new Result(TargetUserDoesNotExist);

            User user = User.CreateFromHistory(EventRepository.GetEventsForAggregate<User>(command.AggregateId));
            Result  result = user.RequestConnection(command.ProcessId, command.TargetUserId);

            if (!result.Success) return result;

            User targetUser = User.CreateFromHistory(EventRepository.GetEventsForAggregate<User>(command.TargetUserId));
            result = targetUser.InitiateConnectionAcceptance(command.ProcessId, command.AggregateId);

            if (!result.Success) return result;

            EventRepository.Save(user);
            EventRepository.Save(targetUser);

            return Success();
        }

    }
}