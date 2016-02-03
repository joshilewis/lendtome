using System;
using System.Linq;
using Lending.Cqrs.Query;
using Lending.ReadModels.Relational.LinkRequested;
using NHibernate;

namespace Lending.ReadModels.Relational.ListReceivedLinks
{
    public class ListReceivedLinksHandler : NHibernateQueryHandler<ListReceivedLinks, Result>, IAuthenticatedQueryHandler<ListReceivedLinks, Result>
    {
        public ListReceivedLinksHandler(Func<ISession> sessionFunc)
            : base(sessionFunc)
        {
        }

        public override Result Handle(ListReceivedLinks query)
        {
            return new Result<RequestedLink[]>(Session.QueryOver<RequestedLink>()
                .JoinQueryOver(x => x.TargetLibrary)
                .Where(x => x.Id == query.UserId)
                .Where(x => x.AdministratorId == query.UserId)
                .List()
                .ToArray());
        }

    }
}
