using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.Core;
using Lending.Cqrs;
using NUnit.Framework;
using static Tests.FixtureExtensions.DIExtensions;
namespace Tests.FixtureExtensions
{
    public static class EventStoreExtensions
    {
        public static void SetUpEventStore()
        {
            GetContainer().GetInstance<ClusterVNode>().Start();
            GetContainer().GetInstance<IEventStoreConnection>().ConnectAsync().Wait();

        }

        public static void TearDownEventStore()
        {
            GetContainer().GetInstance<IEventStoreConnection>().Close();
            GetContainer().GetInstance<IEventStoreConnection>().Dispose();
            GetContainer().GetInstance<ClusterVNode>().Stop();

        }

        private static IEventRepository EventRepository => GetContainer().GetInstance<IEventRepository>();
        public static void AndEventsSavedForAggregate<TAggregate>(Guid aggregateId, params Event[] expectedEvents) where TAggregate : Aggregate
        {
            IEnumerable<Event> actualEvents = EventRepository.GetEventsForAggregate<TAggregate>(aggregateId);
            foreach (Event @event in actualEvents)
            {
                @event.ProcessId = Guid.Empty;
            }
            Assert.That(actualEvents, Is.EquivalentTo(expectedEvents));
        }
    }
}
