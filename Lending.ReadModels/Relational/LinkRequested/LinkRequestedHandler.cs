using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
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
            OpenedLibrary requestingLibrary = Connection.GetOpenedLibrary(@event.AggregateId);
            OpenedLibrary targetLibrary = Connection.GetOpenedLibrary(@event.TargetLibraryId);

            Connection.Insert(new RequestedLink(@event.ProcessId, requestingLibrary, targetLibrary));
        }

    }
}
