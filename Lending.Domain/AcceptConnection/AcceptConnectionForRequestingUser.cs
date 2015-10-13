using System;
using Lending.Domain.Model;
using NHibernate;

namespace Lending.Domain.AcceptConnection
{
    public class AcceptConnectionForRequestingUser : AuthenticatedCommandHandler<AcceptConnection, Response>
    {
        public AcceptConnectionForRequestingUser(Func<ISession> getSession, Func<IRepository> getRepository,
            ICommandHandler<AcceptConnection, Response> nextHandler) : base(getSession, getRepository, nextHandler)
        {
        }

        public override Response HandleCommand(AcceptConnection command)
        {
            User user = User.CreateFromHistory(Repository.GetEventsForAggregate<User>(command.RequestingUserId));

            Response response = user.NotifyConnectionAccepted(command.ProcessId, command.AggregateId);

            if (!response.Success) return response;

            Repository.Save(user);

            if (NextHandler == null) return response;

            return NextHandler.HandleCommand(command);
        }
    }
}