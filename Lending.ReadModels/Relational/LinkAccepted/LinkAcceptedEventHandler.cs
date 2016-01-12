using System;
using NHibernate;

namespace Lending.ReadModels.Relational.LinkAccepted
{
    public class LinkAcceptedEventHandler : Lending.Cqrs.EventHandler<Domain.AcceptLink.LinkAccepted>
    {
        private readonly Func<ISession> getSession; 

        public LinkAcceptedEventHandler(Func<ISession> sessionFunc)
        {
            getSession = sessionFunc;
        }

        public override void When(Domain.AcceptLink.LinkAccepted @event)
        {
            getSession().Save(new LibraryLink(@event.ProcessId, @event.RequestingLibraryId, @event.AggregateId));
        }
    }
}