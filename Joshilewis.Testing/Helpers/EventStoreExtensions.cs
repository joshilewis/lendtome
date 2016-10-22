using System;
using System.Collections.Generic;
using EventStore.ClientAPI;
using EventStore.Core;
using Joshilewis.Cqrs;
using Joshilewis.Infrastructure.UnitOfWork;
using NUnit.Framework;

namespace Joshilewis.Testing.Helpers
{
    public static class EventStoreExtensions
    {
        public static void SetUpEventStore()
        {
            DIExtensions.Container.GetInstance<ClusterVNode>().Start();
            DIExtensions.Container.GetInstance<IEventStoreConnection>().ConnectAsync().Wait();

        }

        public static void TearDownEventStore()
        {
            DIExtensions.Container.GetInstance<IEventStoreConnection>().Close();
            DIExtensions.Container.GetInstance<IEventStoreConnection>().Dispose();
            DIExtensions.Container.GetInstance<ClusterVNode>().Stop();

        }

        public static void EventsSavedForAggregate<TAggregate>(Guid aggregateId, params Event[] expectedEvents) where TAggregate : Aggregate
        {
            IEnumerable<Event> actualEvents = new Event[] {};
            EventStoreUnitOfWork unitOfWork = DIExtensions.Container.GetInstance<EventStoreUnitOfWork>();
            unitOfWork.DoInTransaction(() =>
            {
                IEventRepository eventRepository = unitOfWork.EventRepository;
                actualEvents = eventRepository.GetEventsForAggregate<TAggregate>(aggregateId);
            });

            foreach (Event @event in actualEvents)
            {
                @event.ProcessId = Guid.Empty;
            }
            Assert.That(actualEvents, Is.EquivalentTo(expectedEvents));
        }
    }
}
