using System.Collections.Generic;
using Lending.Cqrs;

namespace Lending.Execution
{
    public class EventDispatcher
    {
        private readonly InMemoryEventEmitter eventEmitter;
        private readonly EventHandlerProvider eventHandlerProvider;

        public EventDispatcher(InMemoryEventEmitter eventEmitter, EventHandlerProvider eventHandlerProvider)
        {
            this.eventEmitter = eventEmitter;
            this.eventHandlerProvider = eventHandlerProvider;
        }

        public void DispatchEvents()
        {
            foreach (var emittedEvent in eventEmitter.EmittedEvents)
            {
                IEnumerable<IEventHandler> handlers = eventHandlerProvider.GetEventHandlers(emittedEvent.GetType());
                foreach (var eventHandler in handlers)
                {
                    eventHandler.When(emittedEvent);
                }
            }
            eventEmitter.EmittedEvents.Clear();
        }
    }
}