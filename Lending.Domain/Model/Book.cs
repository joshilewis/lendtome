namespace Lending.Domain.Model
{
    public class Book
    {
        public string Title { get; protected set; }
        public string Author { get; protected set; }
        public string Isbn { get; protected set; }
        public int Year { get; protected set; }
        public string CoverPicture { get; protected set; }

        public Book(string title, string author, string isbn, int year, string coverPicure)
        {
            Title = title;
            Author = author;
            Isbn = isbn;
            Year = year;
            CoverPicture = coverPicure;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Book)) return false;

            Book other = (Book) obj;

            return Title.Equals(other.Title) &&
                   Author.Equals(other.Author) &&
                   Year.Equals(other.Year) &&
                   Isbn.Equals(other.Isbn) &&
                   CoverPicture.Equals(other.CoverPicture);
        }

        public override int GetHashCode()
        {
            int result = base.GetHashCode();
            result = (result * 397) ^ Title.GetHashCode();
            result = (result * 397) ^ Author.GetHashCode();
            result = (result * 397) ^ Isbn.GetHashCode();
            result = (result * 397) ^ Year.GetHashCode();
            result = (result * 397) ^ CoverPicture.GetHashCode();
            return result;
        }
    }
}