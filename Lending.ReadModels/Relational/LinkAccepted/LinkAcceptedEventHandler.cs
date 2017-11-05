using System;
using Dapper;
using Dapper.Contrib.Extensions;
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
            OpenedLibrary requestingLibrary = Connection.GetOpenedLibrary(@event.RequestingLibraryId);
            OpenedLibrary acceptingLibrary = Connection.GetOpenedLibrary(@event.AggregateId);
            Connection.Insert(new LibraryLink(@event.ProcessId, requestingLibrary, acceptingLibrary));
            Connection.Execute(
                "DELETE FROM \"RequestedLink\" WHERE \"RequestingLibraryId\" = @RequestingLibraryId AND \"TargetLibraryId\" = @TargetLibraryId",
                new { @event.RequestingLibraryId, TargetLibraryId = @event.AggregateId });
        }
    }
}