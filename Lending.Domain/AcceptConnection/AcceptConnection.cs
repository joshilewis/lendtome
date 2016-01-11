using System;
using Lending.Cqrs;
using Lending.Cqrs.Command;

namespace Lending.Domain.AcceptConnection
{
    public class AcceptConnection : AuthenticatedCommand
    {
        public Guid RequestingUserId { get; set; }

        public AcceptConnection(Guid processId, Guid aggregateId, Guid userId, Guid requestingUserId)
            : base(processId, aggregateId, userId)
        {
            RequestingUserId = requestingUserId;
        }

        public AcceptConnection()
        {
            
        }
    }
}