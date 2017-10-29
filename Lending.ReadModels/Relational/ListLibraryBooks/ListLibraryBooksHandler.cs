using System;
using System.Linq;
using Joshilewis.Cqrs.Query;
using Lending.ReadModels.Relational.BookAdded;
using Lending.ReadModels.Relational.SearchForBook;
using NHibernate;
using Dapper;

namespace Lending.ReadModels.Relational.ListLibraryBooks
{
    public class ListLibraryBooksHandler : NHibernateQueryHandler<ListLibraryBooks>, IAuthenticatedQueryHandler<ListLibraryBooks>
    {
        public ListLibraryBooksHandler(Func<ISession> sessionFunc) : base(sessionFunc)
        {
        }

        public override object Handle(ListLibraryBooks query)
        {
            return Connection
                .Query<LibraryBook>($"SELECT * FROM \"LibraryBook\" WHERE LibraryAdminId = '{query.UserId}'")
                .Select(x =>
                    new BookSearchResult(x.LibraryId, x.LibraryName, x.AdministratorPicture, x.Title, x.Author, x.Isbn,
                        x.PublishYear))
                .ToArray();
        }
    }
}
