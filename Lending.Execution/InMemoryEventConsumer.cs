using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Joshilewis.Cqrs;
using Lending.Domain;
using Lending.Execution.UnitOfWork;
using NHibernate;

namespace Lending.Execution
{
    public class InMemoryEventConsumer
    {
        private readonly IEventHandlerProvider eventHandlerProvider;

        public InMemoryEventConsumer(IEventHandlerProvider eventHandlerProvider)
        {
            this.eventHandlerProvider = eventHandlerProvider;
        }

        public void ConsumeEvent(Event @event)
        {

            foreach (IEventHandler handler in eventHandlerProvider.GetEventHandlers(@event.GetType()))
            {
                handler.When(@event);
            }
        }
    }
}
