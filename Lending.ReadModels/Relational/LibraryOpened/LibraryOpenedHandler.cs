using System;
using Lending.Domain.OpenLibrary;
using NHibernate;

namespace Lending.ReadModels.Relational.LibraryOpened
{
    public class LibraryOpenedHandler : Lending.Cqrs.EventHandler<Domain.OpenLibrary.LibraryOpened>
    {
        private readonly Func<ISession> getSession;

        public LibraryOpenedHandler(Func<ISession> sessionFunc)
        {
            this.getSession = sessionFunc;
        }

        public override void When(Domain.OpenLibrary.LibraryOpened @event)
        {
            OpenedLibrary openedLibrary = new OpenedLibrary(@event.AggregateId, @event.Name);
            getSession().Save(openedLibrary);

        }
    }
}
