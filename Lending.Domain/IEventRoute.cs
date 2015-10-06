using System;

namespace Lending.Domain
{
    public interface IEventRoute
    {
        Type HandlerType { get; }
        void Handle(object @event);
    }
}