using System;
using Lending.Cqrs;

namespace Lending.Domain.RequestConnection
{
    public class InitiateConnectionAcceptance : Command
    {
        public Guid RequestingUserId { get; set; }

        public InitiateConnectionAcceptance(Guid processId, Guid aggregateId, Guid requestingUserId) : base(processId, aggregateId)
        {
            RequestingUserId = requestingUserId;
        }
    }
}