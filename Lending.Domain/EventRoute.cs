using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lending.Domain
{
    public class EventRoute<T> : IEventRoute
    {
        private Action<T> handler { get; set; }
        public Type HandlerType { get; private set; }
        public EventRoute(Action<T> handler, Type handlerType)
        {
            this.handler = handler;
            this.HandlerType = handlerType;
        }
        public void Handle(object @event)
        {
            handler((dynamic)@event);
        }
        public override string ToString()
        {
            return HandlerType.Name;
        }
    }
}
