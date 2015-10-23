using System;
using Lending.Cqrs;
using Lending.Domain.Model;
using NHibernate;

namespace Lending.Domain.AcceptConnection
{
    public class AcceptConnectionHandler : AuthenticatedCommandHandler<AcceptConnection, Result>
    {
        public AcceptConnectionHandler(Func<ISession> getSession, Func<IEventRepository> getRepository) 
            : base(getSession, getRepository)
        {
        }

        public override Result HandleCommand(AcceptConnection command)
        {
            User acceptingUser = User.CreateFromHistory(EventRepository.GetEventsForAggregate<User>(command.AggregateId));

            Result result = acceptingUser.AcceptConnection(command.ProcessId, command.RequestingUserId);

            if (!result.Success) return result;

            User requestingUser = User.CreateFromHistory(EventRepository.GetEventsForAggregate<User>(command.RequestingUserId));

            result = requestingUser.CompleteConnection(command.ProcessId, command.AggregateId);
            if (!result.Success) return result;

            EventRepository.Save(acceptingUser);
            EventRepository.Save(requestingUser);

            return Success();
        }
    }
}