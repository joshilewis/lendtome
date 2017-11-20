using System;

namespace Joshilewis.Cqrs.Exceptions
{
    public class NotAuthorizedException : Exception
    {
        public string UserId { get; set; }
        public Guid AggregateId { get; set; }
        public Type AggregateType { get; set; }

        public NotAuthorizedException(string userId, Guid aggregateId, Type aggregateType)
            : base($"User {userId} is not authorized for {aggregateType} {aggregateId}")
        {
            UserId = userId;
            AggregateId = aggregateId;
            AggregateType = aggregateType;
        }
    }
}
