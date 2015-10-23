using System;
using Lending.Cqrs;

namespace Lending.Domain.AddBookToLibrary
{
    public class BookAddedToLibrary : Event
    {
        public string Title { get; protected set; }
        public string Author { get; protected set; }
        public string Isbn { get; protected set; }

        public BookAddedToLibrary(Guid processId, Guid aggregateId, string title, string author, string isbn)
            : base(processId, aggregateId)
        {
            Title = title;
            Author = author;
            Isbn = isbn;
        }
    }
}