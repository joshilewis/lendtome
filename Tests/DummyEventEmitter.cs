using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs;
using Lending.Domain;
using Lending.Execution;

namespace Tests
{
    public class DummyEventEmitter : IEventEmitter
    {
        public void EmitEvent(Event @event)
        {
            
        }

        public void EmitEvents(IEnumerable<Event> events)
        {
        }
    }
}
