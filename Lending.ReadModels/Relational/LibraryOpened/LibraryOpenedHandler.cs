using System;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
using NHibernate;

namespace Lending.ReadModels.Relational.LibraryOpened
{
    public class LibraryOpenedHandler : NHibernateEventHandler<Domain.OpenLibrary.LibraryOpened>
    {
        public LibraryOpenedHandler(Func<ISession> sessionFunc)
            : base(sessionFunc)
        {
        }

        public override void When(Domain.OpenLibrary.LibraryOpened @event)
        {
            OpenedLibrary existingLibrary = Connection.GetOpenedLibrary(@event.AggregateId);
            if (existingLibrary != null) return;
            AuthenticatedUser user = Connection.GetAuthenticatedUser(@event.AdministratorId);

            OpenedLibrary openedLibrary = new OpenedLibrary(@event.AggregateId, @event.Name, user.Id, user.Picture);
            Connection.Insert(openedLibrary);

        }
    }
}
