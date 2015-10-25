using System;
using System.Collections.Generic;
using Lending.Domain.RegisterUser;
using ServiceStack.Authentication.NHibernate;
using ServiceStack.ServiceInterface.Auth;

namespace Lending.Execution.Auth
{
    public class ServiceStackUser : RegisteredUser
    {
        public virtual long AuthenticatedUserId { get; protected set; }

        public ServiceStackUser(long authenticatedUserId, Guid userId, string username)
            : base(userId, username)
        {
            AuthenticatedUserId = authenticatedUserId;
        }

        protected ServiceStackUser()
        {
        }

    }
}
