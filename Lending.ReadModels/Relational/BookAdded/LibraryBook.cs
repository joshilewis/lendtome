using System;

namespace Lending.ReadModels.Relational.BookAdded
{
    public class LibraryBook
    {
        public virtual long Id { get; protected set; }
        public virtual Guid ProcessId { get; protected set; }
        public virtual Guid LibraryId { get; protected set; }
        public virtual string LibraryName { get; set; }
        public virtual string Title { get; protected set; }
        public virtual string Author { get; protected set; }
        public virtual string Isbn { get; protected set; }

        public LibraryBook(Guid processId, Guid libraryId, string libraryName, string title, string author, string isbn)
        {
            ProcessId = processId;
            LibraryId = libraryId;
            LibraryName = libraryName;
            Title = title;
            Author = author;
            Isbn = isbn;
        }

        protected LibraryBook() {  }
    }
}