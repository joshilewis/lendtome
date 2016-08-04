using System;
using System.Collections.Generic;
using System.Linq;
using Joshilewis.Cqrs;

namespace Joshilewis.Infrastructure.EventRouting
{
    public class EventHandlerProvider : IEventHandlerProvider
    {
        private readonly Func<Type, IEnumerable<IEventHandler>> eventHandlerFunc;

        public EventHandlerProvider(Func<Type, IEnumerable<IEventHandler>> eventHandlerFunc)
        {
            this.eventHandlerFunc = eventHandlerFunc;
        }

        public IEnumerable<IEventHandler> GetEventHandlers(Type type)
        {
            return eventHandlerFunc(type).Distinct();
        }
    }
}
