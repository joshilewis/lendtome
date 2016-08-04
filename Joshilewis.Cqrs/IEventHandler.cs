namespace Joshilewis.Cqrs
{
    public interface IEventHandler
    {
        void When(Event @event);

    }
    public interface IEventHandler<in TEvent> : IEventHandler where TEvent : Event
    {
        void When(TEvent @event);
    }
}
