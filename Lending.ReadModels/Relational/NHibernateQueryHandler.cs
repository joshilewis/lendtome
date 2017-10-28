using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Joshilewis.Cqrs;
using Joshilewis.Cqrs.Query;
using NHibernate;

namespace Lending.ReadModels.Relational
{
    public abstract class NHibernateQueryHandler<TMessage> : MessageHandler<TMessage>,
        IQueryHandler<TMessage> where TMessage : Query
    {
        private readonly Func<ISession> getSession;

        protected NHibernateQueryHandler(Func<ISession> sessionFunc)
        {
            this.getSession = sessionFunc;
        }

        protected ISession Session => getSession();
        protected IDbConnection Connection => Session.Connection;
    }
}
