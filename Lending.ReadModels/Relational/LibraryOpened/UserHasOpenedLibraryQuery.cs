using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Domain.OpenLibrary;
using NHibernate;

namespace Lending.ReadModels.Relational.LibraryOpened
{
    public class UserHasOpenedLibraryQuery : ICheckIfUserHasOpenedLibrary
    {
        private readonly Func<ISession> getSession;

        public UserHasOpenedLibraryQuery(Func<ISession> sessionFunc)
        {
            this.getSession = sessionFunc;
        }

        public bool UserHasOpenedLibrary(Guid userId)
        {
            int count = getSession().QueryOver<OpenedLibrary>()
                .Where(x => x.AdministratorId == userId)
                .RowCount();
            return count > 0;
        }
    }
}
