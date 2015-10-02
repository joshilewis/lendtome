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
        private readonly ConcurrentQueue<StreamEventTuple> eventQueue;
        private readonly IPEndPoint eventStoreEndPoint;

        public UnitOfWork(ISessionFactory sessionFactory, string eventStoreIpAddress)
        {
            //Log.DebugFormat("Creating unit of work {0}", GetHashCode());
            this.sessionFactory = sessionFactory;
            this.eventStoreEndPoint = new IPEndPoint(IPAddress.Parse(eventStoreIpAddress), 1113);
            eventQueue = new ConcurrentQueue<StreamEventTuple>();
        }

        public void Begin()
        {
            //Log.DebugFormat("Beginning unit of work {0}", GetHashCode());
            currentSession = sessionFactory.OpenSession();
            CurrentSessionContext.Bind(CurrentSession);
            CurrentSession.BeginTransaction();
        }

        public void Commit()
        {

            IEventStoreConnection connection = EventStoreConnection.Create(eventStoreEndPoint);
            connection.ConnectAsync().Wait();
            foreach (StreamEventTuple tuple in eventQueue)
            {
                connection.AppendToStreamAsync(tuple.Stream, ExpectedVersion.Any, tuple.Event.AsJson()).Wait();
            }
            connection.Close();
            connection.Dispose();

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
            currentSession.Transaction.Dispose();
            CurrentSession.Dispose();
        }

        private ISession currentSession;
        public ISession CurrentSession 
        {
            get
            {
                //Log.DebugFormat("Retrieving current session of unit of work {0}", GetHashCode());
                return currentSession;
            }
        }

        public ConcurrentQueue<StreamEventTuple> Queue 
        {
            get
            {
                return eventQueue;
            }
        }

    }
}
