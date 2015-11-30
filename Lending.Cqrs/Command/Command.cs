using System;

namespace Lending.Cqrs.Command
{
    /// <summary>
    /// Empty class used for DI registration
    /// </summary>
    public abstract class Command : Message
    {
        public Guid AggregateId { get; set; }
        public Guid ProcessId { get; set; }

        protected Command(Guid processId, Guid aggregateId)
        {
            AggregateId = aggregateId;
            ProcessId = processId;
        }

        protected Command()
        {
        }
    }
}
