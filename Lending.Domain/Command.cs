using System;

namespace Lending.Domain
{
    /// <summary>
    /// Empty class used for DI registration
    /// </summary>
    public abstract class Command
    {
        public Guid AggregateId { get; set; }
        public Guid ProcessId { get; set; }

        protected Command(Guid aggregateId, Guid processId)
        {
            AggregateId = aggregateId;
            ProcessId = processId;
        }
    }
}
