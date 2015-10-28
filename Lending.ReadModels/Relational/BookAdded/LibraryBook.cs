using System;

namespace Lending.ReadModels.Relational.BookAdded
{
    public class LibraryBook
    {
        public virtual long Id { get; protected set; }
        public virtual Guid ProcessId { get; protected set; }
        public virtual Guid OwnerId { get; protected set; }
        public virtual string Title { get; protected set; }
        public virtual string Author { get; protected set; }
        public virtual string Isbn { get; protected set; }

        public LibraryBook(Guid processId, Guid ownerId, string title, string author, string isbn)
        {
            ProcessId = processId;
            OwnerId = ownerId;
            Title = title;
            Author = author;
            Isbn = isbn;
        }

        protected LibraryBook() {  }
    }
}