using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Joshilewis.Cqrs;

namespace Joshilewis.Infrastructure.EventRouting
{
    public abstract class EventRepository : IEventRepository, IDisposable
    {
        public ConcurrentQueue<Aggregate> Queue { get; private set; }
        private readonly IEventEmitter eventEmitter;

        protected EventRepository(IEventEmitter eventEmitter)
        {
            this.eventEmitter = eventEmitter;
            this.Queue = new ConcurrentQueue<Aggregate>();
        }

        public abstract IEnumerable<Event> GetEventsForAggregate<TAggregate>(Guid id) where TAggregate : Aggregate;
        public abstract IEnumerable<Event> GetEventsForAggregate<TAggregate>(Guid id, int version) where TAggregate : Aggregate;

        public virtual void Save(Aggregate aggregate)
        {
            Queue.Enqueue(aggregate);
        }

        public void Commit(Guid transactionId)
        {
            var eventsToEmit = new List<Event>();
            foreach (Aggregate aggregate in Queue)
            {
                eventsToEmit.AddRange(aggregate.GetUncommittedEvents());
                SaveAggregate(aggregate, transactionId);
            }

            eventEmitter.EmitEvents(eventsToEmit);

            Queue = new ConcurrentQueue<Aggregate>(); //https://social.msdn.microsoft.com/Forums/en-US/accf4254-ee81-4059-9251-619bc6bbeadf/clear-a-concurrentqueue?forum=rx
        }

        protected abstract void SaveAggregate(Aggregate aggregate, Guid transactionId);

        public void Dispose()
        {
            Queue = null;
        }
    }
}