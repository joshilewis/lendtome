using System;
using System.Collections.Generic;
using Nancy.Security;

namespace Lending.Execution.Nancy
{
    public class AuthenticatedUser : IUserIdentity
    {
        public Guid Id { get; }  
        public string UserName { get; }
        public IEnumerable<string> Claims { get; }

        public AuthenticatedUser(Guid id, string userName, IEnumerable<string> claims)
        {
            Id = id;
            UserName = userName;
            Claims = claims;
        }
    }
}