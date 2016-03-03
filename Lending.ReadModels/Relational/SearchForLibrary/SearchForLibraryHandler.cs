using System;
using System.Linq;
using Joshilewis.Cqrs.Query;
using Lending.Domain.OpenLibrary;
using Lending.ReadModels.Relational.LibraryOpened;
using Lending.ReadModels.Relational.ListLibrayLinks;
using NHibernate;

namespace Lending.ReadModels.Relational.SearchForLibrary
{
    public class SearchForLibraryHandler : NHibernateQueryHandler<SearchForLibrary>
    {
        public SearchForLibraryHandler(Func<ISession> sessionFunc)
            : base(sessionFunc)
        {
        }

        public override object Handle(SearchForLibrary query)
        {
            var queryOver = Session.QueryOver<OpenedLibrary>()
                .WhereRestrictionOn(x => x.Name).IsInsensitiveLike("%" + query.SearchString.ToLower() + "%");


            if (query.UserId.HasValue) //Authenticated so exclude own Library in result
            {
                queryOver = queryOver
                    .Where(x => x.Id != query.UserId.Value);
            }

            LibrarySearchResult[] libraries = queryOver
                .List()
                .Select(x => new LibrarySearchResult(x.Id, x.Name, x.AdministratorPicture))
                .ToArray();
            return libraries;
        }
    }
}