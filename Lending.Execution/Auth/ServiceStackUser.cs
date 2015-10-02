using System;
using System.Collections.Generic;
using ServiceStack.Authentication.NHibernate;
using ServiceStack.ServiceInterface.Auth;

namespace Lending.Execution.Auth
{
    public class ServiceStackUser
    {
        public virtual long AuthenticatedUserId { get; protected set; }
        public virtual Guid UserId { get; protected set; }

        public ServiceStackUser(long authenticatedUserId, Guid userId)
        {
            AuthenticatedUserId = authenticatedUserId;
            UserId = userId;
        }

        protected ServiceStackUser()
        {
        }

    }
}
