using System;
using Lending.ReadModels.Relational.LibraryOpened;

namespace Lending.ReadModels.Relational.BookAdded
{
    public class LibraryBook
    {
        public virtual long Id { get; protected set; }
        public virtual Guid ProcessId { get; protected set; }
        public virtual Guid LibraryId { get; protected set; }
        public virtual string LibraryName { get; protected set; }
        public virtual Guid LibraryAdminId { get; protected set; }
        public virtual string AdministratorPicture { get; protected set; }
        public virtual string Title { get; protected set; }
        public virtual string Author { get; protected set; }
        public virtual string Isbn { get; protected set; }
        public virtual int PublishYear { get; protected set; }

        public LibraryBook(Guid processId, OpenedLibrary library, string title, string author, string isbn, int publishYear)
        {
            ProcessId = processId;
            LibraryId = library.Id;
            LibraryName = library.Name;
            LibraryAdminId = library.AdministratorId;
            AdministratorPicture = library.AdministratorPicture;
            Title = title;
            Author = author;
            Isbn = isbn;
            PublishYear = publishYear;
        }

        protected LibraryBook()
        {
        }
    }
}