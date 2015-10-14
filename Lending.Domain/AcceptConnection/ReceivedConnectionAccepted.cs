using System;
using Lending.Cqrs;

namespace Lending.Domain.AcceptConnection
{
    public class ReceivedConnectionAccepted : Event
    {
        public Guid RequestingUserId { get; set; }

        public ReceivedConnectionAccepted(Guid processId, Guid aggregateId, Guid requestingUserId)
            : base(processId, aggregateId)
        {
            RequestingUserId = requestingUserId;
        }
    }
}