using EventStore.ClientAPI;
using Joshilewis.Infrastructure.EventRouting;
using Joshilewis.Infrastructure.UnitOfWork;
using Lending.Execution;
using NHibernate;

namespace Tests
{
    public class TestUnitOfWork : EventStoreUnitOfWork
    {
        public TestUnitOfWork(IEventStoreConnection eventStoreConnection, IEventEmitter eventEmitter)
            : base(eventStoreConnection, eventEmitter)
        {
        }
    }
}