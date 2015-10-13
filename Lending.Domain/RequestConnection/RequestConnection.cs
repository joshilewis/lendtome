using System;

namespace Lending.Domain.RequestConnection
{
    public class RequestConnection : AuthenticatedCommand
    {
        public Guid TargetUserId { get; set; }

        public RequestConnection(Guid aggregateId, Guid processId, Guid userId, Guid targetUserId)
            : base(aggregateId, processId, userId)
        {
            TargetUserId = targetUserId;
        }
    }
}