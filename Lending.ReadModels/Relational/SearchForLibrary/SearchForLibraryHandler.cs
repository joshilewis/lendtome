using System;
using System.Data;
using System.Linq;
using Dapper;
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
            string sqlQuery =
                $"SELECT * FROM \"OpenedLibrary\" WHERE LOWER(\"Name\") LIKE '%{query.SearchString.ToLower()}%'";

            if (query.UserId.HasValue) //Authenticated so exclude own Library in result
            {
                sqlQuery += $" AND \"Id\" != '{query.UserId.Value}'";
            }

            LibrarySearchResult[] libraries = Connection.Query<OpenedLibrary>(sqlQuery)
                .Select(x => new LibrarySearchResult(x.Id, x.Name, x.AdministratorPicture))
                .ToArray();
            return libraries;
        }
    }
}