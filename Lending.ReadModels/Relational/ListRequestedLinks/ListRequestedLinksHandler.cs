using System;
using System.Linq;
using Dapper;
using Joshilewis.Cqrs.Query;
using Lending.ReadModels.Relational.LinkRequested;
using Lending.ReadModels.Relational.ListLibrayLinks;
using NHibernate;

namespace Lending.ReadModels.Relational.ListRequestedLinks
{
    public class ListRequestedLinksHandler : NHibernateQueryHandler<ListRequestedLinks>, IAuthenticatedQueryHandler<ListRequestedLinks>
    {
        public ListRequestedLinksHandler(Func<ISession> sessionFunc)
            : base(sessionFunc)
        {
        }

        public override object Handle(ListRequestedLinks query)
        {
            return Connection
                .Query<RequestedLink>(
                    $" SELECT * FROM \"RequestedLink\" WHERE \"RequestingLibraryId\" = '{query.UserId}' AND \"RequestingAdministratorId\" = '{query.UserId}'")
                .Select(x => new LibrarySearchResult(x.TargetLibraryId, x.TargetLibraryName,
                    x.TargetAdministratorPicture))
                .ToArray();
        }

    }
}
