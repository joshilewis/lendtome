using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Joshilewis.Cqrs;
using NHibernate;

namespace Lending.ReadModels.Relational
{
    public abstract class NHibernateEventHandler<TEvent> : Joshilewis.Cqrs.EventHandler<TEvent> where TEvent : Event
    {
        private readonly Func<ISession> getSession;

        protected NHibernateEventHandler(Func<ISession> sessionFunc)
        {
            this.getSession = sessionFunc;
        }

        protected ISession Session => getSession();

    }
}
