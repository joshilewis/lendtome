using EventStore.ClientAPI;
using Joshilewis.Infrastructure.EventRouting;
using Joshilewis.Infrastructure.UnitOfWork;
using Lending.Execution;
using NHibernate;

namespace Tests
{
    public class TestUnitOfWork : UnitOfWork
    {
        public TestUnitOfWork(ISessionFactory sessionFactory, IEventStoreConnection eventStoreConnection,
            IEventEmitter eventEmitter, EventDispatcher eventDispatcher)
            : base(sessionFactory, eventStoreConnection, eventEmitter, eventDispatcher)
        {
        }
    }
}