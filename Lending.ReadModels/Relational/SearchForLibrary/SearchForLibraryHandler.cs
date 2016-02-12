using System;
using System.Linq;
using Joshilewis.Cqrs.Query;
using Lending.Domain.OpenLibrary;
using Lending.ReadModels.Relational.LibraryOpened;
using Lending.ReadModels.Relational.ListLibrayLinks;
using NHibernate;

namespace Lending.ReadModels.Relational.SearchForLibrary
{
    public class SearchForLibraryHandler : NHibernateQueryHandler<SearchForLibrary, Result>, 
        IQueryHandler<SearchForLibrary, Result>
    {
        public SearchForLibraryHandler(Func<ISession> sessionFunc)
            : base(sessionFunc)
        {
        }

        public override Result Handle(SearchForLibrary query)
        {
            LibrarySearchResult[] libraries = Session.QueryOver<OpenedLibrary>()
                .WhereRestrictionOn(x => x.Name).IsInsensitiveLike("%" + query.SearchString.ToLower() + "%")
                .List()
                .Select(x => new LibrarySearchResult(x.Id, x.Name))
                .ToArray();

            return new Result<LibrarySearchResult[]>(libraries);
        }
    }
}