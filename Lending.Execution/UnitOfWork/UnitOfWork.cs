using NHibernate;
using NHibernate.Context;
using ServiceStack.Logging;

namespace Lending.Execution.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly static ILog Log = LogManager.GetLogger(typeof(UnitOfWork).FullName);

        private readonly ISessionFactory sessionFactory;

        public UnitOfWork(ISessionFactory sessionFactory)
        {
            Log.DebugFormat("Creating unit of work {0}", GetHashCode());
            this.sessionFactory = sessionFactory;
        }

        public void Begin()
        {
            Log.DebugFormat("Beginning unit of work {0}", GetHashCode());
            currentSession = sessionFactory.OpenSession();
            CurrentSessionContext.Bind(CurrentSession);
            CurrentSession.BeginTransaction();
        }

        public void Commit()
        {
            Log.DebugFormat("Committing unit of work {0}", GetHashCode());
            currentSession.Transaction.Commit();
            CurrentSessionContext.Unbind(sessionFactory);
        }

        public void RollBack()
        {
            Log.DebugFormat("Rolling back unit of work {0}", GetHashCode());
            currentSession.Transaction.Rollback();
            CurrentSessionContext.Unbind(sessionFactory);
        }

        public void Dispose()
        {
            Log.DebugFormat("Disposing {0}", GetHashCode());
            currentSession.Transaction.Dispose();
            CurrentSession.Dispose();
        }

        private ISession currentSession;
        public ISession CurrentSession 
        {
            get
            {
                Log.DebugFormat("Retrieving current session of unit of work {0}", GetHashCode());
                return currentSession;
            }
        }

    }
}
