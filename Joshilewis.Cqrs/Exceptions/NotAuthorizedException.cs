using System;

namespace Joshilewis.Cqrs.Exceptions
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
