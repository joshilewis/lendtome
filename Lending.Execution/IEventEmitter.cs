using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Joshilewis.Cqrs;
using Lending.Domain;

namespace Lending.Execution
{
    public interface IEventEmitter
    {
        void EmitEvent(Event @event);
        void EmitEvents(IEnumerable<Event> events);
    }
}
