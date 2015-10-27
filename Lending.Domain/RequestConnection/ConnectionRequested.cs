using System;
using Lending.Cqrs;

namespace Lending.Domain.RequestConnection
{
    public class ConnectionRequested : Event
    {
        public Guid TargetUserId { get; set; }

        public ConnectionRequested(Guid processId, Guid aggregateId, Guid targetUserId)
            : base(processId, aggregateId)
        {
            TargetUserId = targetUserId;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            if (!base.Equals(obj)) return false;
            var other = (ConnectionRequested)obj;
            return TargetUserId.Equals(other.TargetUserId);
        }

    }
}