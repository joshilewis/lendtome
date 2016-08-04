using System;
using Joshilewis.Cqrs;

namespace Lending.Domain.RequestLink
{
    public class LinkRequested : Event
    {
        public Guid TargetLibraryId { get; set; }

        public LinkRequested(Guid processId, Guid aggregateId, Guid targetLibraryId)
            : base(processId, aggregateId)
        {
            TargetLibraryId = targetLibraryId;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            if (!base.Equals(obj)) return false;
            var other = (LinkRequested)obj;
            return TargetLibraryId.Equals(other.TargetLibraryId);
        }

        public override int GetHashCode()
        {
            return (base.GetHashCode() * 397) ^ TargetLibraryId.GetHashCode();
        }
    }
}