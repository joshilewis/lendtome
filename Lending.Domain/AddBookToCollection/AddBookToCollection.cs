using System;
using Lending.Cqrs;

namespace Lending.Domain.AddBookToCollection
{
    public class AddBookToCollection : AuthenticatedCommand
    {
        public string Isbn { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }

        public AddBookToCollection(Guid processId, Guid id, Guid userId, string title, string author, string isbnnumber)
            : base(processId, id, userId)
        {
            Title = title;
            Author = author;
            Isbn = isbnnumber;
        }

    }
}