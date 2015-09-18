using System;
using System.Text;
using EventStore.ClientAPI;
using Lending.Core;
using ServiceStack.Text;

namespace Lending.Execution.EventStore
{
    public class EventStoreEventEmitter : IEventEmitter
    {
        private readonly IEventStoreConnection eventStoreConnection;

        public EventStoreEventEmitter(IEventStoreConnection eventStoreConnection)
        {
            this.eventStoreConnection = eventStoreConnection;
        }

        public void EmitEvent(Event @event)
        {
            string streamName = String.Format("{0}-{1}", @event.GetType().Name, @event.Id);
            eventStoreConnection.AppendToStreamAsync(streamName, ExpectedVersion.Any, AsJson(@event)).Wait();
        }

        private static EventData AsJson(object value)
        {
            if (value == null) throw new ArgumentNullException("value");

            var json = value.ToJson();
            var data = Encoding.UTF8.GetBytes(json);
            var eventName = value.GetType().Name;

            return new EventData(Guid.NewGuid(), eventName, true, data, new byte[] {});
        }

    }
}