using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Joshilewis.Cqrs.Query;

namespace Lending.ReadModels.Relational.ListLibraryStatus
{
    public class ListLibraryStatus : AuthenticatedQuery
    {
        public Guid AggregateId { get; set; }

        public ListLibraryStatus(string userId, Guid aggregateId)
            : base(userId)
        {
            AggregateId = aggregateId;
        }

        protected ListLibraryStatus()
        {
        }
    }
}
