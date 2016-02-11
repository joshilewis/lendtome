using System;
using System.Collections.Generic;
using Joshilewis.Cqrs;
using Lending.Execution;

namespace Tests
{
    public class DummyEventHandlerProvider : IEventHandlerProvider
    {
        private readonly Dictionary<Type, HashSet<IEventHandler>> eventHandlerMap;

        public DummyEventHandlerProvider()
        {
            eventHandlerMap = new Dictionary<Type, HashSet<IEventHandler>>();
        }

        public IEnumerable<IEventHandler> GetEventHandlers(Type type)
        {
            if (!eventHandlerMap.ContainsKey(type)) return new IEventHandler[] {};
            return eventHandlerMap[type];
        }

        public void RegisterHandler<TEvent>(IEventHandler handler) where TEvent : Event
        {
            if (!eventHandlerMap.ContainsKey(typeof (TEvent)))
                eventHandlerMap.Add(typeof (TEvent), new HashSet<IEventHandler>());

            eventHandlerMap[typeof (TEvent)].Add(handler);
        }
    }
}