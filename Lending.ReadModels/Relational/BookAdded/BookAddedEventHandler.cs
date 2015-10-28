using System;
using Lending.Domain.AddBookToLibrary;
using NHibernate;

namespace Lending.ReadModels.Relational.BookAdded
{
    public class BookAddedEventHandler : Lending.Cqrs.EventHandler<BookAddedToLibrary>
    {
        private readonly Func<ISession> getSession;

        public BookAddedEventHandler(Func<ISession> sessionFunc)
        {
            getSession = sessionFunc;
        }

        public override void When(BookAddedToLibrary @event)
        {
            getSession()
                .Save(new LibraryBook(@event.ProcessId, @event.AggregateId, @event.Title, @event.Author, @event.Isbn));
        }
    }
}