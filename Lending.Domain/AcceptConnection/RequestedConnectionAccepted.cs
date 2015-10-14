using System;
using Lending.Cqrs;

namespace Lending.Domain.AcceptConnection
{
    public class RequestedConnectionAccepted : Event
    {
        public Guid AcceptingUserId { get; set; }

        public RequestedConnectionAccepted(Guid processId, Guid aggregateId, Guid acceptingUserId)
            : base(processId, aggregateId)
        {
            AcceptingUserId = acceptingUserId;
        }
    }
}