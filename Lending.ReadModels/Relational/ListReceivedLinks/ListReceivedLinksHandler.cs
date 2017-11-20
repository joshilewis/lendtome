using System;
using System.Linq;
using Joshilewis.Cqrs.Query;
using Lending.ReadModels.Relational.LinkRequested;
using Lending.ReadModels.Relational.ListLibrayLinks;
using NHibernate;
using Dapper;

namespace Lending.ReadModels.Relational.ListReceivedLinks
{
    public class ListReceivedLinksHandler : NHibernateQueryHandler<ListReceivedLinks>, IAuthenticatedQueryHandler<ListReceivedLinks>
    {
        public ListReceivedLinksHandler(Func<ISession> sessionFunc)
            : base(sessionFunc)
        {
        }

        public override object Handle(ListReceivedLinks query)
        {
            return Connection
                .Query<RequestedLink>(
                    $"SELECT * FROM \"RequestedLink\" WHERE targetlibraryid = '{query.AggregateId}' AND targetadministratorid = '{query.UserId}'")
                .Select(x => new LibrarySearchResult(x.RequestingLibraryId, x.RequestingLibraryName,
                    x.RequestingAdministratorPicture))
                .ToArray();
        }

    }
}
