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
        private readonly ConcurrentQueue<Event> eventQueue;

        public EventStoreEventEmitter(ConcurrentQueue<Event> eventQueue)
        {
            this.eventQueue = eventQueue;
        }

        public ConcurrentQueue<Event> Queue { get { return eventQueue; } }

        public void EmitEvent(Event @event)
        {
            eventQueue.Enqueue(@event);
        }

    }
}