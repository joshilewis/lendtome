using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Joshilewis.Cqrs.Query;
using Lending.ReadModels.Relational.LibraryOpened;
using Lending.ReadModels.Relational.ListLibrayLinks;
using NHibernate;

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
            IEnumerable<OpenedLibrary> libraries = Session.QueryOver<OpenedLibrary>()
                .Where(x => x.AdministratorId == query.UserId)
                .List();

            return libraries.Select(x => new LibrarySearchResult(x.Id, x.Name))
                .ToArray();
        }
    }
}
