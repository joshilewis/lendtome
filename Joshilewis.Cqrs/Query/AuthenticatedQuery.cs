using System;

namespace Joshilewis.Cqrs.Query
{
    public abstract class AuthenticatedQuery : Query, IAuthenticated
    {
        public new string UserId { get; set; }

        protected AuthenticatedQuery(string userId)
        {
            UserId = userId;
        }

        protected AuthenticatedQuery()
        {
        }
    }
}
