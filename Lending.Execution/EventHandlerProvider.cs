using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs;

namespace Lending.Execution
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
