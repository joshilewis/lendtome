using System;
using Lending.Cqrs;
using Lending.Cqrs.Command;

namespace Lending.Domain.RequestConnection
{
    public class RequestConnection : AuthenticatedCommand
    {
        public Guid TargetUserId { get; set; }

        public RequestConnection(Guid processId, Guid aggregateId, Guid userId, Guid targetUserId)
            : base(processId, aggregateId, userId)
        {
            TargetUserId = targetUserId;
        }
    }
}