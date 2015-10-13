using System;
using Lending.Domain.Model;
using NHibernate;

namespace Lending.Domain.AcceptConnection
{
    public class AcceptConnectionForAcceptingUser : AuthenticatedCommandHandler<AcceptConnection, Response>
    {
        public AcceptConnectionForAcceptingUser(Func<ISession> getSession, Func<IRepository> getRepository,
            ICommandHandler<AcceptConnection, Response> nextHandler) : base(getSession, getRepository, nextHandler)
        {
        }

        public override Response HandleCommand(AcceptConnection command)
        {
            User user = User.CreateFromHistory(Repository.GetEventsForAggregate<User>(command.AggregateId));

            Response response = user.AcceptReceivedConnection(command.ProcessId, command.RequestingUserId);

            if (!response.Success) return response;

            Repository.Save(user);

            if (NextHandler == null) return response;

            return NextHandler.HandleCommand(command);
        }
    }
}