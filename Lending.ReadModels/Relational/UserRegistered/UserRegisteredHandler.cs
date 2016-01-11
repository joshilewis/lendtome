using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Domain.RegisterUser;
using NHibernate;

namespace Lending.ReadModels.Relational.UserRegistered
{
    public class UserRegisteredHandler : Lending.Cqrs.EventHandler<Domain.RegisterUser.UserRegistered>
    {
        private readonly Func<ISession> getSession;

        public UserRegisteredHandler(Func<ISession> sessionFunc)
        {
            this.getSession = sessionFunc;
        }

        public override void When(Domain.RegisterUser.UserRegistered @event)
        {
            RegisteredUser registeredUser = new RegisteredUser(@event.AggregateId, @event.UserName);
            getSession().Save(registeredUser);

        }
    }
}
