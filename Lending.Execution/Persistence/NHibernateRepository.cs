using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs;
using NHibernate;

namespace Lending.Execution.Persistence
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
