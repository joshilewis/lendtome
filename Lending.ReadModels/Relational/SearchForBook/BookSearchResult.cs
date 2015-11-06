using System;

namespace Lending.ReadModels.Relational.SearchForBook
{
    public class BookSearchResult
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }

        public BookSearchResult(Guid userId, string username, string title, string author)
        {
            UserId = userId;
            Username = username;
            Title = title;
            Author = author;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (BookSearchResult)obj;
            return UserId.Equals(other.UserId) &&
                   Username.Equals(other.Username) &&
                   Title.Equals(other.Title) &&
                   Author.Equals(other.Author);
        }
    }
}