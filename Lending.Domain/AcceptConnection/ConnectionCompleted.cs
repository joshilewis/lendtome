using System;
using Lending.Cqrs;

namespace Lending.Domain.AcceptConnection
{
    public class ConnectionCompleted : Event
    {
        public Guid AcceptingUserId { get; set; }

        public ConnectionCompleted(Guid processId, Guid aggregateId, Guid acceptingUserId)
            : base(processId, aggregateId)
        {
            AcceptingUserId = acceptingUserId;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            if (!base.Equals(obj)) return false;
            var other = (ConnectionCompleted)obj;
            return AcceptingUserId.Equals(other.AcceptingUserId);

        }
    }
}