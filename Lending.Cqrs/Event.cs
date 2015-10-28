using System;

namespace Lending.Cqrs
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

    }
}
