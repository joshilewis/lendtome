using System;
using Lending.Cqrs;

namespace Lending.Domain.AcceptConnection
{
    public class ConnectionAccepted : Event
    {
        public Guid RequestingUserId { get; set; }

        public ConnectionAccepted(Guid processId, Guid aggregateId, Guid requestingUserId)
            : base(processId, aggregateId)
        {
            RequestingUserId = requestingUserId;
        }
    }
}