using System;
using System.Collections.Generic;
using Lending.Cqrs;

namespace Lending.Execution
{
    public interface IEventHandlerProvider
    {
        IEnumerable<IEventHandler> GetEventHandlers(Type type);
    }
}