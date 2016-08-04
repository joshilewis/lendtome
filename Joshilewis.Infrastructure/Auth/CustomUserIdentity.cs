using System;
using System.Collections.Generic;
using Nancy.Security;

namespace Joshilewis.Infrastructure.Auth
{
    public class CustomUserIdentity : IUserIdentity
    {
        public Guid Id { get; }  
        public string UserName { get; }
        public IEnumerable<string> Claims { get; }

        public CustomUserIdentity(Guid id, string userName, IEnumerable<string> claims)
        {
            Id = id;
            UserName = userName;
            Claims = claims;
        }
    }
}