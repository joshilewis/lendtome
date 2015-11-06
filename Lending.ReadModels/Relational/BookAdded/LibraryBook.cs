using System;

namespace Lending.ReadModels.Relational.BookAdded
{
    public class LibraryBook
    {
        public virtual long Id { get; protected set; }
        public virtual Guid ProcessId { get; protected set; }
        public virtual Guid OwnerId { get; protected set; }
        public virtual string OwnerName { get; set; }
        public virtual string Title { get; protected set; }
        public virtual string Author { get; protected set; }
        public virtual string Isbn { get; protected set; }

        public LibraryBook(Guid processId, Guid ownerId, string ownerName, string title, string author, string isbn)
        {
            ProcessId = processId;
            OwnerId = ownerId;
            OwnerName = ownerName;
            Title = title;
            Author = author;
            Isbn = isbn;
        }

        protected LibraryBook() {  }
    }
}