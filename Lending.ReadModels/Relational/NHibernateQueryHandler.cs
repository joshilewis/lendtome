using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Joshilewis.Cqrs;
using Joshilewis.Cqrs.Query;
using NHibernate;

namespace Lending.ReadModels.Relational
{
    public abstract class NHibernateQueryHandler<TMessage, TResult> : MessageHandler<TMessage, TResult>,
        IQueryHandler<TMessage, TResult> where TMessage : Query
    {
        private readonly Func<ISession> getSession;

        protected NHibernateQueryHandler(Func<ISession> sessionFunc)
        {
            this.getSession = sessionFunc;
        }

        protected ISession Session => getSession();
    }
}
