namespace Lending.Cqrs
{
    public interface IEventHandler { }
    public interface IEventHandler<in TEvent> : IEventHandler where TEvent : Event
    {
        void When(TEvent @event);
    }
}
