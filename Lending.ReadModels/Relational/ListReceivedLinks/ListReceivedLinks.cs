using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Joshilewis.Cqrs.Query;

namespace Lending.ReadModels.Relational.ListReceivedLinks
{
    public class ListReceivedLinks : AuthenticatedQuery
    {
        public Guid AggregateId { get; set; }

        public ListReceivedLinks(string userId, Guid libraryId) 
            : base(userId)
        {
            AggregateId = libraryId;
        }

        protected ListReceivedLinks()
        {
        }
    }
}
