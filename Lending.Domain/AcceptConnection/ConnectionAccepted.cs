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

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            if (!base.Equals(obj)) return false;
            var other = (ConnectionAccepted)obj;
            return RequestingUserId.Equals(other.RequestingUserId);
        }
    }
}