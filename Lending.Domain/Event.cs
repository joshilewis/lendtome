using System;

namespace Lending.Domain
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

    }
}
