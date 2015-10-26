using System;
using NHibernate;

namespace Lending.ReadModels.Relational.ConnectionAccepted
{
    public class ConnectionAcceptedEventHandler : Lending.Cqrs.EventHandler<Domain.AcceptConnection.ConnectionAccepted>
    {
        private readonly Func<ISession> getSession; 

        public ConnectionAcceptedEventHandler(Func<ISession> sessionFunc)
        {
            getSession = sessionFunc;
        }

        public override void When(Domain.AcceptConnection.ConnectionAccepted @event)
        {
            getSession().Save(new UserConnection(@event.ProcessId, @event.RequestingUserId, @event.AggregateId));
        }
    }
}