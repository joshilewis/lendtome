using System;
using Joshilewis.Cqrs.Command;

namespace Lending.Domain.AddBookToLibrary
{
    public class AddBookToLibrary : AuthenticatedCommand
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Isbn { get; set; }
        public DateTime PublishDate { get; set; }

        public AddBookToLibrary(Guid processId, Guid aggregateId, Guid userId, string title, string author, string isbnnumber, DateTime publishDate)
            : base(processId, aggregateId, userId)
        {
            Title = title;
            Author = author;
            Isbn = isbnnumber;
            PublishDate = publishDate;
        }

        public AddBookToLibrary()
        {
        }
    }
}