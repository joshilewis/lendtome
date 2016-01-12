using System;
using Lending.Cqrs;
using Lending.Cqrs.Command;

namespace Lending.Domain.RemoveBookFromLibrary
{
    public class RemoveBookFromLibrary : AuthenticatedCommand
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Isbn { get; set; }

        public RemoveBookFromLibrary(Guid processId, Guid aggregateId, Guid libraryId, string title, string author,
            string isbn) : base(processId, aggregateId, libraryId)
        {
            Title = title;
            Author = author;
            Isbn = isbn;
        }
    }
}