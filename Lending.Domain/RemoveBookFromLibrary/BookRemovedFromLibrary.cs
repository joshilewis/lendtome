using System;
using Joshilewis.Cqrs;

namespace Lending.Domain.RemoveBookFromLibrary
{
    public class BookRemovedFromLibrary : Event
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Isbn { get; set; }
        public int PublishYear { get; set; }
        public string CoverPicture { get; set; }

        public BookRemovedFromLibrary(Guid processId, Guid aggregateId, string title, string author, string isbn, int publishYear, string coverPicture)
            : base(processId, aggregateId)
        {
            Title = title;
            Author = author;
            Isbn = isbn;
            PublishYear = publishYear;
            CoverPicture = coverPicture;
        }
    }
}