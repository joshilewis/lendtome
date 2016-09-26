using System;
using System.Linq;
using Joshilewis.Cqrs.Query;
using Lending.ReadModels.Relational.LinkRequested;
using Lending.ReadModels.Relational.ListLibrayLinks;
using NHibernate;

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
            return Session.QueryOver<RequestedLink>()
                .Where(x => x.TargetLibraryId == query.UserId)
                .Where(x => x.TargetAdministratorId == query.UserId)
                .List()
                .Select(x => new LibrarySearchResult(x.RequestingLibraryId, x.RequestingLibraryName, x.RequestingAdministratorPicture))
                .ToArray();
        }

    }
}
