namespace Lending.Cqrs
{
    public abstract class EventHandler<TEvent> : IEventHandler<TEvent> where TEvent : Event
    {
        public abstract void When(TEvent @event);

        public void When(Event @event)
        {
            When((TEvent) @event);
        }
    }
}