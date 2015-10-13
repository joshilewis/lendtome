using System;

namespace Lending.Domain.RequestConnection
{
    public class RequestConnection : AuthenticatedCommand
    {
        public Guid TargetUserId { get; set; }

        public RequestConnection(Guid processId, Guid aggregateId, Guid userId, Guid targetUserId)
            : base(aggregateId, processId, userId)
        {
            TargetUserId = targetUserId;
        }
    }
}