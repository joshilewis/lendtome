using System;

namespace Lending.Domain
{
    public abstract class Event
    {
        public Guid AggregateId { get; set; }
        public Guid ProcessId { get; set; }

        protected Event(Guid processId, Guid aggregateId)
        {
            AggregateId = aggregateId;
            ProcessId = processId;
        }

    }
}
