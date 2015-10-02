using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Text;
using EventStore.ClientAPI;
using Lending.Domain;
using Lending.Execution.EventStore;
using NHibernate;
using NHibernate.Context;
using ServiceStack.Text;

namespace Lending.Execution.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
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
            foreach (Aggregate aggregate in aggregateQueue)
            {
                foreach (var uncommittedEvent in aggregate.GetUncommittedEvents())
                {
                    connection.AppendToStreamAsync(aggregate.Stream, ExpectedVersion.Any,
                        uncommittedEvent.AsJson()).Wait();
                }
            }

            //Log.DebugFormat("Committing unit of work {0}", GetHashCode());
            currentSession.Transaction.Commit();
            CurrentSessionContext.Unbind(sessionFactory);
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
