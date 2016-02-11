using System;
using System.Linq;
using Joshilewis.Cqrs.Query;
using Lending.Domain.OpenLibrary;
using Lending.ReadModels.Relational.LibraryOpened;
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
            OpenedLibrary[] libraries = Session.QueryOver<OpenedLibrary>()
                .WhereRestrictionOn(x => x.Name).IsInsensitiveLike("%" + query.SearchString.ToLower() + "%")
                .List()
                .ToArray();

            return new Result<OpenedLibrary[]>(libraries);
        }
    }
}