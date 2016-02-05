using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs;
using Lending.Cqrs.Query;
using Lending.ReadModels.Relational.LibraryOpened;
using NHibernate;

namespace Lending.ReadModels.Relational.ListLibraries
{
    public class ListLibrariesHandler : NHibernateQueryHandler<ListLibraries, Result>, IAuthenticatedQueryHandler<ListLibraries, Result>
    {
        public ListLibrariesHandler(Func<ISession> sessionFunc)
            : base(sessionFunc)
        {
        }

        public override Result Handle(ListLibraries query)
        {
            OpenedLibrary[] libraries = Session.QueryOver<OpenedLibrary>()
                .Where(x => x.AdministratorId == query.UserId)
                .List()
                .ToArray();

            return new Result<OpenedLibrary[]>(libraries);
        }
    }
}
