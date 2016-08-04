using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.ReadModels.Relational.LibraryOpened;
using NHibernate;

namespace Lending.ReadModels.Relational.LinkRequested
{
    public class LinkRequestedHandler : NHibernateEventHandler<Domain.RequestLink.LinkRequested>
    {
        public LinkRequestedHandler(Func<ISession> sessionFunc)
            : base(sessionFunc)
        {
        }

        public override void When(Domain.RequestLink.LinkRequested @event)
        {
            OpenedLibrary requestingLibrary = Session.Get<OpenedLibrary>(@event.AggregateId);
            OpenedLibrary targetLibrary = Session.Get<OpenedLibrary>(@event.TargetLibraryId);

            Session.Save(new RequestedLink(@event.ProcessId, requestingLibrary, targetLibrary));
        }

    }
}
