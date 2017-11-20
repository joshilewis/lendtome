using System;
using System.Collections.Generic;
using Nancy.Security;

namespace Joshilewis.Infrastructure.Auth
{
    public class CustomUserIdentity : IUserIdentity
    {
        public string Id { get; }  
        public string UserName { get; }
        public IEnumerable<string> Claims { get; }

        public CustomUserIdentity(string id, string userName, IEnumerable<string> claims)
        {
            Id = id;
            UserName = userName;
            Claims = claims;
        }
    }
}