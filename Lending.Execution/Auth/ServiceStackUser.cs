using System;
using System.Collections.Generic;
using Lending.Core;
using Lending.Core.Model;
using Lending.Core.NewUser;
using ServiceStack.Authentication.NHibernate;
using ServiceStack.ServiceInterface.Auth;

namespace Lending.Execution.Auth
{
    public class ServiceStackUser
    {
        public virtual long AuthenticatedUserId { get; protected set; }
        public virtual Guid UserId { get; protected set; }

        public ServiceStackUser(UserAuthPersistenceDto userAuth, Guid userId)
        {
            AuthenticatedUserId = userAuth.Id;
            UserId = userId;
        }

        protected ServiceStackUser()
        {
        }

    }
}
