using System;
using System.Text;
using EventStore.ClientAPI;
using Joshilewis.Cqrs;
using ServiceStack.Text;

namespace Joshilewis.Infrastructure.EventStore
{
    public static class EventExtensions
    {
        public static EventData AsJson(this Event value)
        {
            if (value == null) throw new ArgumentNullException("value");

            var json = value.ToJson();
            var data = Encoding.UTF8.GetBytes((string) json);
            var eventName = value.GetType().Name;

            return new EventData(Guid.NewGuid(), eventName, true, data, new byte[] { });
        }

    }
}
