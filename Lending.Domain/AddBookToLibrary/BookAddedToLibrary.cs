using System;
using Joshilewis.Cqrs;

namespace Lending.Domain.AddBookToLibrary
{
    public class BookAddedToLibrary : Event
    {
        public string Title { get; protected set; }
        public string Author { get; protected set; }
        public string Isbn { get; protected set; }
        public DateTime PublishDate { get; set; }

        public BookAddedToLibrary(Guid processId, Guid aggregateId, string title, string author, string isbn, DateTime publishDate)
            : base(processId, aggregateId)
        {
            Title = title;
            Author = author;
            Isbn = isbn;
            PublishDate = publishDate;
        }
    }
}