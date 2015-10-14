using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs;
using Lending.Domain;

namespace Lending.Execution
{
    public class InMemoryEventEmitter : IEventEmitter
    {
        private readonly InMemoryEventConsumer inMemoryEventConsumer;

        public InMemoryEventEmitter(InMemoryEventConsumer inMemoryEventConsumer)
        {
            this.inMemoryEventConsumer = inMemoryEventConsumer;
        }


        public void EmitEvent(Event @event)
        {
            inMemoryEventConsumer.ConsumeEvent(@event);
        }

        public void EmitEvents(IEnumerable<Event> events)
        {
            foreach (var @event in events)
            {
                EmitEvent(@event);
            }
        }
    }
}
