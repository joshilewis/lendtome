using System;

namespace Lending.ReadModels.Relational.SearchForBook
{
    public class BookSearchResult
    {
        public Guid LibraryId { get; set; }
        public string LibraryName { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Isbn { get; set; }

        public BookSearchResult(Guid libraryId, string libraryName, string title, string author, string isbn)
        {
            LibraryId = libraryId;
            LibraryName = libraryName;
            Title = title;
            Author = author;
            Isbn = isbn;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (BookSearchResult)obj;
            return LibraryId.Equals(other.LibraryId) &&
                   LibraryName.Equals(other.LibraryName) &&
                   Title.Equals(other.Title) &&
                   Author.Equals(other.Author) &&
                   Isbn.Equals(other.Isbn);
        }
    }
}