using System;
using Joshilewis.Cqrs;
using NHibernate;

namespace Joshilewis.Infrastructure.Persistence
{
    public class NHibernateRepository : IRepository
    {
        private readonly Func<ISession> getSession;

        public NHibernateRepository(Func<ISession> sessionFunc)
        {
            this.getSession = sessionFunc;
        }

        public void Save(object obj)
        {
            getSession().Save(obj);
        }

        public T Get<T>(object identifier)
        {
            return getSession().Get<T>(identifier);
        }
    }
}
