using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs;
using Lending.Domain;
using Lending.Execution.UnitOfWork;
using NHibernate;

namespace Lending.Execution
{
    public class InMemoryEventConsumer
    {
        private readonly Dictionary<Type, HashSet<IEventHandler>> eventHandlerMap;

        public InMemoryEventConsumer()
        {
            eventHandlerMap = new Dictionary<Type, HashSet<IEventHandler>>();
        }

        public void RegisterHandler<TEvent>(IEventHandler handler) where TEvent : Event
        {
            if (!eventHandlerMap.ContainsKey(typeof(TEvent))) eventHandlerMap.Add(typeof(TEvent), new HashSet<IEventHandler>());
            eventHandlerMap[typeof (TEvent)].Add(handler);
        }

        public void ConsumeEvent(Event @event)
        {
            if (!eventHandlerMap.ContainsKey(@event.GetType())) return;
            foreach (IEventHandler handler in eventHandlerMap[@event.GetType()])
            {
                handler.When(@event);
            }
        }
    }
}
