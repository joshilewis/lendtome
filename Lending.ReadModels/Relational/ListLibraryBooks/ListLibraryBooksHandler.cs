using System;
using System.Linq;
using Lending.Cqrs.Query;
using Lending.ReadModels.Relational.BookAdded;
using Lending.ReadModels.Relational.SearchForBook;
using NHibernate;

namespace Lending.ReadModels.Relational.ListLibraryBooks
{
    public class ListLibraryBooksHandler : NHibernateQueryHandler<ListLibraryBooks, Result>, IAuthenticatedQueryHandler<ListLibraryBooks, Result>
    {
        public ListLibraryBooksHandler(Func<ISession> sessionFunc) : base(sessionFunc)
        {
        }

        public override Result Handle(ListLibraryBooks query)
        {
            LibraryBook[] libraryBooks = Session.QueryOver<LibraryBook>()
                .JoinQueryOver(x => x.Library)
                .Where(x => x.AdministratorId == query.UserId)
                .List()
                .ToArray();

            return new Result<BookSearchResult[]>(libraryBooks
                .Select(x => new BookSearchResult(x.Library.Id, x.LibraryName, x.Title, x.Author, x.Isbn))
                .ToArray());
        }
    }
}
