using System;
using Lending.Cqrs;

namespace Lending.Domain.AddBookToCollection
{
    public class BookAddedToCollection : Event
    {
        public Guid BookId { get; set; }

        public BookAddedToCollection(Guid processId, Guid userId, Guid bookId)
            : base(processId, userId)
        {
            BookId = bookId;
        }

    }
}