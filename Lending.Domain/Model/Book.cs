using System;
using System.Collections.Generic;
using Lending.Cqrs;
using Lending.Domain.AddBookToCollection;

namespace Lending.Domain.Model
{
    public class Book : Aggregate
    {
        public virtual string Title { get; protected set; }
        public virtual string Author { get; protected set; }
        public virtual string Isbn { get; protected set; }

        protected Book(Guid processId, Guid id, string title, string author, string isbn)
            : this()
        {
            RaiseEvent(new BookAdded(processId, id, title, author, isbn));
        }

        protected Book()
        {

        }

        public static Book AddBook(Guid processId, Guid newBookId, string title, string author, string isbn)
        {
            return new Book(processId, newBookId, title, author, isbn);
        }

        public static Book CreateFromHistory(IEnumerable<Event> events)
        {
            var book = new Book();
            foreach (var @event in events)
            {
                book.ApplyEvent(@event);
            }
            return book;
        }

        protected virtual void When(BookAdded @event)
        {
            Id = @event.AggregateId;
            Title = @event.Title;
            Author = @event.Author;
            Isbn = @event.Isbn;
        }

        protected override List<IEventRoute> EventRoutes => new List<IEventRoute>()
        {
            new EventRoute<BookAdded>(When, typeof(BookAdded)),

        };
    }
}