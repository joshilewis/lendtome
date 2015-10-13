using System;

namespace Lending.Domain.RequestConnection
{
    public class ConnectionRequested : Event
    {
        public Guid DesintationUserId { get; set; }

        public ConnectionRequested(Guid processId, Guid aggregateId, Guid desintationUserId)
            : base(processId, aggregateId)
        {
            DesintationUserId = desintationUserId;
        }
    }
}