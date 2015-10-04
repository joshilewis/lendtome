using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using EventStore.ClientAPI;
using Lending.Domain;
using Lending.Execution.EventStore;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Context;
using ServiceStack.Text;

namespace Lending.Execution.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private const string EventClrTypeHeader = "EventClrTypeName";
        private const string AggregateClrTypeHeader = "AggregateClrTypeName";
        private const string CommitIdHeader = "CommitId";
        private const int WritePageSize = 500;
        private const int ReadPageSize = 500;

        //private readonly static ILog Log = LogManager.GetLogger(typeof(UnitOfWork).FullName);

        private readonly ISessionFactory sessionFactory;
        private readonly ConcurrentQueue<Aggregate> aggregateQueue;
        private readonly IEventStoreConnection connection;
        private readonly Guid transactionId;

        public UnitOfWork(ISessionFactory sessionFactory, string eventStoreIpAddress)
        {
            //Log.DebugFormat("Creating unit of work {0}", GetHashCode());
            this.sessionFactory = sessionFactory;
            aggregateQueue = new ConcurrentQueue<Aggregate>();
            connection = EventStoreConnection.Create(new IPEndPoint(IPAddress.Parse(eventStoreIpAddress), 1113));
            transactionId = Guid.NewGuid();
        }

        public void Begin()
        {
            //Log.DebugFormat("Beginning unit of work {0}", GetHashCode());
            currentSession = sessionFactory.OpenSession();
            CurrentSessionContext.Bind(CurrentSession);
            CurrentSession.BeginTransaction();
            connection.ConnectAsync().Wait();
        }

        public void Commit()
        {
            SaveAggregatesInQueue();

            //Log.DebugFormat("Committing unit of work {0}", GetHashCode());
            currentSession.Transaction.Commit();
            CurrentSessionContext.Unbind(sessionFactory);
        }

        private void SaveAggregatesInQueue()
        {
            foreach (Aggregate aggregate in aggregateQueue)
            {
                SaveAggregate(aggregate);
            }
        }

        private void SaveAggregate(Aggregate aggregate)
        {
            //Taken from https://github.com/pgermishuys/getting-started-with-event-store/blob/master/src/GetEventStoreRepository/GetEventStoreRepository.cs
            var commitHeaders = new Dictionary<string, object>
            {
                {CommitIdHeader, transactionId},
                {AggregateClrTypeHeader, aggregate.GetType().AssemblyQualifiedName}
            };

            var newEvents = aggregate.GetUncommittedEvents().Cast<object>().ToList();
            var originalVersion = aggregate.Version - newEvents.Count;
            var expectedVersion = originalVersion == 0 ? ExpectedVersion.NoStream : originalVersion;
            var eventsToSave = newEvents.Select(e => ToEventData(Guid.NewGuid(), e, commitHeaders)).ToList();

            connection.AppendToStreamAsync(aggregate.Stream, expectedVersion, eventsToSave).Wait();

            aggregate.ClearUncommittedEvents();

        }

        private static EventData ToEventData(Guid eventId, object evnt, IDictionary<string, object> headers)
        {
            var serializerSettings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.None};
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(evnt, serializerSettings));

            var eventHeaders = new Dictionary<string, object>(headers)
            {
                {
                    EventClrTypeHeader, evnt.GetType().AssemblyQualifiedName
                }
            };
            var metadata = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventHeaders, serializerSettings));
            var typeName = evnt.GetType().Name;

            return new EventData(eventId, typeName, true, data, metadata);
        }

        public void RollBack()
        {
            //Log.DebugFormat("Rolling back unit of work {0}", GetHashCode());
            currentSession.Transaction.Rollback();
            CurrentSessionContext.Unbind(sessionFactory);
        }

        public void Dispose()
        {
            //Log.DebugFormat("Disposing {0}", GetHashCode());
            connection.Close();
            connection.Dispose();
            currentSession.Transaction.Dispose();
            CurrentSession.Dispose();
        }

        private ISession currentSession;
        public ISession CurrentSession => currentSession;

        public ConcurrentQueue<Aggregate> Queue => aggregateQueue;
    }
}
