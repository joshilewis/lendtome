using System;
using Dapper.Contrib.Extensions;
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

        public void Save<T>(T obj) where T : class
        {
            getSession().Connection.Insert(obj);
        }

        public T Get<T>(object identifier) where T : class
        {
            return getSession().Connection.Get<T>(identifier);
        }
    }
}
