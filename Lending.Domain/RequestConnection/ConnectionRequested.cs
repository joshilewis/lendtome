using System;
using Lending.Cqrs;

namespace Lending.Domain.RequestConnection
{
    public class ConnectionRequested : Event
    {
        public Guid TargetUserId { get; set; }

        public ConnectionRequested(Guid processId, Guid aggregateId, Guid targetUserId)
            : base(processId, aggregateId)
        {
            TargetUserId = targetUserId;
        }
    }
}