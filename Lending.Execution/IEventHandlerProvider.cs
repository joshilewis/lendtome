using System;
using System.Collections.Generic;
using Joshilewis.Cqrs;

namespace Lending.Execution
{
    public interface IEventHandlerProvider
    {
        IEnumerable<IEventHandler> GetEventHandlers(Type type);
    }
}