using System;
using System.Linq;
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
            return Session.QueryOver<RequestedLink>()
                .JoinQueryOver(x => x.RequestingLibrary)
                .Where(x => x.Id == query.UserId)
                .Where(x => x.AdministratorId == query.UserId)
                .List()
                .Select(x => new LibrarySearchResult(x.TargetLibrary.Id, x.TargetLibrary.Name, x.TargetLibrary.AdministratorPicture))
                .ToArray();
        }

    }
}
