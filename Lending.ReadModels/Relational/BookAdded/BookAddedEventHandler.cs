using System;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.OpenLibrary;
using Lending.ReadModels.Relational.LibraryOpened;
using NHibernate;

namespace Lending.ReadModels.Relational.BookAdded
{
    public class BookAddedEventHandler : NHibernateEventHandler<BookAddedToLibrary>
    {
        public BookAddedEventHandler(Func<ISession> sessionFunc)
            : base(sessionFunc)
        {
        }

        public override void When(BookAddedToLibrary @event)
        {
            string username = Session.Get<OpenedLibrary>(@event.AggregateId).Name;
            OpenedLibrary library = Session.Get<OpenedLibrary>(@event.AggregateId);

            Session.Save(new LibraryBook(@event.ProcessId, library, username, @event.Title, @event.Author,
                @event.Isbn, @event.PublishYear));
        }
    }
}