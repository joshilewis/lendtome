using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Joshilewis.Cqrs.Query;
using Lending.ReadModels.Relational.LibraryOpened;
using Lending.ReadModels.Relational.LinkAccepted;
using Lending.ReadModels.Relational.LinkRequested;
using NHibernate;

namespace Lending.ReadModels.Relational.ListLibrayLinks
{
    public class ListLibrayLinksHandler : NHibernateQueryHandler<ListLibraryLinks>,
        IAuthenticatedQueryHandler<ListLibraryLinks>
    {
        public ListLibrayLinksHandler(Func<ISession> sessionFunc)
            : base(sessionFunc)
        {
        }

        public override object Handle(ListLibraryLinks query)
        {
            return Connection.Query<LibraryLink>($"SELECT * FROM \"LibraryLink\" WHERE acceptingadministratorid = '{query.UserId}' OR requestingadministratorid = '{query.UserId}'")
                .Select(x =>
                {
                    if (x.AcceptingLibraryId == query.UserId)
                        return new LibrarySearchResult(x.RequestingLibraryId, x.RequestingLibraryName, x.RequestingAdministratorPicture);
                    return new LibrarySearchResult(x.AcceptingLibraryId, x.AcceptingLibraryName, x.AcceptingAdministratorPicture);
                })
                .ToArray();
        }
    }
}
