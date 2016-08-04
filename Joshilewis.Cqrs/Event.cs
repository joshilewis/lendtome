using System;

namespace Joshilewis.Cqrs
{
    public abstract class Event
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public Guid ProcessId { get; set; }

        protected Event(Guid processId, Guid aggregateId)
        {
            Id = Guid.NewGuid();
            AggregateId = aggregateId;
            ProcessId = processId;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Event)obj;
            return AggregateId.Equals(other.AggregateId) &&
                   ProcessId.Equals(other.ProcessId);

        }

        public override int GetHashCode()
        {
            int result = base.GetHashCode();
            result = (result * 397) ^ Id.GetHashCode();
            result = (result * 397) ^ AggregateId.GetHashCode();
            result = (result * 397) ^ ProcessId.GetHashCode();
            return result;
        }
    }
}
