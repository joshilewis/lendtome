using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Lending.Cqrs;
using Lending.Domain;
using ServiceStack.Text;

namespace Lending.Execution.EventStore
{
    public static class EventExtensions
    {
        public static EventData AsJson(this Event value)
        {
            if (value == null) throw new ArgumentNullException("value");

            var json = value.ToJson();
            var data = Encoding.UTF8.GetBytes(json);
            var eventName = value.GetType().Name;

            return new EventData(Guid.NewGuid(), eventName, true, data, new byte[] { });
        }

    }
}
