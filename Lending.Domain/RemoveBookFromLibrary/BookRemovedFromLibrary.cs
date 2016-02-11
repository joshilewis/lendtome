using System;
using Joshilewis.Cqrs;

namespace Lending.Domain.RemoveBookFromLibrary
{
    public class BookRemovedFromLibrary : Event
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Isbn { get; set; }

        public BookRemovedFromLibrary(Guid processId, Guid aggregateId, string title, string author, string isbn)
            : base(processId, aggregateId)
        {
            Title = title;
            Author = author;
            Isbn = isbn;
        }
    }
}