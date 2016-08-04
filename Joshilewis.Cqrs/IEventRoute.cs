using System;

namespace Joshilewis.Cqrs
{
    public interface IEventRoute
    {
        Type HandlerType { get; }
        void Handle(object @event);
    }
}