using System;
using Dapper;
using Dapper.Contrib.Extensions;
using FluentNHibernate.Conventions;
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
            OpenedLibrary existingLibrary = Session.Connection.Get<OpenedLibrary>(@event.AggregateId);
            if (existingLibrary != null) return;

            var existingLibraries =
                Session.Connection.Query<OpenedLibrary>(
                    $"SELECT * FROM \"OpenedLibrary\" WHERE administratorId = '{@event.AdministratorId}'");

            if(existingLibraries.IsNotEmpty()) return;

            OpenedLibrary openedLibrary =
                new OpenedLibrary(@event.AggregateId, @event.Name, @event.AdministratorId, @event.Picture);
            Session.Save(openedLibrary);

        }
    }
}
