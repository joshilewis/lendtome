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

namespace Lending.Execution
{
    public class InMemoryEventConsumer
    {
        private readonly BlockingCollection<Event> eventQueue;
        private readonly IEnumerable<IEventHandler> eventHandlers;
        private readonly IUnitOfWork unitOfWork;

        public InMemoryEventConsumer(BlockingCollection<Event> eventQueue, IEnumerable<IEventHandler> eventHandlers,
            IUnitOfWork unitOfWork)
        {
            this.eventQueue = eventQueue;
            this.eventHandlers = eventHandlers;
            this.unitOfWork = unitOfWork;
        }

        public void Start()
        {
            while (true)
            {
                Event @event = eventQueue.Take();
                IEnumerable<IEventHandler> matchingHandlers = eventHandlers.Where(x => x.GetType().GetInterfaces().Any(y => y.GetGenericArguments().Contains(@event.GetType())));

                foreach (IEventHandler matchingHandler in matchingHandlers)
                {
                    unitOfWork.DoInTransaction(() => ((dynamic) matchingHandler).When(@event));
                }

            }
        }
    }
}
