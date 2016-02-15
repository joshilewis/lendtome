using System;
using System.Linq;
using Joshilewis.Cqrs.Query;
using Lending.ReadModels.Relational.LinkRequested;
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
                .JoinQueryOver(x => x.TargetLibrary)
                .Where(x => x.Id == query.UserId)
                .Where(x => x.AdministratorId == query.UserId)
                .List()
                .ToArray();
        }

    }
}
