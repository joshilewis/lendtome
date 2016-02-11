using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Joshilewis.Cqrs;
using Lending.Domain;

namespace Lending.Execution
{
    public class InMemoryEventEmitter : IEventEmitter
    {
        private readonly Queue<Event> eventsToEmit;

        public InMemoryEventEmitter()
        {
            eventsToEmit = new Queue<Event>();
        }

        public void EmitEvent(Event @event)
        {
            eventsToEmit.Enqueue(@event);
        }

        public void EmitEvents(IEnumerable<Event> events)
        {
            foreach (var @event in events)
            {
                EmitEvent(@event);
            }
        }

        public Queue<Event> EmittedEvents => eventsToEmit;
    }
}
