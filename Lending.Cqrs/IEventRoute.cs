using System;

namespace Lending.Cqrs
{
    public interface IEventRoute
    {
        Type HandlerType { get; }
        void Handle(object @event);
    }
}