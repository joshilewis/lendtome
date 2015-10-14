using System;
using Lending.Cqrs;
using Lending.Domain.Model;
using Lending.Domain.Persistence;
using NHibernate;

namespace Lending.Domain.RequestConnection
{
    public class RequestConnectionForRequestingUser : AuthenticatedCommandHandler<RequestConnection, Result>
    {
        public const string TargetUserDoesNotExist = "The target user does not exist";
        public const string CantConnectToSelf = "You can't connect to yourself";

        public RequestConnectionForRequestingUser(Func<ISession> getSession, Func<IRepository> getRepository,
            ICommandHandler<RequestConnection, Result> nextHandler) : base(getSession, getRepository, nextHandler)
        {
        }

        public override Result HandleCommand(RequestConnection command)
        {
            if (command.TargetUserId == command.AggregateId) return new Result(CantConnectToSelf);

            RegisteredUser targetUser = Session.Get<RegisteredUser>(command.TargetUserId);
            if (targetUser == null) return new Result(TargetUserDoesNotExist);

            User user = User.CreateFromHistory(Repository.GetEventsForAggregate<User>(command.AggregateId));

            Result  result = user.RequestConnectionTo(command.ProcessId, command.TargetUserId);

            if (!result.Success) return result;

            Repository.Save(user);

            if (NextHandler == null) return result;

            return NextHandler.HandleCommand(command);
        }

    }
}