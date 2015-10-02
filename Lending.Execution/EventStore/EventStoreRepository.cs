using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using EventStore.ClientAPI;
using Lending.Domain;
using ServiceStack.Text;

namespace Lending.Execution.EventStore
{
    public class EventStoreRepository : IRepository
    {
        private readonly ConcurrentQueue<StreamEventTuple> eventQueue;

        public EventStoreRepository(ConcurrentQueue<StreamEventTuple> eventQueue)
        {
            this.eventQueue = eventQueue;
        }

        public ConcurrentQueue<StreamEventTuple> Queue { get { return eventQueue; } }

        public TAggregate GetById<TAggregate>(Guid id) where TAggregate : Aggregate
        {
            throw new NotImplementedException();
        }

        public TAggregate GetById<TAggregate>(Guid id, int version) where TAggregate : Aggregate
        {
            throw new NotImplementedException();
        }

        public void Save(Aggregate aggregate, Guid commitId, Action<IDictionary<string, object>> updateHeaders)
        {
            throw new NotImplementedException();
        }

        public void EmitEvent(string stream, Event @event)
        {
            eventQueue.Enqueue(new StreamEventTuple(stream, @event));
        }

    }
}