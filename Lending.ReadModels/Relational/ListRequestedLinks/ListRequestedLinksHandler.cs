using System;
using System.Linq;
using Lending.Cqrs.Query;
using Lending.ReadModels.Relational.LinkRequested;
using NHibernate;

namespace Lending.ReadModels.Relational.ListRequestedLinks
{
    public class ListRequestedLinksHandler : NHibernateQueryHandler<ListRequestedLinks, Result>, IAuthenticatedQueryHandler<ListRequestedLinks, Result>
    {
        public ListRequestedLinksHandler(Func<ISession> sessionFunc)
            : base(sessionFunc)
        {
        }

        public override Result Handle(ListRequestedLinks query)
        {
            return new Result<RequestedLink[]>(Session.QueryOver<RequestedLink>()
                .JoinQueryOver(x => x.RequestingLibrary)
                .Where(x => x.Id == query.UserId)
                .Where(x => x.AdministratorId == query.UserId)
                .List()
                .ToArray());
        }

    }
}
