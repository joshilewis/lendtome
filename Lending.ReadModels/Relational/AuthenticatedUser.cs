using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace Lending.ReadModels.Relational
{
    [Table("\"AuthenticatedUser\"")]
    public class AuthenticatedUser
    {
        public virtual string Id { get; protected set; }
        public virtual string UserName { get; protected set; }
        public virtual string Email { get; protected set; }
        public virtual string Picture { get; protected set; }
        public virtual IList<AuthenticationProvider> AuthenticationProviders { get; protected set; }

        public AuthenticatedUser(string id, string userName, string email, string picture, IList<AuthenticationProvider> authenticationProviders)
        {
            Id = id;
            UserName = userName;
            Email = email;
            AuthenticationProviders = authenticationProviders;
            Picture = picture;
        }

        protected AuthenticatedUser()
        {
        }
    }
}