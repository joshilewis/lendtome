using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Joshilewis.Cqrs;
using NHibernate;
using NHibernate.Context;

namespace Joshilewis.Infrastructure.UnitOfWork
{
    public class NHibernateUnitOfWork : IUnitOfWork
    {
        private readonly ISessionFactory sessionFactory;

        public NHibernateUnitOfWork(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public void Begin()
        {
            CurrentSession = sessionFactory.OpenSession();
            CurrentSessionContext.Bind(CurrentSession);
            CurrentSession.BeginTransaction();
        }

        public void Commit()
        {
            CurrentSession.Transaction.Commit();

            CurrentSessionContext.Unbind(sessionFactory);
        }

        public void RollBack()
        {
            CurrentSession.Transaction.Rollback();
            CurrentSessionContext.Unbind(sessionFactory);
        }

        public void Dispose()
        {
            CurrentSession.Transaction.Dispose();
            CurrentSession.Dispose();
        }

        public ISession CurrentSession { get; private set; }

        public ISession GetSession()
        {
            return CurrentSession;
        }

    }
}
