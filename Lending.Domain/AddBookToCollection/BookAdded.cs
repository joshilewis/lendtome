using System;
using Lending.Cqrs;

namespace Lending.Domain.AddBookToCollection
{
    public class BookAdded : Event
    {
        public string Isbn { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }

        public BookAdded(Guid processId, Guid id, string title, string author, string isbn)
            : base(processId, id)
        {
            Title = title;
            Author = author;
            Isbn = isbn;
        }

    }
}