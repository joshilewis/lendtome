using System;
using System.Collections.Generic;
using Nancy.Security;

namespace Lending.Execution.Auth
{
    public class AuthenticatedUser
    {
        public virtual Guid Id { get; protected set; }
        public virtual string UserName { get; protected set; }
        public virtual IList<AuthenticationProvider> AuthenticationProviders { get; protected set; }

        public AuthenticatedUser(Guid id, string userName, IList<AuthenticationProvider> authenticationProviders)
        {
            Id = id;
            UserName = userName;
            AuthenticationProviders = authenticationProviders;
        }

        protected AuthenticatedUser()
        {
        }

    }
}