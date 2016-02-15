using System;
using System.Linq;
using Joshilewis.Cqrs.Query;
using Lending.Domain.OpenLibrary;
using Lending.ReadModels.Relational.LibraryOpened;
using Lending.ReadModels.Relational.ListLibrayLinks;
using NHibernate;

namespace Lending.ReadModels.Relational.SearchForLibrary
{
    public class SearchForLibraryHandler : NHibernateQueryHandler<SearchForLibrary, LibrarySearchResult[]>
    {
        public SearchForLibraryHandler(Func<ISession> sessionFunc)
            : base(sessionFunc)
        {
        }

        public override LibrarySearchResult[] Handle(SearchForLibrary query)
        {
            LibrarySearchResult[] libraries = Session.QueryOver<OpenedLibrary>()
                .WhereRestrictionOn(x => x.Name).IsInsensitiveLike("%" + query.SearchString.ToLower() + "%")
                .List()
                .Select(x => new LibrarySearchResult(x.Id, x.Name))
                .ToArray();
            return libraries;
        }
    }
}