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
                $"SELECT * FROM \"OpenedLibrary\" WHERE LOWER(Name) LIKE '%{query.SearchString.ToLower()}%'";

            if (!string.IsNullOrEmpty(query.UserId)) //Authenticated so exclude own Library in result
            {
                sqlQuery += $" AND AdministratorId != '{query.UserId}'";
            }

            LibrarySearchResult[] libraries = Connection.Query<OpenedLibrary>(sqlQuery)
                .Select(x => new LibrarySearchResult(x.Id, x.Name, x.AdministratorPicture))
                .ToArray();
            return libraries;
        }
    }
}