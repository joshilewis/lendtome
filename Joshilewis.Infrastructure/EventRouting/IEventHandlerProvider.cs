using System;
using System.Collections.Generic;
using Joshilewis.Cqrs;

namespace Joshilewis.Infrastructure.EventRouting
{
    public interface IEventHandlerProvider
    {
        IEnumerable<IEventHandler> GetEventHandlers(Type type);
    }
}