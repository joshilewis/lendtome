using System;
using Lending.Cqrs;

namespace Lending.Domain.RemoveBookFromLibrary
{
    public class RemoveBookFromLibrary : AuthenticatedCommand
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Isbn { get; set; }

        public RemoveBookFromLibrary(Guid processId, Guid aggregateId, Guid userId, string title, string author,
            string isbn) : base(processId, aggregateId, userId)
        {
            Title = title;
            Author = author;
            Isbn = isbn;
        }
    }
}