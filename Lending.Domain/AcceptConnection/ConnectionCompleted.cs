using System;
using Lending.Cqrs;

namespace Lending.Domain.AcceptConnection
{
    public class ConnectionCompleted : Event
    {
        public Guid AcceptingUserId { get; set; }

        public ConnectionCompleted(Guid processId, Guid aggregateId, Guid acceptingUserId)
            : base(processId, aggregateId)
        {
            AcceptingUserId = acceptingUserId;
        }
    }
}