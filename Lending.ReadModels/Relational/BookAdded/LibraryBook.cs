using System;
using Lending.ReadModels.Relational.LibraryOpened;

namespace Lending.ReadModels.Relational.BookAdded
{
    public class LibraryBook
    {
        public virtual long Id { get; protected set; }
        public virtual Guid ProcessId { get; protected set; }
        public virtual OpenedLibrary Library { get; protected set; }
        public virtual string LibraryName { get; set; }
        public virtual string Title { get; protected set; }
        public virtual string Author { get; protected set; }
        public virtual string Isbn { get; protected set; }

        public LibraryBook(Guid processId, OpenedLibrary library, string libraryName, string title, string author, string isbn)
        {
            ProcessId = processId;
            Library = library;
            LibraryName = libraryName;
            Title = title;
            Author = author;
            Isbn = isbn;
        }

        protected LibraryBook() {  }
    }
}