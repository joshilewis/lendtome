using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Joshilewis.Cqrs.Query;
using Lending.ReadModels.Relational.LibraryOpened;
using Lending.ReadModels.Relational.ListLibrayLinks;
using NHibernate;
using Dapper;

namespace Lending.ReadModels.Relational.ListLibraries
{
    public class ListLibrariesHandler : NHibernateQueryHandler<ListLibraries>, IAuthenticatedQueryHandler<ListLibraries>
    {
        public ListLibrariesHandler(Func<ISession> sessionFunc)
            : base(sessionFunc)
        {
        }

        public override object Handle(ListLibraries query)
        {
            return Connection
                .Query<OpenedLibrary>($"SELECT * FROM \"OpenedLibrary\" WHERE AdministratorId = '{query.UserId}'")
                .Select(x => new LibrarySearchResult(x.Id, x.Name, x.AdministratorPicture))
                .ToArray();
        }
    }
}
