using System.Collections.Generic;
using Joshilewis.Cqrs;

namespace Joshilewis.Infrastructure.EventRouting
{
    public interface IEventEmitter
    {
        void EmitEvent(Event @event);
        void EmitEvents(IEnumerable<Event> events);
    }
}
