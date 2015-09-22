using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using EventStore.ClientAPI;
using Lending.Core;
using ServiceStack.Text;

namespace Lending.Execution.EventStore
{
    public class EventStoreEventEmitter : IEventEmitter
    {
        private readonly ConcurrentQueue<StreamEventTuple> eventQueue;

        public EventStoreEventEmitter(ConcurrentQueue<StreamEventTuple> eventQueue)
        {
            this.eventQueue = eventQueue;
        }

        public ConcurrentQueue<StreamEventTuple> Queue { get { return eventQueue; } }

        public void EmitEvent(string stream, Event @event)
        {
            eventQueue.Enqueue(new StreamEventTuple(stream, @event));
        }

    }
}