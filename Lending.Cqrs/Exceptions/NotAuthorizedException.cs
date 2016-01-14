using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lending.Cqrs.Exceptions
{
    public class NotAuthorizedException : Exception
    {
        public Guid UserId { get; set; }
        public Guid AggregateId { get; set; }
        public Type AggregateType { get; set; }

        public NotAuthorizedException(Guid userId, Guid aggregateId, Type aggregateType)
            : base($"User {userId} is not authorized for {aggregateType} {aggregateId}")
        {
            UserId = userId;
            AggregateId = aggregateId;
            AggregateType = aggregateType;
        }
    }
}
