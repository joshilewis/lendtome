using System;
using Lending.Cqrs;
using Lending.Domain.Model;
using NHibernate;

namespace Lending.Domain.AcceptConnection
{
    public class AcceptConnectionForAcceptingUser : AuthenticatedCommandHandler<AcceptConnection, Result>
    {
        public AcceptConnectionForAcceptingUser(Func<ISession> getSession, Func<IRepository> getRepository,
            ICommandHandler<AcceptConnection, Result> nextHandler) : base(getSession, getRepository, nextHandler)
        {
        }

        public override Result HandleCommand(AcceptConnection command)
        {
            User user = User.CreateFromHistory(Repository.GetEventsForAggregate<User>(command.AggregateId));

            Result result = user.AcceptReceivedConnection(command.ProcessId, command.RequestingUserId);

            if (!result.Success) return result;

            Repository.Save(user);

            if (NextHandler == null) return result;

            return NextHandler.HandleCommand(command);
        }
    }
}