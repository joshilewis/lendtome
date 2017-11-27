using System;
using Joshilewis.Cqrs.Command;

namespace Lending.Domain.AddBookToLibrary
{
    public class AddBookToLibrary : AuthenticatedCommand
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Isbn { get; set; }
        public int PublishYear { get; set; }
        public string CoverPicture { get; set; }

        public AddBookToLibrary(Guid processId, Guid aggregateId, string userId, string title, string author, string isbnnumber, int publishYear, string coverPicture)
            : base(processId, aggregateId, userId)
        {
            Title = title;
            Author = author;
            Isbn = isbnnumber;
            PublishYear = publishYear;
            CoverPicture = coverPicture;
        }

        public AddBookToLibrary()
        {
        }
    }
}