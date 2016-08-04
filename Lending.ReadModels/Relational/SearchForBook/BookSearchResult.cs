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
        public int PublishYear { get; set; }


        public BookSearchResult(Guid libraryId, string libraryName, string title, string author, string isbn, int publishYear)
        {
            LibraryId = libraryId;
            LibraryName = libraryName;
            Title = title;
            Author = author;
            Isbn = isbn;
            PublishYear = publishYear;
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
                   PublishYear.Equals(other.PublishYear) &&
                   Isbn.Equals(other.Isbn);
        }
        public override int GetHashCode()
        {
            int result = base.GetHashCode();
            result = (result * 397) ^ LibraryId.GetHashCode();
            result = (result * 397) ^ LibraryName.GetHashCode();
            result = (result * 397) ^ Title.GetHashCode();
            result = (result * 397) ^ Author.GetHashCode();
            result = (result * 397) ^ Isbn.GetHashCode();
            result = (result * 397) ^ PublishYear.GetHashCode();
            return result;
        }
    }
}