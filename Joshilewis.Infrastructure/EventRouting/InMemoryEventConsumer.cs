using Joshilewis.Cqrs;

namespace Joshilewis.Infrastructure.EventRouting
{
    public class InMemoryEventConsumer
    {
        private readonly IEventHandlerProvider eventHandlerProvider;

        public InMemoryEventConsumer(IEventHandlerProvider eventHandlerProvider)
        {
            this.eventHandlerProvider = eventHandlerProvider;
        }

        public void ConsumeEvent(Event @event)
        {

            foreach (IEventHandler handler in eventHandlerProvider.GetEventHandlers(@event.GetType()))
            {
                handler.When(@event);
            }
        }
    }
}
