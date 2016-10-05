using System;
using System.Net;
using EventStore.ClientAPI;
using Joshilewis.Cqrs;
using Joshilewis.Infrastructure.EventRouting;
using Joshilewis.Infrastructure.EventStore;

namespace Joshilewis.Infrastructure.UnitOfWork
{
    public class EventStoreUnitOfWork : IUnitOfWork//, ICommandUnitOfWork
    {
        //private readonly static ILog Log = LogManager.GetLogger(typeof(EventStoreUnitOfWork).FullName);

        private readonly IEventStoreConnection connection;
        private readonly Guid transactionId;
        private readonly IEventEmitter eventEmitter;
        private EventStoreEventRepository eventRepository;

        public EventStoreUnitOfWork(string eventStoreIpAddress, IEventEmitter eventEmitter)
            : this(EventStoreConnection.Create(new IPEndPoint(IPAddress.Parse(eventStoreIpAddress), 1113)), eventEmitter)
        {
        }

        protected EventStoreUnitOfWork(IEventStoreConnection eventStoreConnection, IEventEmitter eventEmitter)
        {
            this.eventEmitter = eventEmitter;
            connection = eventStoreConnection;
            transactionId = Guid.NewGuid();
        }

        public void Begin()
        {
            //Log.DebugFormat("Beginning unit of work {0}", GetHashCode());
            connection.ConnectAsync().Wait();
            eventRepository = new EventStoreEventRepository(eventEmitter, connection);
        }

        public void Commit()
        {
            eventRepository.Commit(transactionId);
        }

        public void RollBack()
        {
        }

        public void Dispose()
        {
            //Log.DebugFormat("Disposing {0}", GetHashCode());
            eventRepository.Dispose();
            connection.Close();
            connection.Dispose();
        }

        public IEventRepository EventRepository
        {
            get { return eventRepository; }
        }
    }
}
