using System;
using System.Collections.Generic;
using Nancy.Security;

namespace Lending.Execution.Nancy
{
    public class User : IUserIdentity
    {
        public virtual Guid Id { get; protected set; }
        public virtual string UserName { get; protected set; }
        public virtual IEnumerable<string> Claims { get; }
        public virtual IList<AuthenticationProvider> AuthenticationProviders { get; protected set; }

        public User(Guid id, string userName, IList<AuthenticationProvider> authenticationProviders)
        {
            Id = id;
            UserName = userName;
            AuthenticationProviders = authenticationProviders;
            Claims = new List<string>();
        }

        protected User()
        {
            Claims = new List<string>();
        }

    }
}