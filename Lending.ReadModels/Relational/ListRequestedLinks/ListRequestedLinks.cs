using System;
using Joshilewis.Cqrs.Query;

namespace Lending.ReadModels.Relational.ListRequestedLinks
{
    public class ListRequestedLinks : AuthenticatedQuery
    {
        public Guid AggregateId { get; set; }

        public ListRequestedLinks(string userId, Guid libraryId) 
            : base(userId)
        {
            AggregateId = libraryId;
        }

        protected ListRequestedLinks()
        {
            
        }
    }
}
