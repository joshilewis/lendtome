using System;
using Lending.Domain.OpenLibrary;
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
            AuthenticatedUser user = Session.Get<AuthenticatedUser>(@event.AdministratorId);

            OpenedLibrary openedLibrary = new OpenedLibrary(@event.AggregateId, @event.Name, user.Id, user.Picture);
            Session.Save(openedLibrary);

        }
    }
}
