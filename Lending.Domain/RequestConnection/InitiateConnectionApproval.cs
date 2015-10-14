using System;
using Lending.Cqrs;

namespace Lending.Domain.RequestConnection
{
    public class InitiateConnectionApproval : Command
    {
        public Guid RequestingUserId { get; set; }

        public InitiateConnectionApproval(Guid processId, Guid aggregateId, Guid requestingUserId) : base(processId, aggregateId)
        {
            RequestingUserId = requestingUserId;
        }
    }
}