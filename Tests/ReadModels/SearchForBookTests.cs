using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs;
using Lending.Cqrs.Query;
using Lending.Domain.AcceptConnection;
using Lending.Domain.RegisterUser;
using Lending.Domain.RequestConnection;
using Lending.ReadModels.Relational.BookAdded;
using Lending.ReadModels.Relational.ConnectionAccepted;
using NHibernate;
using NUnit.Framework;

namespace Tests.ReadModels
{
    /// <summary>
    /// As a User I want to Search for Books in my Connections' Libraries so that I can find out if any of my Connections have the Book 
    /// I want to Borrow.
    /// </summary>
    public class SearchForBookTests : FixtureWithEventStoreAndNHibernate
    {
        /// <summary>
        /// GIVEN User1 has Registered AND User2 has Registered AND User1 has Request to Connect with User2 AND User2 
        /// has Accepted the Connection from User1
        /// WHEN User1 Searches for a Book with the Search Term "Extreme Programming Explained"
        /// THEN A successful result is returned with an empty list of owners
        /// </summary>
        [Test]
        public void SearchingForBookNotOwnedByAnyConnectionShouldReturnEmptyList()
        {
            var registeredUser1 = new RegisteredUser(1, Guid.NewGuid(), "User1");
            var registeredUser2 = new RegisteredUser(2, Guid.NewGuid(), "User2");
            var userConnection = new UserConnection(Guid.Empty, registeredUser1.Id, registeredUser2.Id);

            SaveEntities(registeredUser1, registeredUser2, userConnection);
            CommitTransactionAndOpenNew();

            var query = new SearchForBook(registeredUser1.Id, "Extreme Programming Explained");

            var expectedResult = new Result<BookSearchResult[]>(new BookSearchResult[] {});

            Result actualResult = new SearchForBookHandler(() => Session).Handle(query);

            ((Result<BookSearchResult[]>) actualResult).ShouldEqual(expectedResult);

        }

        /// <summary>
        /// GIVEN User1 has Registered
        /// WHEN User1 Searches for a Book with the Search Term "Extreme Programming Explained"
        /// THEN A failed result is returned, with reason 'User has no Connections'
        /// </summary>
        [Test]
        public void SearchingForBookWithNoConnectionsShouldFail()
        {
            var registeredUser1 = new RegisteredUser(1, Guid.NewGuid(), "User1");

            SaveEntities(registeredUser1);
            CommitTransactionAndOpenNew();

            var query = new SearchForBook(registeredUser1.Id, "Extreme Programming Explained");

            var expectedResult = new Result<BookSearchResult[]>(SearchForBookHandler.UserHasNoConnection, new BookSearchResult[] { });

            Result actualResult = new SearchForBookHandler(() => Session).Handle(query);

            ((Result<BookSearchResult[]>)actualResult).ShouldEqual(expectedResult);

        }

    }

    public class SearchForBookHandler : IQueryHandler<SearchForBook, Result>
    {
        public const string UserHasNoConnection = "User has no connections";

        private readonly Func<ISession> getSession;

        public SearchForBookHandler(Func<ISession> sessionFunc)
        {
            this.getSession = sessionFunc;
        }

        public Result Handle(SearchForBook message)
        {
            ISession session = getSession();

            int numberOfConnections = session.QueryOver<UserConnection>()
                .Where(x => x.AcceptingUserId == message.UserId || x.RequestingUserId == message.UserId)
                .RowCount();

            if (numberOfConnections == 0)
                return new Result<BookSearchResult[]>(UserHasNoConnection, new BookSearchResult[] {});

            BookSearchResult[] payload = session.QueryOver<LibraryBook>()
                .WhereRestrictionOn(x => x.Title).IsInsensitiveLike("%" + message.SearchString + "%")
                .WhereRestrictionOn(x => x.Author).IsInsensitiveLike("%" + message.SearchString + "%")
                .List()
                .Select(x => new BookSearchResult(x.OwnerId, "", x.Title, x.Author))
                .ToArray();

            return new Result<BookSearchResult[]>(payload);
        }

    }

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

            if (!base.Equals(obj)) return false;
            var other = (BookSearchResult)obj;
            return UserId.Equals(other.UserId) &&
                   Username.Equals(other.Username) &&
                   Title.Equals(other.Title) &&
                   Author.Equals(other.Author);
        }
    }

    public class SearchForBook : Query, IAuthenticated
    {
        public string SearchString { get; set; }

        public SearchForBook(Guid userId, string searchString)
        {
            SearchString = searchString;
            UserId = userId;
        }

        public Guid UserId { get; set; }
    }
}
