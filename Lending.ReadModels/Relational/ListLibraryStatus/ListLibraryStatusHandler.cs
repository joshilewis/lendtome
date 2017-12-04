using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Joshilewis.Cqrs.Query;
using Lending.ReadModels.Relational.ListLibrayLinks;
using Lending.ReadModels.Relational.ListReceivedLinks;
using Lending.ReadModels.Relational.ListRequestedLinks;
using NHibernate;

namespace Lending.ReadModels.Relational.ListLibraryStatus
{
    public class ListLibraryStatusHandler : NHibernateQueryHandler<ListLibraryStatus>, IAuthenticatedQueryHandler<ListLibraryStatus>
    {
        private readonly ListLibrayLinksHandler listLibrayLinksHandler;
        private readonly ListReceivedLinksHandler listReceivedLinksHandler;
        private readonly ListRequestedLinksHandler listRequestedLinksHandler;

        public ListLibraryStatusHandler(Func<ISession> sessionFunc, ListLibrayLinksHandler listLibrayLinksHandler,
            ListReceivedLinksHandler listReceivedLinksHandler,
            ListRequestedLinksHandler listRequestedLinksHandler) : base(sessionFunc)
        {
            this.listLibrayLinksHandler = listLibrayLinksHandler;
            this.listReceivedLinksHandler = listReceivedLinksHandler;
            this.listRequestedLinksHandler = listRequestedLinksHandler;
        }

        public override object Handle(ListLibraryStatus message)
        {
            return new LibraryStatusResult(
                (LibrarySearchResult[]) listLibrayLinksHandler.Handle(new ListLibraryLinks(message.UserId,
                    message.AggregateId)),
                (LibrarySearchResult[]) listReceivedLinksHandler.Handle(
                    new ListReceivedLinks.ListReceivedLinks(message.UserId, message.AggregateId)),
                (LibrarySearchResult[]) listRequestedLinksHandler.Handle(
                    new ListRequestedLinks.ListRequestedLinks(message.UserId, message.AggregateId)));
        }
    }
}
