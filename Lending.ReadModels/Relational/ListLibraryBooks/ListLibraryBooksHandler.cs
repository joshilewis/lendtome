using System;
using System.Linq;
using Joshilewis.Cqrs.Query;
using Lending.ReadModels.Relational.BookAdded;
using Lending.ReadModels.Relational.SearchForBook;
using NHibernate;

namespace Lending.ReadModels.Relational.ListLibraryBooks
{
    public class ListLibraryBooksHandler : NHibernateQueryHandler<ListLibraryBooks>, IAuthenticatedQueryHandler<ListLibraryBooks>
    {
        public ListLibraryBooksHandler(Func<ISession> sessionFunc) : base(sessionFunc)
        {
        }

        public override object Handle(ListLibraryBooks query)
        {
            LibraryBook[] libraryBooks = Session.QueryOver<LibraryBook>()
                .Where(x => x.LibraryAdminId == query.UserId)
                .List()
                .ToArray();

            return libraryBooks
                .Select(x => new BookSearchResult(x.LibraryId, x.LibraryName, x.Title, x.Author, x.Isbn, x.PublishYear))
                .ToArray();
        }
    }
}
