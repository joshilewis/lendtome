using System;

namespace Lending.Domain.Persistence
{
    public class AddedBook
    {
        public virtual Guid Id { get; protected set; }
        public virtual string Title { get; protected set; }
        public virtual string Author { get; protected set; }
        public virtual string Isbn { get; protected set; }

        public AddedBook(Guid id, string title, string author, string isbn)
        {
            Id = id;
            Title = title;
            Author = author;
            Isbn = isbn;
        }

        protected AddedBook() { }

    }
}