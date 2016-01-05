using System.Collections.Generic;
using Nancy.Security;

namespace Lending.Execution.Nancy
{
    public class AuthenticatedUser : IUserIdentity
    {
        public string UserName { get; }
        public IEnumerable<string> Claims { get; }

        public AuthenticatedUser(string userName, IEnumerable<string> claims)
        {
            UserName = userName;
            Claims = claims;
        }
    }
}