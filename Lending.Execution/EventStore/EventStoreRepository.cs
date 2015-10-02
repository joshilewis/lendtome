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
        public EventStoreRepository(ConcurrentQueue<Aggregate> aggregateQueue)
        {
            this.Queue = aggregateQueue;
        }

        public ConcurrentQueue<Aggregate> Queue { get; }

        public TAggregate GetById<TAggregate>(Guid id) where TAggregate : Aggregate
        {
            throw new NotImplementedException();
        }

        public TAggregate GetById<TAggregate>(Guid id, int version) where TAggregate : Aggregate
        {
            throw new NotImplementedException();
        }

        public void Save(Aggregate aggregate)
        {
            Queue.Enqueue(aggregate);
        }

    }
}