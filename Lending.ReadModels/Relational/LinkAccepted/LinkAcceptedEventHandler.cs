using System;
using Lending.ReadModels.Relational.LibraryOpened;
using Lending.ReadModels.Relational.LinkRequested;
using NHibernate;

namespace Lending.ReadModels.Relational.LinkAccepted
{
    public class LinkAcceptedEventHandler : NHibernateEventHandler<Domain.AcceptLink.LinkAccepted>
    {
        public LinkAcceptedEventHandler(Func<ISession> sessionFunc)
            : base(sessionFunc)
        {
        }

        public override void When(Domain.AcceptLink.LinkAccepted @event)
        {
            OpenedLibrary targetLibraryAlias = null;
            OpenedLibrary requestingLibraryAlias = null;
            RequestedLink requestedLink = Session.QueryOver<RequestedLink>()
                .JoinAlias(x => x.TargetLibrary, () => targetLibraryAlias)
                .JoinAlias(x => x.RequestingLibrary, () => requestingLibraryAlias)
                .Where(() => requestingLibraryAlias.Id == @event.RequestingLibraryId)
                .Where(() => targetLibraryAlias.Id == @event.AggregateId)
                .SingleOrDefault();
            Session.Delete(requestedLink);

            OpenedLibrary requestingLibrary = Session.Get<OpenedLibrary>(@event.RequestingLibraryId);
            OpenedLibrary acceptingLibrary = Session.Get<OpenedLibrary>(@event.AggregateId);

            Session.Save(new LibraryLink(@event.ProcessId, requestingLibrary, acceptingLibrary));
        }
    }
}