using System;
using Lending.Domain.Model;
using NHibernate;

namespace Lending.Domain.AcceptConnection
{
    public class AcceptConnectionForRequestingUser : AuthenticatedCommandHandler<AcceptConnection, Result>
    {
        public AcceptConnectionForRequestingUser(Func<ISession> getSession, Func<IRepository> getRepository,
            ICommandHandler<AcceptConnection, Result> nextHandler) : base(getSession, getRepository, nextHandler)
        {
        }

        public override Result HandleCommand(AcceptConnection command)
        {
            User user = User.CreateFromHistory(Repository.GetEventsForAggregate<User>(command.RequestingUserId));

            Result result = user.NotifyConnectionAccepted(command.ProcessId, command.AggregateId);

            if (!result.Success) return result;

            Repository.Save(user);

            if (NextHandler == null) return result;

            return NextHandler.HandleCommand(command);
        }
    }
}