using System.Collections.Generic;
using Lending.Core;
using Lending.Core.Model;
using Lending.Core.NewUser;
using ServiceStack.Authentication.NHibernate;
using ServiceStack.ServiceInterface.Auth;

namespace Lending.Execution.Auth
{
    public class ServiceStackUser : User
    {
        public virtual UserAuthPersistenceDto AuthenticatedUser { get; protected set; }

        private ServiceStackUser(UserAuthPersistenceDto userAuth)
            : base(userAuth.DisplayName, userAuth.PrimaryEmail)
        {
            AuthenticatedUser = userAuth;
        }

        private ServiceStackUser()
        {
        }

        public static ServiceStackUser Create(UserAuthPersistenceDto userAuth)
        {
            return new ServiceStackUser(userAuth);
        }

        public static ServiceStackUser FromEvents(IEnumerable<Event> events)
        {
            var serviceStackUser = new ServiceStackUser();
            foreach (var @event in events)
            {
                serviceStackUser.When(@event);
            }
            return serviceStackUser;
        }
    }
}
