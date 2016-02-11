using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.Core;
using Joshilewis.Cqrs;
using NUnit.Framework;
using static Tests.FixtureExtensions.DIExtensions;
namespace Tests.FixtureExtensions
{
    public static class EventStoreExtensions
    {
        public static void SetUpEventStore()
        {
            Container.GetInstance<ClusterVNode>().Start();
            Container.GetInstance<IEventStoreConnection>().ConnectAsync().Wait();

        }

        public static void TearDownEventStore()
        {
            Container.GetInstance<IEventStoreConnection>().Close();
            Container.GetInstance<IEventStoreConnection>().Dispose();
            Container.GetInstance<ClusterVNode>().Stop();

        }

        public static void AndEventsSavedForAggregate<TAggregate>(Guid aggregateId, params Event[] expectedEvents) where TAggregate : Aggregate
        {
            IEventRepository eventRepository = Container.GetInstance<IEventRepository>();
            IEnumerable<Event> actualEvents = eventRepository.GetEventsForAggregate<TAggregate>(aggregateId);
            foreach (Event @event in actualEvents)
            {
                @event.ProcessId = Guid.Empty;
            }
            Assert.That(actualEvents, Is.EquivalentTo(expectedEvents));
        }
    }
}
