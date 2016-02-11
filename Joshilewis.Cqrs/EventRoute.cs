using System;

namespace Joshilewis.Cqrs
{
    public class EventRoute<T> : IEventRoute
    {
        private Action<T> Handler { get; set; }
        public Type HandlerType { get; private set; }
        public EventRoute(Action<T> handler, Type handlerType)
        {
            this.Handler = handler;
            this.HandlerType = handlerType;
        }
        public void Handle(object @event)
        {
            Handler((dynamic)@event);
        }
        public override string ToString()
        {
            return HandlerType.Name;
        }
    }
}
