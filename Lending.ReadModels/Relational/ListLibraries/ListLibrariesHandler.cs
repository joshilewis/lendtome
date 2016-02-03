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
    public class ListLibrariesHandler : MessageHandler<ListLibraries, Result>, IAuthenticatedQueryHandler<ListLibraries, Result>
    {
        private readonly Func<ISession> getSession;

        public ListLibrariesHandler(Func<ISession> sessionFunc)
        {
            this.getSession = sessionFunc;
        }

        public override Result Handle(ListLibraries query)
        {
            ISession session = getSession();

            OpenedLibrary[] libraries = getSession().QueryOver<OpenedLibrary>()
                .Where(x => x.AdministratorId == query.UserId)
                .List()
                .ToArray();

            return new Result<OpenedLibrary[]>(libraries);
        }
    }
}
